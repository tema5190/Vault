using BankModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.Enums;
using Vault.DATA.Models;
using Vault.Services.BankOperation;

namespace Vault.Services
{
    public class BankOperationService
    {
        private readonly VaultContext _db;

        private List<RefillTransaction> newTransactions;

        public BankOperationService(VaultContext context)
        {
            _db = context;
            this.newTransactions = new List<RefillTransaction>();
        }

        #region Debit

        // Call every day at 23:00
        public async Task PerformAllTransactionsInQueue()
        {
            newTransactions = new List<RefillTransaction>();

            var goalsToPerform = await this._db.Goals.Include(g => g.CreditCard).AsNoTracking().Where(g => IsGoalsCanPerformedToday(g)).ToListAsync();

            if (goalsToPerform.Capacity == 0) return;

            var bankCards = await GetBankCardToGetTransactions(goalsToPerform.Select(g => g.CreditCard).ToList());

            for (var i = 0; i < goalsToPerform.Capacity; i++)
            {
                var goal = goalsToPerform[i];
                var bankCard = bankCards.Find(bc => bc.CardNumber == goal.CreditCard.CardNumber);

                TryToPerformTransactionAndAddInQueueList(goal, bankCard);
            }

            await this._db.Transactions.AddRangeAsync(this.newTransactions);
        }

        public async Task<bool> RetryTransaction(int transactionId)
        {
            var transaction = await _db.Transactions.Include(t => t.Goal.CreditCard).SingleOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null || transaction.TransactionIsRetried) return false;

            var goal = transaction.Goal;
            var bankCard = await GetBankCardToGetTransaction(transaction.CreditCard);

            var isCompleted = TryToPerformTransactionAndAddInQueueList(goal, bankCard);

            if (!isCompleted) return false;
            
            transaction.IsPausedOrError = false;
            transaction.TransactionIsRetried = true; // for view old transaction in list
            await _db.Transactions.AddRangeAsync(this.newTransactions);
            await this._db.SaveChangesAsync();
            return true;
        }

        public bool TryToPerformTransactionAndAddInQueueList(Goal target, BankCard card)
        {
            if (CheckIsCardBlocked(card))
            {
                AddCorruptedTransaction(target, "Card is blocked");
                return false;
            }

            var money = GetMoneyFromBankCard(card, target.MoneyPerMonth);
            if (money == null)
            {
                AddCorruptedTransaction(target, "Not enought money");
                return false;
            }

            AddTransaction(target, money.Value);
            return true;
        }

        public async Task<bool> TryToPerformTransaction(Goal target)
        {
            var bankCard = await GetBankCardToGetTransaction(target.CreditCard);
            return TryToPerformTransactionAndAddInQueueList(target, bankCard);
        }

        private void AddCorruptedTransaction(Goal target, string status = null)
        {
            this.newTransactions.Add(new RefillTransaction()
            {
                Goal = target,
                TransactionDateTime = DateTime.Now,
                CreditCard = target.CreditCard,
                IsPausedOrError = true,
                TransactionIsRetried = true,
                Status = status ?? "Unknown Error",
            });
        }

        private void AddTransaction(Goal target, decimal money)
        {
            this.newTransactions.Add(new RefillTransaction()
            {
                Goal = target,
                TransactionDateTime = DateTime.Now,
                CreditCard = target.CreditCard,
                IsPausedOrError = false,
                TransactionIsRetried = false,
                Money = money,
            });
        }

        private bool CheckIsCardBlocked(BankCard bankCard){
            return bankCard.IsBlocked;
        }

        private decimal? GetMoneyFromBankCard(BankCard bankCard, decimal money)
        {
            if (bankCard.Balance < money) return null;

            bankCard.Balance -= money;
            return money;
        }

        private async Task<List<BankCard>> GetBankCardToGetTransactions(List<UserCard> userCards)
        {
            return await _db.BankCards.Where(bc => userCards.Any(uc => uc.CardNumber == bc.CardNumber)).ToListAsync();
        }

        private async Task<BankCard> GetBankCardToGetTransaction(UserCard userCard)
        {
            return await _db.BankCards.SingleOrDefaultAsync(bc => bc.CardNumber == userCard.CardNumber);
        }

        private static bool IsGoalsCanPerformedToday(Goal goal)
        {
            var todayDay = DateTime.Now.Day;

            //var isInThisMonthDaysEaserThenGoalDate =
            //    DateTime.DaysInMonth(goal.TargetStart.Year, goal.TargetStart.Month) >
            //    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);  // TODO: trying to solve this in future (maybe)

            return goal.ChargeDate.Day == todayDay;
        }

        #endregion

        #region Calculate Profit

        // Call every month on first day of month
        public async Task CalculateGoalsSumWithProfit()
        {
            await _db.Goals.Where(g => !g.IsPaused).ForEachAsync(g =>
            {
                g.CurrentMoney += CountProfitHelper.CalculateProfitPerMonth(g);
            });
            await _db.SaveChangesAsync();
        }

        #endregion
    }
}
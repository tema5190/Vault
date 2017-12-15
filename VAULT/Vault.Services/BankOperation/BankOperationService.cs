using BankModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool AddBankCard(UserCard userCard)
        {
            var bankCard = new BankCard(userCard);
            return AddBankCard(bankCard);
        }

        public bool AddBankCard(BankCard bankCard)
        {
            _db.BankCards.Add(bankCard);
            _db.SaveChanges();

            return true;
        }

        public bool UpdateBankCard(BankCard bankCard)
        {
            var exist = _db.BankCards.SingleOrDefault(c => c.CardNumber == bankCard.CardNumber);

            if (exist == null) return false;

            exist.Balance = bankCard.Balance;
            exist.IsBlocked = bankCard.IsBlocked;

            _db.SaveChanges();

            return true;
        }

        #region Debit

        // Call every day at 23:00
        public void PerformAllTransactionsInQueue(bool performALL = false)
        {
            //newTransactions = new List<RefillTransaction>();

            List<Goal> goalsToPerform;

            if (!performALL)
                goalsToPerform = this._db.Goals
                   .Include(g => g.CreditCard)
                   .Where(g => IsGoalsCanPerformedToday(g)).ToList();
            else
            {
                goalsToPerform = this._db.Goals.Include(g => g.CreditCard).ToList();
            }

            if (goalsToPerform.Count == 0) return;

            var bankCards = GetBankCardToGetTransactions(goalsToPerform.Select(g => g.CreditCard).ToList());

            for (var i = 0; i < goalsToPerform.Count; i++)
            {
                var goal = goalsToPerform[i];
                var bankCard = bankCards.Find(bc => bc.CardNumber == goal.CreditCard.CardNumber);

                TryToPerformTransactionAndAddInSaveList(goal, bankCard);
            }

            this._db.Transactions.AddRange(this.newTransactions);

            _db.SaveChanges();

            newTransactions = new List<RefillTransaction>();
        }

        public async Task<bool> RetryTransaction(int transactionId)
        {
            var transaction = await _db.Transactions.Include(t => t.Goal.CreditCard).SingleOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null || transaction.TransactionIsRetried) return false;

            var goal = transaction.Goal;
            var bankCard = await GetBankCard(transaction.CreditCard);

            var isCompleted = TryToPerformTransactionAndAddInSaveList(goal, bankCard);

            if (!isCompleted) return false;

            transaction.IsPausedOrError = false;
            transaction.TransactionIsRetried = true; // for view old transaction in list
            await _db.Transactions.AddRangeAsync(this.newTransactions);
            await this._db.SaveChangesAsync();
            return true;
        }

        public bool TryToPerformTransactionAndAddInSaveList(Goal target, BankCard card)
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

            target.CurrentMoney += money.Value;
            AddTransaction(target, money.Value);
            return true;
        }

        public async Task<bool> TryToPerformTransaction(Goal target)
        {
            var bankCard = await GetBankCard(target.CreditCard);
            return TryToPerformTransactionAndAddInSaveList(target, bankCard);
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
                Money = 0,
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

        private bool CheckIsCardBlocked(BankCard bankCard)
        {
            return bankCard.IsBlocked;
        }

        private decimal? GetMoneyFromBankCard(BankCard bankCard, decimal money)
        {
            if (bankCard.Balance < money) return null;

            bankCard.Balance = bankCard.Balance - money;
            return money;
        }

        private List<BankCard> GetBankCardToGetTransactions(List<UserCard> userCards)
        {
            return _db.BankCards.Where(bc => userCards.Any(uc => uc.CardNumber == bc.CardNumber)).ToList();
        }

        public async Task<BankCard> GetBankCard(UserCard userCard)
        {
            return await _db.BankCards.SingleOrDefaultAsync(bc => bc.CardNumber == userCard.CardNumber);
        }

        private static bool IsGoalsCanPerformedToday(Goal goal)
        {
            var todayDay = DateTime.Now.Day;

            //var isInThisMonthDaysEaserThenGoalDate =
            //    DateTime.DaysInMonth(goal.TargetStart.Year, goal.TargetStart.Month) >
            //    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);  // TODO: trying to solve this in future (maybe)

            //return true;

            return goal.ChargeDay == todayDay;
        }

        #endregion

        #region Calculate Profit

        // Call every month on first day of month
        public bool CalculateGoalsSumWithProfit()
        {
            var goals = _db.Goals.Where(g => !g.IsPaused).ToList();

            goals.ForEach(g =>
            {
                g.CurrentMoney += CountProfitHelper.CalculateProfitPerMonth(g);
            });

            _db.SaveChanges();

            return true;
        }

        public Tuple<decimal, int> CalculateProfitSpeed(int month, decimal permonth, TargetType type, decimal target = 0)
        {
            decimal sum = 0;
            int counter = 0;
            while (sum < target)
            {
                counter++;
                sum += permonth;
                sum += CountProfitHelper.CalculateProfitPerMonth(sum, type);
            }

            return new Tuple<decimal, int>(sum, counter);
        }

        public decimal CalculateProfit(decimal permonth, TargetType type, int month)
        {
            decimal sum = 0;
            for(var i = 0; i < month; i++)
            {
                sum += permonth;
                var profit = CountProfitHelper.CalculateProfitPerMonth(sum, type);
                sum += profit;
            }

            return sum;
        }
        #endregion
    }
}
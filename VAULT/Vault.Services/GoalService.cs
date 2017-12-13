using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.DTOs.Goal;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class GoalService
    {
        private readonly VaultContext _db;
        private readonly BankOperationService _bankOperationService;

        public GoalService(VaultContext context, BankOperationService bankOperationService)
        {
            this._db = context;
            this._bankOperationService = bankOperationService;
        }

        public IList<GoalDto> GetAllUserGoals(string userName)
        {
            return _db.Users.Include(u => u.ClientInfo.Goals).AsNoTracking().Single(u => u.UserName == userName).ClientInfo.Goals.Select(g => new GoalDto()
            {
                CreditCardId = g.CreditCardId,
                Description = g.Description,
                GoalId = g.Id,
                IsPaused = g.IsPaused,
                MoneyCurrent = g.CurrentMoney,
                MoneyPerMonth = g.MoneyPerMonth,
                MoneyTarget = g.MoneyTarget,
                TargetEnd = g.TargetEnd,
                TargetType = g.TargetType,
                Title = g.Title,
                ChargeDate = g.ChargeDay,
            }).ToList();
        }

        public async Task<bool> DeleteGoal(string userName, int goalId, int cardId)
        {
            var user = _db.Users.Include(u => u.ClientInfo.Goals).Include(u => u.ClientInfo.Cards).Single(u => u.UserName == userName);

            if (user == null) return false;

            var goal = user.ClientInfo.Goals.SingleOrDefault(g => g.Id == goalId);

            if (goal == null) return false;

            var card = user.ClientInfo.Cards.SingleOrDefault(c => c.Id == cardId);

            if (card == null) return false;

            var bankCard = await _bankOperationService.GetBankCard(card);

            if (bankCard == null) return false;

            bankCard.Balance += goal.CurrentMoney;

            user.ClientInfo.Goals.Remove(goal);

            await _db.SaveChangesAsync();

            return true;
        }

        public bool CreateGoal(string userName, Goal newGoal)
        {
            var user = _db.Users.Include(u => u.ClientInfo.Goals).Include(u => u.ClientInfo.Cards).Single(u => u.UserName == userName);

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    user.ClientInfo.Goals.Add(newGoal);

                    _db.SaveChanges();
                    transaction.Commit(); // auto roll back if exception was catched ? TODO: check this out
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }

            }

            return true;
        }

        public bool CreateGoal(string userName, GoalDto newGoal)
        {
            var goal = new Goal()
            {
                CreditCardId = newGoal.CreditCardId,
                Title = newGoal.Title,
                Description = newGoal.Description,
                IsPaused = newGoal.IsPaused,

                CurrentMoney = 0,
                MoneyTarget = newGoal.MoneyTarget,
                MoneyPerMonth = newGoal.MoneyPerMonth,

                ChargeDay = newGoal.ChargeDate,
                TargetStart = DateTime.Now,
                TargetEnd = newGoal.TargetEnd,
                TargetType = newGoal.TargetType,
            };

            this.CreateGoal(userName, goal);

            return true;
        }

        public bool UpdateGoal(string userName, GoalDto goalForUpdate)
        {
            var user = _db.Users.Include(u => u.ClientInfo.Goals).Single(u => u.UserName == userName);

            var goal = user.ClientInfo.Goals.SingleOrDefault(g => g.Id == goalForUpdate.GoalId);

            if (goal == null) return false;

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    goal.Title = goalForUpdate.Title;
                    goal.Description = goalForUpdate.Description;
                    goal.CreditCardId = goalForUpdate.CreditCardId;
                    goal.ChargeDay = goalForUpdate.ChargeDate;

                    goal.IsPaused = goalForUpdate.IsPaused;

                    goalForUpdate.MoneyPerMonth = goalForUpdate.MoneyPerMonth;
                    goalForUpdate.MoneyTarget = goalForUpdate.MoneyTarget;

                    _db.SaveChanges();
                    transaction.Commit(); // auto roll back ? TODO: check this out
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }
    }
}

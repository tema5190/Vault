using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vault.DATA;
using Vault.DATA.DTOs.Goal;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class GoalService
    {
        private readonly VaultContext _db;

        public GoalService(VaultContext context)
        {
            this._db = context;
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
                ChargeDate = g.ChargeDate,
            }).ToList();
        }

        public bool CreateGoal(string userName, GoalDto newGoal)
        {
            var user = _db.Users.Include(u => u.ClientInfo.Goals).Include(u => u.ClientInfo.Cards).Single(u => u.UserName == userName);

            using (var transaction = _db.Database.BeginTransaction())
            {
                try { 
                    user.ClientInfo.Goals.Add(new Goal()
                    {
                        CreditCardId = newGoal.CreditCardId,
                        Title = newGoal.Title,
                        Description = newGoal.Description,
                        IsPaused = newGoal.IsPaused,

                        CurrentMoney = 0,
                        MoneyTarget = newGoal.MoneyTarget,
                        MoneyPerMonth = newGoal.MoneyPerMonth,

                        ChargeDate = newGoal.ChargeDate,
                        TargetStart = DateTime.Now,
                        TargetEnd = newGoal.TargetEnd,
                        TargetType = newGoal.TargetType,
                    });

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
                    goal.ChargeDate = goalForUpdate.ChargeDate;

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

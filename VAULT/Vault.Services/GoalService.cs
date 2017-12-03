using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool CreateGoal(string userName, GoalDto newGoal)
        {
            var user = _db.Users.Include(u => u.ClientInfo.Goals).Include(u => u.ClientInfo.Cards).Single(u => u.UserName == userName);
            user.ClientInfo.Goals.Add(new Goal()
            {
                CreditCardId = newGoal.CreditCardId,
                Title = newGoal.Title,
                Description = newGoal.Description,
                IsPaused = newGoal.IsPaused,

                MoneyCurrent = 0,
                MoneyTarget = newGoal.MoneyTarget,
                MoneyPerMonth = newGoal.MoneyPerMonth,

                TargetStart = DateTime.Now,
                TargetEnd = newGoal.TargetEnd,
                TargetType = newGoal.TargetType,
            });

            _db.SaveChanges();

            return true;
        }

        public IList<GoalDto> GetAllUserGoals(string userName)
        {
            return _db.Users.Include(u => u.ClientInfo.Goals).Single(u => u.UserName == userName).ClientInfo.Goals.Select(g => new GoalDto()
            {
                CreditCardId = g.CreditCardId,
                Description = g.Description,
                GoalId = g.Id,
                IsPaused = g.IsPaused,
                MoneyCurrent = g.MoneyCurrent,
                MoneyPerMonth = g.MoneyPerMonth,
                MoneyTarget = g.MoneyTarget,
                TargetEnd = g.TargetEnd,
                TargetType = g.TargetType, 
                Title = g.Title,
            }).ToList();
        }
    }
}

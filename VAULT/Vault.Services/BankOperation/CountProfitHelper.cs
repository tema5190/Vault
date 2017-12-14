using System;
using Vault.DATA.Enums;
using Vault.DATA.Models;

namespace Vault.Services.BankOperation
{
    public static class CountProfitHelper
    {
        public static decimal CalculateProfitPerMonth(Goal goal, TargetType? targetType = null)
        {
            var target = targetType ?? goal.TargetType;
            var money = goal.CurrentMoney;
            var profitPercent = ((decimal)target / 10) / 100;
            var finalProfit = money * profitPercent;

            return finalProfit;
        }

        public static decimal CalculateProfitAllTime(Goal goal, TargetType? targetType = null)
        {
            var tsp = new TimeSpan();
            tsp = goal.TargetEnd - goal.TargetStart;
            var accruals = tsp.TotalDays / 30;

            decimal profitSum = 0m;
            for (var i = 0; i < accruals; i++)
            {
                profitSum += CalculateProfitPerMonth(goal, targetType);
            }

            return profitSum;
        }

        public static decimal CalculateEndMoneySum(Goal goal, TargetType? targetType = null)
        {
            return goal.CurrentMoney + CalculateProfitAllTime(goal, targetType);
        }
    }
}

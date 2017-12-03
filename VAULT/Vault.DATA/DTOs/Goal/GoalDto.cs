using System;
using Vault.DATA.Enums;

namespace Vault.DATA.DTOs.Goal
{
    public class GoalDto
    {
        public int GoalId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal MoneyTarget { get; set; }
        public decimal MoneyPerMonth { get; set; }
        public decimal MoneyCurrent { get; set; }
        public TargetType TargetType { get; set; }
        public DateTime TargetEnd { get; set; }

        public DateTime ChargeDate { get; set; }

        public bool IsPaused { get; set; }
        public int? CreditCardId { get; set; }
    }
}

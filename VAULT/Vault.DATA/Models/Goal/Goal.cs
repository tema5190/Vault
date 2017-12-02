using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vault.DATA.Enums;

namespace Vault.DATA.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public decimal MoneyTarget { get; set; }
        public decimal MoneyCurrent { get; set; }

        public bool IsPaused { get; set; }

        public TargetType TargetType { get; set; }
        public DateTime TargetStart { get; set; }
        public DateTime TargetEnd { get; set; }

        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}

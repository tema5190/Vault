﻿using System;
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
        public decimal MoneyPerMonth { get; set; }
        public decimal CurrentMoney { get; set; }

        public bool IsPaused { get; set; }

        [DataType(DataType.Date)]
        [Range(1,25)]
        public int ChargeDay { get; set; } // day of month

        public TargetType TargetType { get; set; }

        [DataType(DataType.Date)]
        public DateTime TargetStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime TargetEnd { get; set; }

        public int? CreditCardId { get; set; }
        public UserCard CreditCard { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vault.DATA.Enums;
using Vault.DATA.Models;

namespace Vault.DATA
{
    public class UserCard
    {
        [Key]
        public int Id { get; set; }

        public string CustomCardName { get; set; }

        public string OwnerFullName { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public CardType CardType { get; set; }

        public string CVV { get; set; }

        public bool IsPaused { get; set; }
        //
        public VaultUser Owner { get; set; }
        public IList<RefillTransaction> Transactions { get; set; }
        public IList<Goal> Goals { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vault.DATA.Enums;
using Vault.DATA.Models;

namespace Vault.DATA
{
    public class CreditCard
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string CardNumber { get; set; }

        public DateTime RefillDate { get; set; }

        public CartType CardType { get; set; }

        public string CVV { get; set; }

        public decimal CardBalance { get; set; }

        public bool IsPaused { get; set; }
        //
        public VaultUser Owner { get; set; }
        public IList<RefillTransaction> Transactions { get; set; }
    }
}

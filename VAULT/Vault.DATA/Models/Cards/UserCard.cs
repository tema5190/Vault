using System;
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

        [Required]
        public string OwnerFullName { get; set; }

        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public CardType CardType { get; set; }

        [StringLength(3)]
        public string CVV { get; set; }

        public bool IsPaused { get; set; }

        //
        public VaultUser Owner { get; set; }
        public IList<RefillTransaction> Transactions { get; set; }
        public IList<Goal> Goals { get; set; }
    }
}

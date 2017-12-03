using System;
using System.ComponentModel.DataAnnotations;
using Vault.DATA.Enums;

namespace BankModel
{
    public class BankCard
    {
        [Key]
        public int Id { get; set; }

        public string OwnerFullName { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public CardType CardType { get; set; }

        public string CVV { get; set; }

        public decimal Balance { get; set; }

        public bool IsBlocked { get; set; }
    }
}

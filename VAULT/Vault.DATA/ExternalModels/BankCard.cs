using System;
using System.ComponentModel.DataAnnotations;
using Vault.DATA;
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

        public BankCard()
        {

        }

        public BankCard(UserCard userCard, decimal balance = 0, bool isBlocked = false)
        {
            OwnerFullName = userCard.OwnerFullName;
            CardNumber = userCard.CardNumber;
            ExpirationDate = userCard.ExpirationDate;
            CardNumber = userCard.CardNumber;
            CVV = userCard.CVV;
            Balance = balance;
            IsBlocked = isBlocked;
        }
    }
}

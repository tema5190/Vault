using System;
using System.Text;
using Vault.DATA.Enums;

namespace Vault.DATA.DTOs.Cards
{
    public class CreditCardDto
    {
        public int? CreditCardId { get; set; }

        public string CustomCardName { get; set; }

        public string CardNumber { get; set; }
        public string OwnerFullName { get; set; }
        public CardType CardType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }

        public CreditCardDto(CreditCard card)
        {
            CreditCardId = card.Id;
            CustomCardName = card.CustomCardName;
            CardNumber = SecureCardInfo(card.CardNumber);
            OwnerFullName = card.OwnerFullName;
            CardType = card.CardType;
        }

        private string SecureCardInfo(string cardNumber)
        {
            var sb = new StringBuilder();
            sb.Append("**** **** **** ");
            sb.Append(cardNumber.Substring(cardNumber.Length - 4, 4));
            return sb.ToString();
        }
    }
}

using System;
using Vault.DATA.Enums;

namespace Vault.DATA.DTOs.Cards
{
    public class CreditCardDto
    {
        public string CustomCardName { get; set; }

        public string CardNumber { get; set; }
        public string OwnerFullName { get; set; }
        public CardType CardType { get; set; }
        public DateTime RefillDate { get; set; }
        public string CVV { get; set; }
    }
}

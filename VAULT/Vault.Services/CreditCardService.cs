using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.DTOs.Cards;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class CreditCardService
    {
        private readonly UserService _userService;
        private readonly VaultContext _db;

        public CreditCardService(UserService userService, VaultContext vaultContext)
        {
            this._userService = userService;
            this._db = vaultContext;
        }

        public async Task<IList<CreditCard>> GetUserCards(string userName)
        {
            var user = await _db.Users
                .Include(u => u.ClientInfo.Cards)
                .SingleAsync(u => u.UserName == userName);

            return user.ClientInfo.Cards;
        }

        public async Task<bool> AddUserCard(string userName, CreditCardDto newCardDto)
        {
            var user = await _db.Users.Include(u => u.ClientInfo.Cards).SingleAsync(u => u.UserName == userName);

            var newCard = new CreditCard()
            {
                CardBalance = 0m,
                CardNumber = newCardDto.CardNumber,
                CardType = newCardDto.CardType,
                CVV = newCardDto.CVV,
                IsPaused = false,
                OwnerFullName = newCardDto.OwnerFullName,
                CustomCardName = newCardDto.CustomCardName,
                Owner = user,
                RefillDate = newCardDto.RefillDate,
            };


            user.ClientInfo.Cards.Add(newCard);
            await _db.SaveChangesAsync();

            return true;
        }

    }                                          
}

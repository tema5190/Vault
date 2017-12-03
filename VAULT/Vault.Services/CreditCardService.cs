using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<CreditCardDto>> GetUserCards(string userName)
        {
            var user = await _db.Users
                .Include(u => u.ClientInfo.Cards)
                .AsNoTracking()
                .SingleAsync(u => u.UserName == userName);

            return user.ClientInfo.Cards.Select(c => new CreditCardDto(c)).ToList();
        }

        public async Task<CreditCardDto> GetCreditCardById(string userName, int id)
        {
            var user = await _db.Users.Include(u => u.ClientInfo.Cards).FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return null;

            var card = user.ClientInfo.Cards.FirstOrDefault(c => c.Id == id);

            if (card == null) return null;

            return new CreditCardDto(card);
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
                ExpirationDate = newCardDto.ExpirationDate,
            };

            user.ClientInfo.Cards.Add(newCard);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserCard(string userName, int cardId)
        {
            var user = await _db.Users.Include(u => u.ClientInfo.Cards)
                                        .Include(u => u.ClientInfo.Goals)
                                        .FirstOrDefaultAsync(u => u.UserName == userName);

            var card = user.ClientInfo.Cards.SingleOrDefault(c => c.Id == cardId);

            if (card == null)
            {
                return false;
            }
            card.Goals.All(g => DeleteCardAndPauseGoal(g));
            user.ClientInfo.Cards.Remove(card);

            await _db.SaveChangesAsync();

            return true;
        }

        private bool DeleteCardAndPauseGoal(Goal goal)
        {
            goal.IsPaused = true;
            goal.CreditCard = null;
            goal.CreditCardId = null;

            return true;
        }

    }                                          
}

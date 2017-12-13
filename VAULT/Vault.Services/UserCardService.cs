using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.DTOs.Cards;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class UserCardService
    {
        private readonly UserService _userService;
        private readonly BankOperationService _bankOperationService;
        private readonly VaultContext _db;

        public UserCardService(UserService userService, VaultContext vaultContext, BankOperationService operationService)
        {
            this._userService = userService;
            this._db = vaultContext;
            this._bankOperationService = operationService;
        }

        public async Task<IList<UserCardDto>> GetUserCardsByUserName(string userName)
        {
            var user = await _db.Users
                .Include(u => u.ClientInfo.Cards)
                .AsNoTracking()
                .SingleAsync(u => u.UserName == userName);

            return user.ClientInfo.Cards.Select(c => new UserCardDto(c)).ToList();
        }

        public async Task<IList<UserCard>> GetUserCardsByUserId(int id)
        {
            var user = await _db.Users
                .Include(u => u.ClientInfo.Cards)
                .AsNoTracking()
                .SingleAsync(u => u.Id == id);

            return user.ClientInfo.Cards;
        }

        public async Task<UserCardDto> GetCreditCardDtoById(string userName, int cardId)
        {
            return new UserCardDto(await GetCreditCardById(userName, cardId));
        }

        public async Task<UserCard> GetCreditCardById(string userName, int cardId)
        {
            var user = await _db.Users.Include(u => u.ClientInfo.Cards).FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return null;

            var card = user.ClientInfo.Cards.FirstOrDefault(c => c.Id == cardId);

            if (card == null) return null;

            return card;
        }

        public async Task<bool> AddUserCard(string userName, UserCardDto newCardDto)
        {
            var user = await _db.Users.Include(u => u.ClientInfo.Cards).SingleAsync(u => u.UserName == userName);

            var newCard = new UserCard()
            {
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

            // TODO: I AM HERE
            _bankOperationService.AddBankCard(newCard);

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

            if (card.Goals != null && card.Goals.Count != 0)
                card.Goals.All(g => DeleteCardAndPauseGoal(g));

            user.ClientInfo.Cards.Remove(card);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SwitchCardPause(string userName, int cardId)
        {
            var card = await GetCreditCardById(userName, cardId);
            card.IsPaused = card.IsPaused == true ? false : true;
            var switchCompletedGoalCompleted = SwitchAllCardGoals(card);
            await _db.SaveChangesAsync();

            return true && switchCompletedGoalCompleted;
        }

        private bool DeleteCardAndPauseGoal(Goal goal)
        {
            goal.IsPaused = true;
            goal.CreditCard = null;
            goal.CreditCardId = null;

            return true;
        }

        private bool SwitchAllCardGoals(UserCard card)
        {
            if (card == null) return false;

            if (card.Goals != null && card.Goals.Count != 0)
            {         
                card.Goals.All(g => { g.IsPaused = g.IsPaused ? false : true; return true; });
            }
            return true;
        }

    }                                          
}

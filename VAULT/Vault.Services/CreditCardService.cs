using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vault.DATA;
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

        public async Task<bool> AddUserCard(int userId, CreditCard newCard)
        {
            var user = await _db.Users.Include(u => u.ClientInfo.Cards).SingleAsync(u => u.Id == userId);

            user.ClientInfo.Cards.Add(newCard);
            await _db.SaveChangesAsync();

            return true;
        }

    }                                          
}

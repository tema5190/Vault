using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class UserService
    {
        private readonly VaultContext _context;

        public UserService(VaultContext context){
            this._context = context;
        }

        public async Task<bool> IsUserExist(string login)
        {
            return await this.GetUserByLogin(login) == null ? false : true;
        }

        public async Task<VaultUser> GetUserByLogin(string login)
        {
            var existUser = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == login);

            if(existUser != null)
            {
                return existUser;
            }

            return null;
        }

        public async Task<VaultUser> GetUserById(int id)
        {
            var existUser = await this._context.Users
                .Include(u => u.ClientInfo.Cards)
                .Include(u => u.ClientInfo.Goals)
                .Include(u => u.ClientInfo.Transactions)
                .SingleOrDefaultAsync(u => u.Id == id);
            return existUser;
        }

        public async Task<bool> DeleteUserById(int id)
        {

            var userForDelete = await GetUserById(id);
            _context.Users.Remove(userForDelete);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

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

        public async Task<User> GetUserByLogin(string login)
        {
            var existUser = await this._context.Users.FirstOrDefaultAsync(u => u.UserName == login);

            if(existUser != null)
            {
                return existUser;
            }

            return null;
        }
    }
}

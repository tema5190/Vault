using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.Enums;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class AuthService
    {
        private readonly VaultContext _context;
        private readonly UserService _userService;

        public AuthService(VaultContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<VaultUser> Login(string login, string password)
        {
            return await GetUser(login, password);
        }

        private async Task<VaultUser> GetUser(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == login);

            if(user != null && password == user.Password)
            {
                return user;
            }
            return null;
        }


    }
}

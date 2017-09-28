using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class AuthService
    {
        private readonly VaultContext _context;

        public AuthService(VaultContext context)
        {
            _context = context;
        }

        public async Task<ClaimsIdentity> GetUserClaimsIdentity(string login, string password)
        {
            var user = await GetUserIsExistAndPasswordValid(login, password);
            if (user != null)
            {
                var claims = this.GetUserClaims(user);
                return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }

            return null;
        }

        private IEnumerable<Claim> GetUserClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            };
        }

        public async Task<User> GetUserIsExistAndPasswordValid(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == login);

            if (MD5.Create(password).ToString() == user.PasswordHash)
                return user;

            return null;
        }            
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.Enums;
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

        public async Task<ClaimsIdentity> Login(string login, string password)
        {
            var user = await GetUser(login, password);
            return GetIdentity(user);
        }

        private async Task<User> GetUser(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == login);

            if(user != null && CalculateMD5Hash(password) == user.PasswordHash)
            {
                return user;
            }
            return null;
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            if (user == null) return null;

            var claims = new List<Claim>
            {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, Enum.GetName(typeof(UserRoles), user.Role))
            };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private string CalculateMD5Hash(string input)
        {           
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}

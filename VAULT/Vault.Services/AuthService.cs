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

        public async Task<ClaimsIdentity> Login(string login, string password)
        {
            var user = await GetUser(login, password);
            return GetIdentity(user);
        }

        private async Task<User> GetUser(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == login);

            if(user != null && password == user.Password)
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
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, Enum.GetName(typeof(UserRoles), user.Role))
            };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        //private string CalculateMD5Hash(string input)
        //{           
        //    MD5 md5 = MD5.Create();
        //    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        //    byte[] hash = md5.ComputeHash(inputBytes);
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < hash.Length; i++)
        //    {
        //        sb.Append(hash[i].ToString("X2"));
        //    }
        //    return sb.ToString().ToLower();
        //}

        //public async Task<bool> Register(RegisterData registerData)
        //{
        //    if (!await _userService.IsUserExist(registerData.Login))
        //    {

        //        this._context.Users.Add(new User
        //        {
        //            UserName = registerData.Login,
        //            IsActive = true,
        //            Password = this.CalculateMD5Hash(registerData.Password),
        //            Role = UserRoles.Client,
        //        });
        //        await this._context.SaveChangesAsync();
        //    }
        //    return false;
        //}        
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vault.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Vault.API.Infrastructure.JWT;
using Vault.DATA.DTOs;
using Vault.DATA.Models;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using Vault.DATA.Enums;
using Vault.DATA.DTOs.Auth;

namespace Vault.API.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService service) : base()
        {
            _authService = service;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task Login([FromBody] dynamic loginData)
        {
            var login = (string)loginData.login;
            var password = (string)loginData.password;

            var user = await _authService.Login(login, password);

            Response.ContentType = "application/json";

            if (!user.IsRegistrationFinished) { 
                await Response.WriteAsync(JsonConvert.SerializeObject(new AuthResponse() { isRegistrationFinished = false }));
                return;
            }

            var identity = GetIdentity(user);

            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var token = JwtHelper.CreateToken(identity);
            await Response.WriteAsync(JsonConvert.SerializeObject(token, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(VaultUser user)
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

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<bool> Register([FromBody] RegisterData registerData)
        //{
        //    return await this._authService.Register(registerData);           
        //}
    }
}

﻿using System.Threading.Tasks;
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
using Vault.DATA.DTOs.Registration;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
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
        public LoginResponse Login([FromBody] dynamic loginData)
        {
            var result = new LoginResponse() { IsError = true };
            var login = (string)loginData.login;
            var password = (string)loginData.password;

            var user = _authService.Login(login, password);

            if (user == null)
            {
                return result;
            }

            if (!user.IsRegistrationFinished) {
                result.IsRegistrationNotFinished = true;
                result.IsError = false;
                return result;
            }

            var identity = GetIdentity(user);

            if (identity == null)
            {
                return result;
            }

            result.IsError = false;
            result.IsWaitEmailKey = true;
            _authService.RequestEmailKey(user);
            return result;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public LoginResponse Token([FromBody] dynamic loginData)
        {
            var emailKey = (string)loginData.emailKey;

            var result = new LoginResponse();
            var user = _authService.GetUserByEmailKey(emailKey);
            var identity = GetIdentity(user);

            if (identity == null) {
                result.IsError = true;
                return result;
            };

            result.Token = JwtHelper.CreateToken(identity);
            return result;
        }

        [HttpPost("register/step-one")]
        [AllowAnonymous]
        public FirstStepResultDto FirstStepRegister([FromBody] FirstStepRegisterData firstStep)
        {
            return _authService.FirstStepRegister(firstStep);
        }

        [HttpPost("register/step-two")]
        [AllowAnonymous]
        public bool SecondStepRegister([FromBody] SecondStepRegisterData secondStep)
        {
            return _authService.SecondStepRegister(secondStep);
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
    }
}

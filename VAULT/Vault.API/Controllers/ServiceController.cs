using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Vault.Services;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("service")]
    public class ServiceController : Controller
    {
        private readonly VaultContext db;
        private readonly VaultContextInitializer initializer;

        private readonly SmsService smsService;

        public ServiceController(VaultContext db, SmsService smsService)
        {
            this.db = db;
            this.initializer = new VaultContextInitializer(this.db);
            this.smsService = smsService;
        }

        [HttpGet("db-redrop")]
        [AllowAnonymous]
        public void DropDb()
        {
            this.db.Database.EnsureDeleted();
            this.db.Database.EnsureCreated();
            this.initializer.Seed();
        }

        [HttpGet("ping")]
        [AllowAnonymous]
        public string Ping()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        [HttpGet("sms/{to}")]
        [AllowAnonymous]
        public void SendSms(string to)
        {
            this.smsService.SendLoginVerification(to, GetRandomEmailKey(6));
        }

        private static string GetRandomEmailKey(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
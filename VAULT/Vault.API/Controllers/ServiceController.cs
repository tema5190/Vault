using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Microsoft.AspNetCore.Authorization;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("service/admin")]
    public class ServiceController : Controller
    {
        private readonly VaultContext db;
        private readonly VaultContextInitializer initializer;

        public ServiceController(VaultContext db)
        {
            this.db = db;
            this.initializer = new VaultContextInitializer(this.db);
        }

        [HttpGet("NENADO")]
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
    }
}
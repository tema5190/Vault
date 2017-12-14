using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Microsoft.AspNetCore.Authorization;
using Vault.Services;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("service")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly BankOperationService bankOperationService;

        public ServiceController(BankOperationService service)
        {
            bankOperationService = service;
        }

        [HttpGet("ping")]
        [AllowAnonymous]
        public string Ping()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        [HttpGet("PERFORM")]
        [AllowAnonymous]
        public bool Perform()
        {
            try
            {
                bankOperationService.PerformAllTransactionsInQueue();
            }
            catch
            {
                return false;
            }

            return true;
        }

        [HttpGet("PROFIT")]
        [AllowAnonymous]
        public bool Profit()
        {
            try
            {
               bankOperationService.CalculateGoalsSumWithProfit();
            }
            catch
            {
                return false;
            }

            return true;
        }

        //[HttpGet("db-redrop")]
        //[AllowAnonymous] // TODO: remove for prod (L1)
        //public void DropDb()
        //{
        //    this.db.Database.EnsureDeleted();
        //    this.db.Database.EnsureCreated();
        //    this.initializer.Seed();
        //}
    }
}
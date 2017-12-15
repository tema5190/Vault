using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Microsoft.AspNetCore.Authorization;
using Vault.Services;
using Vault.DATA.Enums;

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
        public bool Perform(bool isAll = false)
        {
            try
            {
                bankOperationService.PerformAllTransactionsInQueue(isAll);
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

        [HttpGet("ProfitSpeed")]
        [AllowAnonymous]
        public string Speed(decimal perMonth, int? month, TargetType type, decimal target)
        {
            var result = bankOperationService.CalculateProfitSpeed(0, perMonth, type, target);

            return $"{result.Item1} за {result.Item2} месяца(ев)";
        }

        [HttpGet("ProfitSpeed2")]
        [AllowAnonymous]
        public decimal Speed2(decimal perMonth, int month, TargetType type)
        {
            return bankOperationService.CalculateProfit(perMonth, type, month);
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
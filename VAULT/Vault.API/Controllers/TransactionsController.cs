using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.Services;
using Vault.DATA.Models;
using Microsoft.AspNetCore.Authorization;
using Vault.DATA.DTOs.Transaction;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("transactions")]
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly TransactionsService _transactionsService;

        public TransactionsController(TransactionsService service)
        {
            this._transactionsService = service;
        }

        [HttpGet("bycard/{cardId}")]
        public IList<TransactionsDto> GetAllCardTransactions(int cardId)
        {
            var userName = User.Identity.Name;
            if (userName == null) return null;
            return this._transactionsService.GetAllCardTransactions(userName, cardId);
        }

        [HttpGet("bygoal/{goalId}")]
        public IList<TransactionsDto> GetAllGoalTransactions(int goalId)
        {
            var userName = User.Identity.Name;
            if (userName == null) return null;
            return this._transactionsService.GetAllGoalTransactions(userName, goalId);
        }

        [HttpGet("retry/{transactionId}")]
        public async Task<bool> RetryTransactionById(int transactionId)
        {
            var userName = User.Identity.Name;
            if (userName == null) return false;

            return await this._transactionsService.TryToRetryTransaction(userName, transactionId);
        }
    }
}
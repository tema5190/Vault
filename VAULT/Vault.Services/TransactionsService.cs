using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vault.DATA;
using Vault.DATA.DTOs.Transaction;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class TransactionsService
    {
        private readonly VaultContext _db;
        private readonly BankOperationService _bankOperationService;

        public TransactionsService(VaultContext context, BankOperationService bankOperationService)
        {
            this._db = context;
            this._bankOperationService = bankOperationService;
        }

        public IList<TransactionsDto> GetAllCardTransactions(string userName, int cardId)
        {
            return GetUserWithFullInfo(userName).ClientInfo.Transactions.Where(t => t.CardId == cardId).Select(t => new TransactionsDto(t)).ToList();
        }

        public IList<TransactionsDto> GetAllGoalTransactions(string userName, int goalId)
        {
            return GetUserWithFullInfo(userName).ClientInfo.Transactions.Where(t => t.GoalId == goalId).Select(t => new TransactionsDto(t)).ToList();
        }

        public async Task<bool> TryToRetryTransaction(string userName, int transactionId)
        {
            if (!CheckIsTransactionBelongsUser(userName, transactionId)) return false;
            return await this._bankOperationService.RetryTransaction(transactionId);
        }

        private VaultUser GetUserWithFullInfo(string userName)
        {
            return _db.Users.Include(u => u.ClientInfo.Cards).Include(u => u.ClientInfo.Transactions).Single(u => u.UserName == userName);
        }

        private bool CheckIsTransactionBelongsUser(string userName, int transactionId)
        {
            return GetUserWithFullInfo(userName).ClientInfo.Transactions.Any(t => t.Id == transactionId);
        }
    }
}

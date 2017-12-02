using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vault.DATA;
using Vault.DATA.Models;

namespace Vault.Services
{
    public class TransactionsService
    {
        private readonly VaultContext _db;

        public TransactionsService(VaultContext context)
        {
            this._db = context;
        }

        public IList<RefillTransaction> GetAllCardTransactions(string userName, int cardId)
        {
            return GetUserWithFullInfo(userName).ClientInfo.Transactions.Where(t => t.CardId == cardId).ToList();
        }

        public IList<RefillTransaction> GetAllGoalTransactions(string userName, int goalId)
        {
            return GetUserWithFullInfo(userName).ClientInfo.Transactions.Where(t => t.GoalId == goalId).ToList();
        }

        //public bool TryToRetryTransaction(string userName, int transactionId)
        //{
        //    return GetUserWithFullInfo(userName).ClientInfo.Transactions
        //}

        private VaultUser GetUserWithFullInfo(string userName)
        {
            return _db.Users.Include(u => u.ClientInfo.Cards).Include(u => u.ClientInfo.Transactions).Single(u => u.UserName == userName);
        }
    }
}

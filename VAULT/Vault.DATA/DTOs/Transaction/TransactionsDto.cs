using Vault.DATA.Models;

namespace Vault.DATA.DTOs.Transaction
{
    public class TransactionsDto
    {
        public int TransactionId { get; set; }

        public bool IsPausedOrError { get; set; }

        public bool IsTransactionRetried { get; set; }

        public decimal Money { get; set; }

        public string Status { get; set; }

        // heh
        public int CardId { get; set; }
        public int GoalId { get; set; }

        public TransactionsDto(RefillTransaction transaction)
        {
            this.TransactionId = transaction.Id;
            this.IsPausedOrError = transaction.IsPausedOrError;
            this.IsTransactionRetried = transaction.TransactionIsRetried;
            this.Money = transaction.Money;
            this.Status = transaction.Status;

            this.CardId = transaction.CardId;
            this.GoalId = transaction.GoalId;
        }
    }
}

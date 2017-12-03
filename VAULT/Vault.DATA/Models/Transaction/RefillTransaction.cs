using System;
using System.ComponentModel.DataAnnotations;

namespace Vault.DATA.Models
{
    public class RefillTransaction
    {
        [Key]
        public int Id { get; set; }

        public bool IsPausedOrError { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public decimal Money { get; set; }

        public bool TransactionIsRetried { get; set; }
        public string Status { get; set; }

        // Navigation property
        public int CardId { get; set; }
        public UserCard CreditCard { get; set; }

        public int GoalId { get; set; }
        public Goal Goal { get; set; }
    }
}

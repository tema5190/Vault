using System.ComponentModel.DataAnnotations;

namespace Vault.DATA.Models
{
    public class RefillTransaction
    {
        [Key]
        public int Id { get; set; }

        public bool IsPausedError { get; set; }

        // Navigation property
        public int? CardId { get; set; }
        public CreditCard CreditCard { get; set; }

        public int? GoalId { get; set; }
        public Goal Goal { get; set; }
    }
}

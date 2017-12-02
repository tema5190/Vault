using System.ComponentModel.DataAnnotations;

namespace Vault.DATA.Models
{
    public class RefillTransaction
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        // Navigation property
        public int CardId { get; set; }
        public CreditCard CreditCard { get; set; }
        public bool IsPausedError { get; set; }

        public int? TargetId { get; set; }
        public Goal Target { get; set; }
    }
}

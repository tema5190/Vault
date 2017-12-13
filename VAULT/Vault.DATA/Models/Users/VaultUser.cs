using System.ComponentModel.DataAnnotations;
using Vault.DATA.Enums;

namespace Vault.DATA.Models
{
    public class VaultUser
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsRegistrationFinished { get; set; }

        public UserRole Role { get; set; }
        public AuthModelType? AuthModelType { get; set; }

        public ClientInfo ClientInfo { get; set; }
    }
}

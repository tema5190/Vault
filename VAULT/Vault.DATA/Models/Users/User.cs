using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vault.DATA.Enums;

namespace Vault.DATA.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public bool IsRegistrationFinished { get; set; }

        public UserRoles Role { get; set; }

        public ClientInfo ClientInfo { get; set; }
    }
}

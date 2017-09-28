using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vault.DATA.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Role { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        
        public IList<CreditCard> Cards { get; set; }
        public IList<Target> Targets { get; set;}
        public IList<RefillTransaction> Transactions { get; set; }
    }
}

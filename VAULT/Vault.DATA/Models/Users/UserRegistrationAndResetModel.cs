using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vault.DATA.Models.User
{
    public class UserRegistrationAndResetModel
    {
        [Key]
        public int Id { get; set; }

        public string InitialPassword { get; set; }
    }
}

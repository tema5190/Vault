using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vault.DATA.Enums;

namespace Vault.DATA.Models
{
    public class ClientInfo
    {
        [Key]
        public int Id { get; set; }

        public IList<CreditCard> Cards { get; set; }
        public IList<Goal> Targets { get; set; }
        public IList<RefillTransaction> Transactions { get; set; }
    }
}

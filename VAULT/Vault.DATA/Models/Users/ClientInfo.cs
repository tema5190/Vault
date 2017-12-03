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

        public string Email { get; set; }
        public string Phone { get; set; }

        public IList<UserCard> Cards { get; set; }
        public IList<Goal> Goals { get; set; }
        public IList<RefillTransaction> Transactions { get; set; }

        public ClientInfo()
        {
            Cards = new List<UserCard>();
            Goals = new List<Goal>();
            Transactions = new List<RefillTransaction>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Vault.DATA.DTOs.Email
{
    public class EmailSMTPConfiguration
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public bool UseSSL { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string Sign { get; set; }
    }
}

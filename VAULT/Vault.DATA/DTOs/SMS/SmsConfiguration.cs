using System;
using System.Collections.Generic;
using System.Text;

namespace Vault.DATA.DTOs
{
    public class SmsConfiguration
    {
        public string ACCOUNT_SID { get; set; }
        public string AUTH_TOKEN { get; set; }
        public string FROM_NUMBER { get; set; }
    }
}

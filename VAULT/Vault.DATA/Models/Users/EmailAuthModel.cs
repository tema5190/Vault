using System;
using System.Collections.Generic;
using System.Text;
using Vault.DATA.DTOs.Auth;
using Vault.DATA.Enums;

namespace Vault.DATA.Models.Users
{
    public class EmailAuthModel
    {
        public int Id { get; set; }

        public EmailAuthReason Reason { get; set; }

        public string UserName { get; set; }

        public string NewPassword { get; set; }

        public DateTime CodeSendedDateTime { get; set; }

        public string TargetEmail { get; set; }

        public string EmailKey { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Vault.DATA.DTOs.Auth;
using Vault.DATA.Enums;

namespace Vault.DATA.Models.Users
{
    public class AuthVerificationModel
    {
        public int Id { get; set; }

        public AuthReason Reason { get; set; }

        public string UserName { get; set; }

        public string NewPassword { get; set; }

        public DateTime CodeSendedDateTime { get; set; }

        public AuthModelType AuthModelType { get; set; }

        public string TargetEmail { get; set; }

        public string TargetPhone { get; set; }

        public string TwoWayAuthKey { get; set; }
    }
}

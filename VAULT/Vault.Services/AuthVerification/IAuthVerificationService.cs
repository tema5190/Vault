using System;
using System.Collections.Generic;
using System.Text;

namespace Vault.Services.AuthVerification
{
    public interface IAuthVerificationService
    {
        void SendMessage(string target, string content, string subject = null);
        void SendAuthTypeVerification(string target, string authCode);
        void SendLoginVerification(string target, string authCode);
    }
}

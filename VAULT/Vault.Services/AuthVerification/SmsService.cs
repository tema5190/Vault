using Microsoft.Extensions.Options;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Vault.DATA;
using Vault.DATA.DTOs;
using Vault.Services.AuthVerification;

namespace Vault.Services
{
    public class SmsService : IAuthVerificationService
    {
        private readonly SmsConfiguration _smsConfiguration;
        private readonly VaultContext _db;

        public SmsService(IOptions<SmsConfiguration> options, VaultContext context)
        {
            this._db = context;
            this._smsConfiguration = options.Value;
        }

        public void SendAuthTypeVerification(string target, string authCode)
        {
            throw new NotImplementedException();
        }

        public void SendLoginVerification(string to, string code)
        {
            string validPhone = to;
            if(!to.StartsWith('+'))
                validPhone = $"+{to}";

            var content = $"Your verification code for VaultBank login is {code}";
            this.SendMessage(validPhone, content, null);
        }

        public void SendMessage(string to, string message, string subject)
        {
            TwilioClient.Init(this._smsConfiguration.ACCOUNT_SID, this._smsConfiguration.AUTH_TOKEN);

            MessageResource.Create(
                from: new PhoneNumber(this._smsConfiguration.FROM_NUMBER),
                to: new PhoneNumber(to),
                body: message
            );
        }
    }
}

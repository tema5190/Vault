using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Vault.DATA;
using Vault.DATA.DTOs.Email;

namespace Vault.Services
{
    public class EmailService
    {
        private readonly VaultContext _db;
        private readonly IOptions<EmailSMTPConfiguration> _smptpOptions;

        public EmailService(VaultContext vaultContext, IConfiguration configuration, IOptions<EmailSMTPConfiguration> options)
        {
            this._db = vaultContext;
            this._smptpOptions = options;

        }

        public void SendEmailVerification(string email)
        {
            
        }

        public void SendEmail(string email, string content, string subject)
        {
            var mail = new MailMessage(this._smptpOptions.Value.Email, email);
            mail.Subject = subject ?? "";
            mail.Body = this.GetEmailWithSign(content);

            var smtpClient = this.GetSMTPClient();
            smtpClient.Send(mail);
        }

        private string GetEmailWithSign(string message)
        {
            var sb = new StringBuilder(message);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.Append(this._smptpOptions.Value.Sign);
            return sb.ToString();
        }

        private SmtpClient GetSMTPClient() {

            SmtpClient client = new SmtpClient();
            client.Port = this._smptpOptions.Value.Port;
            client.Host = this._smptpOptions.Value.Host;
            client.UseDefaultCredentials = this._smptpOptions.Value.UseDefaultCredentials;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            return client;
        }
    }
}

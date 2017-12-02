using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Vault.DATA;
using Vault.DATA.DTOs.Email;

namespace Vault.Services
{
    public class EmailService
    {
        private readonly VaultContext _db;
        private readonly EmailSMTPConfiguration _smptpOptions;

        public EmailService(VaultContext vaultContext, IOptions<EmailSMTPConfiguration> options)
        {
            _db = vaultContext;
            _smptpOptions = options.Value;
        }

        public void SendEmailVerification(string email, string key, DateTime sendDate)
        {
            FileStream fileStream = new FileStream($@"{Directory.GetCurrentDirectory()}\wwwroot\FirstStepRegistration.html", FileMode.Open);
            var sb = new StringBuilder();
            using (var sr = new StreamReader(fileStream))
            {
                while (!sr.EndOfStream)
                {
                    sb.Append(sr.ReadLine());
                }
            }
            var emailContent = sb.ToString();
            emailContent = emailContent.Replace("*CODE*", key);
            emailContent = emailContent.Replace("*Month Year*", sendDate.ToLongDateString());
            emailContent = AddCongirmRegistrationContent(emailContent); 

            SendEmail(email, emailContent, "Email verification");
        }

        private string AddCongirmRegistrationContent(string message)
        {
            return message.Replace("*REGISTRATIONTEXT*", "This email is to confirm your recent registration.");
        }

        private string AddConfirmLogInContent(string message)
        {
            return message.Replace("*REGISTRATIONTEXT*", "This email is to confirm your recent login");
        }

        public void SendEmail(string email, string content, string subject)
        {
            var mail = new MailMessage(_smptpOptions.Email, email);
            mail.Subject = subject ?? "";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            mail.Body = content;
            var smtpClient = GetSMTPClient();
            smtpClient.Send(mail);
        }

        private void AddSignToEmailContent(string message)
        {
            var sb = new StringBuilder(message);
            sb.AppendLine();
            sb.Append(this._smptpOptions.Sign);
            message = sb.ToString();
        }

        private SmtpClient GetSMTPClient() {

            SmtpClient client = new SmtpClient
            {
                Port = _smptpOptions.Port,
                Host = _smptpOptions.Host,
                UseDefaultCredentials = _smptpOptions.UseDefaultCredentials,
                Credentials = new NetworkCredential(_smptpOptions.Email, _smptpOptions.Password),
                EnableSsl = _smptpOptions.UseSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            return client;
        }
    }
}

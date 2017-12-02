using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Vault.DATA;
using Vault.DATA.DTOs.Auth;
using Vault.DATA.DTOs.Registration;
using Vault.DATA.Models;
using Vault.DATA.Models.Users;

namespace Vault.Services
{
    public class AuthService
    {
        private readonly VaultContext _context;
        private readonly UserService _userService;
        private readonly EmailService _emailService;

        public AuthService(VaultContext context, UserService userService, EmailService emailService)
        {
            _context = context;
            _userService = userService;
            _emailService = emailService;
        }

        public VaultUser Login(string login, string password)
        {
            return GetUser(login, password);
        }

        public FirstStepResultDto FirstStepRegister(FirstStepRegisterData firstStep)
        {
            var result = new FirstStepResultDto();

            var user = this._context.Users.FirstOrDefault(u => u.UserName == firstStep.UserName);

            if(user == null)
            {
                result.UserNameNotFound = true;
                return result;
            }

            if (CheckEmailExistanse(firstStep.Email))
            {
                result.IsEmailExist = true;
                return result;
            }

            var registration = new Registration()
            {
                EmailKey = GetRandomEmailKey(8),
                TargetEmail = firstStep.Email,
                CodeSendedDateTime = DateTime.Now,
                UserName = firstStep.UserName,
                NewPassword = firstStep.Password,
            };

            var lastRegistration = _context.Registrations.FirstOrDefault(r => r.UserName == firstStep.UserName);

            if(lastRegistration == null)
            {
                _context.Registrations.Add(registration);
            }
            else
            {
                lastRegistration.TargetEmail = registration.TargetEmail;
                lastRegistration.EmailKey = registration.EmailKey;
                lastRegistration.CodeSendedDateTime = registration.CodeSendedDateTime;
                _context.Attach(lastRegistration).State = EntityState.Modified;
            }
            _context.SaveChanges();

            _emailService.SendEmailVerification(registration.TargetEmail, registration.EmailKey, registration.CodeSendedDateTime);

            return result;
        }

        public bool SecondStepRegister(SecondStepRegisterData data)
        {
            var registration = _context.Registrations.FirstOrDefault(r => r.UserName == data.UserName);

            if (registration == null || registration.EmailKey != data.EmailKey)
                return false;

            if(registration.EmailKey == data.EmailKey)
            {
                var user = _context.Users.Include(u => u.ClientInfo).FirstOrDefault(u => u.UserName == data.UserName);

                if (user == null) return false;

                user.ClientInfo = new ClientInfo();
                user.ClientInfo.Email = registration.TargetEmail;
                user.IsRegistrationFinished = true;
                user.Password = registration.NewPassword;

                _context.SaveChanges();

                return true;
            }

            return false;
        } 

        public static string GetRandomEmailKey(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool CheckEmailExistanse(string email)
        {
            var isExist = _context.Users.Include(u => u.ClientInfo).FirstOrDefault(ci => ci.ClientInfo.Email == email) != null;
            return isExist;
        }

        private VaultUser GetUser(string login, string password)
            {
                var user = _context.Users.SingleOrDefault(u => u.UserName == login);

                if(user != null && password == user.Password)
                {
                    return user;
                }
                return null;
            }
        }
}

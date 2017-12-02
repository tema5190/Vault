using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Vault.DATA;
using Vault.DATA.DTOs.Auth;
using Vault.DATA.DTOs.Registration;
using Vault.DATA.Enums;
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

        public VaultUser GetUserByEmailKey(string emailKey)
        {
            return _context.EmailAuthModels.Include(r => r.User).Single(r => r.EmailKey == emailKey).User;
        }

        public void RequestEmailKey(VaultUser user)
        {
            var loginAuthModel = new EmailAuthModel()
            {
                Reason = EmailAuthReason.IsLogin,
                EmailKey = GetRandomEmailKey(8),
                TargetEmail = user.ClientInfo.Email,
                CodeSendedDateTime = DateTime.Now,
                UserName = user.UserName,
                NewPassword = user.Password,
                User = user,
            };

            this.AddOrUpdateEmailAuthModel(loginAuthModel);
            this._emailService.SendLoginVerification(loginAuthModel.TargetEmail, loginAuthModel.EmailKey);
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

            var authModel = new EmailAuthModel()
            {
                EmailKey = GetRandomEmailKey(8),
                TargetEmail = firstStep.Email,
                CodeSendedDateTime = DateTime.Now,
                UserName = firstStep.UserName,
                NewPassword = firstStep.Password,
            };
            AddOrUpdateEmailAuthModel(authModel);

            _emailService.SendEmailVerification(authModel.TargetEmail, authModel.EmailKey);

            return result;
        }

        public bool SecondStepRegister(SecondStepRegisterData data)
        {
            var registration = _context.EmailAuthModels.FirstOrDefault(r => r.UserName == data.UserName);

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

        private void AddOrUpdateEmailAuthModel(EmailAuthModel model)
        {
            var lastAuthModel = _context.EmailAuthModels.FirstOrDefault(r => r.UserName == model.UserName);

            if (lastAuthModel == null)
            {
                _context.EmailAuthModels.Add(model);
                _context.Attach(lastAuthModel).State = EntityState.Added;
            }
            else
            {
                lastAuthModel.TargetEmail = model.TargetEmail;
                lastAuthModel.EmailKey = model.EmailKey;
                lastAuthModel.CodeSendedDateTime = model.CodeSendedDateTime;
                lastAuthModel.NewPassword = lastAuthModel.Reason == EmailAuthReason.IsLogin ? null : lastAuthModel.NewPassword;
                lastAuthModel.Reason = model.Reason;
                _context.Attach(lastAuthModel).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }

        private static string GetRandomEmailKey(int length)
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

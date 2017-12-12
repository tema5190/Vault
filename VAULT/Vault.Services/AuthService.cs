using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        private readonly SmsService _smsService;

        public AuthService(VaultContext context, UserService userService, EmailService emailService, SmsService smsService)
        {
            _context = context;
            _userService = userService;
            _emailService = emailService;
            _smsService = smsService;
        }

        public VaultUser Login(LoginDto loginDto)
        {
            return GetUser(loginDto.Login, loginDto.Password);
        }

        public VaultUser GetUserByAuthKey(string emailKey)
        {
            var user = _context.AuthVerificationModel.SingleOrDefault(r => r.TwoWayAuthKey == emailKey);
            if (user == null) return null;

            var userName = user.UserName;
            return _context.Users.Single(u => u.UserName == userName);
        }

        public void RequestAuthKey(VaultUser user)
        {
            if (user.AuthModelType == AuthModelType.Email) { 
                RequestEmailKey(user);
                return;
            }
            if (user.AuthModelType == AuthModelType.Phone) { 
                RequestSmsKey(user);
                return;
            }
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

            if (CheckAuthModelTargetExistanse(firstStep.TwoWayAuthTarget, firstStep.AuthModelType))
            {
                result.IsEmailOrPhoneExists = true;
                return result;
            }

            var authModel = new AuthVerificationModel() // TOOD add ctor... ehhh
            {
                Reason = AuthReason.TwoWayAuthTargetVerification,
                AuthModelType = firstStep.AuthModelType,

                TwoWayAuthKey = GetRandomTwoWayAuthKey(8),

                TargetPhone = firstStep.AuthModelType == AuthModelType.Phone ? firstStep.TwoWayAuthTarget : null,
                TargetEmail = firstStep.AuthModelType == AuthModelType.Email ? firstStep.TwoWayAuthTarget : null,

                CodeSendedDateTime = DateTime.Now,
                UserName = firstStep.UserName,
                NewPassword = firstStep.Password,
            };

            AddOrUpdateEmailAuthModel(authModel);
            _emailService.SendAuthTypeVerification(authModel.TargetEmail, authModel.TwoWayAuthKey);

            return result;
        }

        public bool SecondStepRegister(SecondStepRegisterData data)
        {
            var registration = _context.AuthVerificationModel.FirstOrDefault(r => r.UserName == data.UserName);
            if (registration == null) return false;

            if(registration.TwoWayAuthKey == data.TwoWayAuthKey)
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

        private void RequestEmailKey(VaultUser user)
        {
            var loginAuthModel = new AuthVerificationModel()
            {
                Reason = AuthReason.Login,
                AuthModelType = AuthModelType.Email,

                TwoWayAuthKey = GetRandomTwoWayAuthKey(8),

                TargetEmail = user.ClientInfo.Email,

                CodeSendedDateTime = DateTime.Now,
                UserName = user.UserName,
                NewPassword = user.Password,
            };

            this.AddOrUpdateEmailAuthModel(loginAuthModel);
            this._emailService.SendLoginVerification(loginAuthModel.TargetEmail, loginAuthModel.TwoWayAuthKey);
        }

        private void RequestSmsKey(VaultUser user)
        {
            var loginAuthModel = new AuthVerificationModel()
            {
                Reason = AuthReason.Login,
                AuthModelType = AuthModelType.Phone,

                TwoWayAuthKey = GetRandomTwoWayAuthKey(8),

                TargetPhone = user.ClientInfo.Phone,

                CodeSendedDateTime = DateTime.Now,
                UserName = user.UserName,
                NewPassword = user.Password,
            };

            this.AddOrUpdateEmailAuthModel(loginAuthModel);
            this._smsService.SendLoginVerification(loginAuthModel.TargetPhone, loginAuthModel.TwoWayAuthKey);
        }

        private void AddOrUpdateEmailAuthModel(AuthVerificationModel model)
        {
            var lastAuthModel = _context.AuthVerificationModel.FirstOrDefault(r => r.UserName == model.UserName);

            if (lastAuthModel == null)
            {
                _context.AuthVerificationModel.Add(model);
            }
            else
            {
                lastAuthModel.TargetEmail = model.TargetEmail;
                lastAuthModel.TwoWayAuthKey = model.TwoWayAuthKey;
                lastAuthModel.CodeSendedDateTime = model.CodeSendedDateTime;
                lastAuthModel.NewPassword = lastAuthModel.Reason == AuthReason.Login ? null : lastAuthModel.NewPassword;
                lastAuthModel.Reason = model.Reason;
                _context.Attach(lastAuthModel).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }

        private static string GetRandomTwoWayAuthKey(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool CheckAuthModelTargetExistanse(string target, AuthModelType type)
        {
            if (type == AuthModelType.Email)
                return CheckEmailExistanse(target);

            return CheckPhoneExistanse(target);
        }

        private bool CheckEmailExistanse(string email)
        {
            return _context.Users.Include(u => u.ClientInfo).FirstOrDefault(ci => ci.ClientInfo.Email == email) != null;
        }

        private bool CheckPhoneExistanse(string phone)
        {
            return _context.Users.Include(u => u.ClientInfo).FirstOrDefault(ci => ci.ClientInfo.Phone == phone) != null;
        }

        private VaultUser GetUser(string login, string password)
        {
            var user = _context.Users.Include(u => u.ClientInfo).SingleOrDefault(u => u.UserName == login);

            if(user != null && CheckUserPasswordValidity(user.Password, password))
            {
                return user;
            }
            return null;
        }

        private bool CheckUserPasswordValidity(string existUserPassword, string inputPassword)
        {
            return String.Compare(existUserPassword, inputPassword) == 0;
            //return String.Compare(existUserPassword, CalculateMD5Hash(inputPassword)) == 0;
        }

        private string CalculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

    }
}

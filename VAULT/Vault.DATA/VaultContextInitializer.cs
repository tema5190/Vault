using BankModel;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vault.DATA.Enums;
using Vault.DATA.Models;

namespace Vault.DATA
{
    public class VaultContextInitializer
    {
        private readonly VaultContext _db;

        public VaultContextInitializer(VaultContext context)
        {
            if((context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                this._db = context;
        }

        public void Seed()
        {          
            // 1
            var u1 = new VaultUser()
            {
                UserName = "test",
                Role = UserRole.Client,
                Password = "test",
                IsRegistrationFinished = false,
            };

            // 2
            var c1 = new UserCard()
            {
                CardNumber = "1111111111111111",
                CardType = CardType.Visa,
                CustomCardName = "ООООО, МОЯ ОБОРОНА1",
                CVV = "111",
                ExpirationDate = new DateTime(2019, 5, 20),
                IsPaused = false,
                OwnerFullName = "Mr Artem Derid",
                
            };
            var c2 = new UserCard()
            {
                CardNumber = "2111111111111112",
                CardType = CardType.MasterCard,
                CustomCardName = "ООООО, МОЯ ОБОРОНА2",
                CVV = "112",
                ExpirationDate = new DateTime(2020, 5, 20),
                IsPaused = false,
                OwnerFullName = "Mr Artem Derid2",
            };
            var c3 = new UserCard()
            {
                CardNumber = "91121111111113",
                CardType = CardType.BelCard,
                CustomCardName = "ООООО, МОЯ ОБОРОНА3",
                CVV = "113",
                ExpirationDate = new DateTime(2021, 10, 20),
                IsPaused = true,
                OwnerFullName = "Mr Artem Derid3",
            };

            var g1 = new Goal()
            {
                ChargeDay = new DateTime(2014, 09, 1).Day,
                CurrentMoney = 1000,
                Description = "На взятку военкому",
                IsPaused = false,
                MoneyPerMonth = 100,
                MoneyTarget = 2000,
                TargetEnd = new DateTime(2017, 12, 08),
                TargetStart = new DateTime(2014, 09, 1),
                TargetType = TargetType.Short,
                Title = "Ну вы поняли",

            };

            var g2 = new Goal()
            {
                ChargeDay = new DateTime(2017,12,08).Day,
                CurrentMoney = 0,
                Description = "На машину",
                IsPaused = true,
                MoneyPerMonth = 10,
                MoneyTarget = 15000,
                TargetEnd = new DateTime(2018, 12, 01),
                TargetStart = new DateTime(2017, 12, 08),
                TargetType = TargetType.Short,
                Title = "Би-би",
            };

            var ci0 = new ClientInfo()
            {
                Email = "tema5190@gmail.com",
                Phone = null,
                Goals = new List<Goal>(),
                Cards = new List<UserCard>(),
            };

            // admin
            var u0 = new VaultUser()
            {
                UserName = "admin",
                Password = "admin",
                Role = UserRole.Admin,
                IsRegistrationFinished = true,
                ClientInfo = ci0,
            };

            var ci2 = new ClientInfo()
            {
                Email = "artemderid@gmail.com",
                Goals = new List<Goal>() { g1, g2 },
                Cards = new List<UserCard>() { c1, c2, c3 }
            };

            var u2 = new VaultUser()
            {
                UserName = "test1",
                Role = UserRole.Client,
                Password = "test1",
                IsRegistrationFinished = true,
                ClientInfo = ci2,
                AuthModelType = AuthModelType.Email,
            };

            c1.Owner = u2;
            c2.Owner = u2;
            c3.Owner = u2;

            g1.CreditCard = c2;
            g2.CreditCard = c3;

            var bc1 = new BankCard()
            {
                Balance = 1000000,
                CardNumber = c1.CardNumber,
                CardType = c1.CardType,
                CVV = c1.CVV,
                ExpirationDate = c1.ExpirationDate,
                IsBlocked = false,
                OwnerFullName = c1.OwnerFullName,
            };
            var bc2 = new BankCard()
            {
                Balance = -50,
                CardNumber = c3.CardNumber,
                CardType = c3.CardType,
                CVV = c3.CVV,
                ExpirationDate = c3.ExpirationDate,
                IsBlocked = true,
                OwnerFullName = c3.OwnerFullName,
            };

            var t1 = new RefillTransaction()
            {
                CreditCard = c1,
                Goal = g1,
                IsPausedOrError = false,
                Money = 100,
                Status = "Heh",
                TransactionDateTime = new DateTime(2017, 11, 08),
                TransactionIsRetried = false,
            };
            var t2 = new RefillTransaction()
            {
                CreditCard = c1,
                Goal = g1,
                IsPausedOrError = false,
                Money = 100,
                Status = "Heh",
                TransactionDateTime = new DateTime(2017, 10, 08),
                TransactionIsRetried = false,
            };
            var t3 = new RefillTransaction()
            {
                CreditCard = c1,
                Goal = g1,
                IsPausedOrError = true,
                Money = 0,
                Status = "Some error",
                TransactionDateTime = new DateTime(2017, 9, 08),
                TransactionIsRetried = true,
            };
            var t4 = new RefillTransaction()
            {
                CreditCard = c1,
                Goal = g1,
                IsPausedOrError = false,
                Money = 0,
                Status = "Completed",
                TransactionDateTime = new DateTime(2017, 9, 09),
                TransactionIsRetried = false,
            };
            var t5 = new RefillTransaction()
            {
                CreditCard = c1,
                Goal = g1,
                IsPausedOrError = false,
                Money = 100,
                Status = "Heh",
                TransactionDateTime = new DateTime(2017, 8, 08),
                TransactionIsRetried = false,
            };

            var t6 = new RefillTransaction()
            {
                CreditCard = c3,
                Goal = g2,
                IsPausedOrError = true,
                Money = 0,
                Status = "Card is blocked",
                TransactionDateTime = new DateTime(2017, 12, 07),
                TransactionIsRetried = false,
            };

            _db.Users.AddRange(new List<VaultUser>() { u1, u2, u0});
            _db.ClientInfos.AddRange(new List<ClientInfo>() { ci2, ci0 });
            _db.UserCards.AddRange(u2.ClientInfo.Cards);
            _db.Goals.AddRange(u2.ClientInfo.Goals);
            _db.Transactions.AddRange(new List<RefillTransaction>() { t1, t2, t3, t4, t5, t6});


            this._db.SaveChanges();
        }
    }
}

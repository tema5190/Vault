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
            if (IsUsersEmpty())
            {
                this.SimpleInitialWithTwoUsers();
            }
        }

        private bool IsUsersEmpty()
        {
            return _db.Users.FirstOrDefault() == null;
        }

        private void SimpleInitialWithTwoUsers()
        {
            _db.Users.AddRange(new List<VaultUser>(){
                new VaultUser()
                {
                    UserName = "test1",
                    IsRegistrationFinished = false,
                    Password = "test1",
                    Role = UserRoles.Client,
                },
                new VaultUser()
                {
                    UserName = "test2",
                    IsRegistrationFinished = true,
                    Password = "test2",
                    Role = UserRoles.Manager,
                 ClientInfo = new ClientInfo()
                 {
                     Cards = new List<CreditCard>()
                     {
                         new CreditCard()
                         {
                           CardType = CardType.Visa,
                           CustomCardName = "My salary credit card",
                           ExpirationDate = new DateTime(2019, 2, 5),
                           CardNumber = "8800555353511111",
                         }
                     },
                     Email = "test@test",
                     Goals = new List<Goal>()
                     {
                         new Goal()
                         {
                             Title = "На откос от армии",
                             TargetType = TargetType.Middle,
                             MoneyCurrent = 1800,
                             MoneyTarget = 4000,
                             TargetStart = new DateTime(2014,9,1),
                             TargetEnd = new DateTime(2017,12,30),
                             Description = "Ну вы поняли",
                         },
                     },
                 }
                }
            });

            this._db.SaveChanges();
        }
    }
}

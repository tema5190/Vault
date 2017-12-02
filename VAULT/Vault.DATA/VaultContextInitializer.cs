using System;
using System.Collections.Generic;
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
            this._db = context;
        }

        public void SimpleInitialWithTwoUsers()
        {
            _db.Users.AddRange(new List<VaultUser>(){
                new VaultUser()
                {
                    UserName = "test",
                    IsRegistrationFinished = false,
                    Password = "test",
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
                           CardType = CartType.Visa,
                           Name = "My salary credit card",
                           RefillDate = new DateTime(2019, 2, 5),
                           CardNumber = "8800555353511111",
                         }
                     },
                     Email = "tema5190@gmail.com",
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

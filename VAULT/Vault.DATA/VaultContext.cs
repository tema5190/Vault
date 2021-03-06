﻿using BankModel;
using Microsoft.EntityFrameworkCore;
using Vault.DATA.Models;
using Vault.DATA.Models.Users;

namespace Vault.DATA
{
    public class VaultContext : DbContext
    {
        public VaultContext(DbContextOptions<VaultContext> options) : base(options) { }

        public DbSet<VaultUser> Users { get; set; }
        public DbSet<ClientInfo> ClientInfos { get; set; }

        public DbSet<UserCard> UserCards { get; set; }
        public DbSet<RefillTransaction> Transactions { get; set; }
        public DbSet<Goal> Goals { get; set; }

        public DbSet<AuthVerificationModel> AuthVerificationModel { get; set; }

        // ONLY FOR ADMIN OR SERVICES => DO NOT USE THIS MODELS
        public DbSet<BankCard> BankCards { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VaultUser>()
                .HasOne(u => u.ClientInfo)
                .WithOne(info => info.User)
                .HasForeignKey<ClientInfo>(clientInfo => clientInfo.UserId);

            modelBuilder.Entity<UserCard>()
                .HasMany<Goal>(u => u.Goals)
                .WithOne(g => g.CreditCard)
                .HasForeignKey(g => g.CreditCardId);
        }
    }
}

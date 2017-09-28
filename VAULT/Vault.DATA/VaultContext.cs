using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vault.DATA.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Vault.DATA
{
    public class VaultContext : DbContext
    {
        private readonly string connectionString;

        public VaultContext(): base()
        {

            this.connectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build().GetConnectionString("VaultDataBase");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CreditCard> Cards { get; set; }
        public DbSet<RefillTransaction> Transactions { get; set; }
        public DbSet<Target> Targets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(this.connectionString);
        }

    }
}

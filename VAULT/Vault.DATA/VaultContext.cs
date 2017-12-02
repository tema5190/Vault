using Microsoft.EntityFrameworkCore;
using Vault.DATA.Models;
using Vault.DATA.Models.Users;

namespace Vault.DATA
{
    public class VaultContext : DbContext
    {
        public VaultContext(DbContextOptions<VaultContext> options) : base(options) {
        }

        public DbSet<VaultUser> Users { get; set; }
        public DbSet<CreditCard> Cards { get; set; }
        public DbSet<RefillTransaction> Transactions { get; set; }
        public DbSet<Goal> Targets { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}

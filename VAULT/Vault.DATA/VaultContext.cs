using Microsoft.EntityFrameworkCore;
using Vault.DATA.Models;

namespace Vault.DATA
{
    public class VaultContext : DbContext
    {
        public VaultContext(DbContextOptions<VaultContext> options) : base(options) { }

        public DbSet<VaultUser> Users { get; set; }
        public DbSet<CreditCard> Cards { get; set; }
        public DbSet<RefillTransaction> Transactions { get; set; }
        public DbSet<Goal> Targets { get; set; }
    }
}

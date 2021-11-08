using Microsoft.EntityFrameworkCore;
using ZipPay.API.Interfaces;
using ZipPay.API.Model;

namespace ZipPay.API.Repository
{
    public class ZipPayDataContext : DbContext, IMySqlServerDatabaseContext
    {
        public ZipPayDataContext (DbContextOptions<ZipPayDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapUser(modelBuilder);
            MapAccount(modelBuilder);
        }

        public override int SaveChanges() => base.SaveChanges();

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        private static void MapUser(ModelBuilder modelBuilder)
        {
            var spotRateEntity = modelBuilder.Entity<User>();
            spotRateEntity.ToTable("user");
            spotRateEntity.HasKey(x => x.Id);
            spotRateEntity.Property(x => x.Name);
            spotRateEntity.Property(x => x.EmailAddress);
            spotRateEntity.Property(x => x.MonthlySalary).HasColumnType("double(8,2)");
            spotRateEntity.Property(x => x.MonthlyExpenses).HasColumnType("double(9,0)");
        }

        private static void MapAccount(ModelBuilder modelBuilder)
        {
            var spotRateEntity = modelBuilder.Entity<Account>();
            spotRateEntity.ToTable("account");
            spotRateEntity.HasKey(x => x.Id);
            spotRateEntity.Property(x => x.UserId);
            spotRateEntity.Property(x => x.EmailAddress);
            spotRateEntity.Property(x => x.AccountNumber);
            spotRateEntity.Property(x => x.AccountName);
            spotRateEntity.Property(x => x.CreditBalance).HasColumnType("double(6,2)");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ZipPay.API.Model;

namespace ZipPay.API.Interfaces
{
    public interface IMySqlServerDatabaseContext
    {
        int SaveChanges();
        DbSet<User> Users { get; set; }
        DbSet<Account> Accounts { get; set; }
    }
}
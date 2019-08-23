using Microsoft.EntityFrameworkCore;
using ZipPay.Users.Entities;

namespace ZipPay.Users.DataService
{
    public class DefaultDBContext : DbContext
    {
        public DefaultDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Account> Account { get; set; }
    }
}

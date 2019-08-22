using Microsoft.EntityFrameworkCore;
using ZipPay.Users.Entities;

namespace ZipPay.Users.Domain
{
    public class DefaultDBContext : DbContext
    {
        public DefaultDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}

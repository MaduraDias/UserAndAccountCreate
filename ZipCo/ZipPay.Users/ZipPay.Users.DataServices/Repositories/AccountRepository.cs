using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZipPay.Users.DataService;
using ZipPay.Users.Entities;

namespace ZipPay.Users.DataServices.Repositories
{
    public interface IAccountRepository
    {
        Task CreateAsync(User user);
        Task<List<Account>> GetAllAsync();
        Task<bool> IsAccountExists(Guid userId);
    }
    public class AccountRepository:IAccountRepository
    {
        private readonly DefaultDBContext defaultDbContext;

        public AccountRepository(DefaultDBContext defaultDbContext)
        {
            this.defaultDbContext = defaultDbContext 
                ?? throw new ArgumentNullException(nameof(defaultDbContext));
        }

        public Task CreateAsync(User user)
        {
            defaultDbContext.Account.Add(new Account()
            {
                UserId = user.Id
            }
          );

          return defaultDbContext.SaveChangesAsync();
        }

        public Task<List<Account>> GetAllAsync()
        {
            return defaultDbContext.Account.ToListAsync();
        }

        public async Task<bool> IsAccountExists(Guid userId)
        {
            var count = await defaultDbContext.Account.CountAsync
                                    (dbAccount => dbAccount.UserId == userId).ConfigureAwait(false);

            return count > 0;
        }
    }
}

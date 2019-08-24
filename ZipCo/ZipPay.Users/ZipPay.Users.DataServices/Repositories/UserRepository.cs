using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.Users.DataService;
using ZipPay.Users.Entities;

namespace ZipPay.Users.DataServices.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsUserIdExistsAsync(Guid id);
    }

    public class UserRepository:IUserRepository
    {

        private readonly DefaultDBContext defaultDbContext;

        public UserRepository(DefaultDBContext defaultDbContext)
        {
            this.defaultDbContext = defaultDbContext
                ?? throw new ArgumentNullException(nameof(defaultDbContext));
        }

        public Task CreateAsync(User user)
        {
            defaultDbContext.User.Add(user);
            return defaultDbContext.SaveChangesAsync();
        }

        public Task<List<User>> GetAllAsync()
        {
            return defaultDbContext.User
                        .ToListAsync();
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            return defaultDbContext.User
                    .FirstOrDefaultAsync(dbUser => dbUser.Id == id);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var count = await defaultDbContext.User.CountAsync
                                     (dbUser => dbUser.Email == email).ConfigureAwait(false);

            return count > 0;
        }

        public async Task<bool> IsUserIdExistsAsync(Guid id)
        {
            var count = await  defaultDbContext.User.CountAsync
                                    (dbUser => dbUser.Id == id).ConfigureAwait(false);

            return count > 0;

        }
    }
}

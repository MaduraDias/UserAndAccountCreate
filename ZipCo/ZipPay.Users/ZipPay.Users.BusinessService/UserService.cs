using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.Users.Domain;
using ZipPay.Users.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZipPay.Users.BusinessService
{
    public interface IUserService
    {
        Task CreateAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly DefaultDBContext defaultDBContext;

        public UserService(DefaultDBContext defaultDBContext)
        {
            this.defaultDBContext = defaultDBContext 
                ?? throw new ArgumentNullException(nameof(defaultDBContext));
        }

        public async Task CreateAsync(User user)
        {
            var isIdExists = await defaultDBContext.User.CountAsync
                                    (dbUser => dbUser.Id == user.Id).ConfigureAwait(false) > 0;

            if (isIdExists)
            {
                throw new ValidationException("Id already exists");
            }

            var isEmailExists = await defaultDBContext.User.CountAsync
                                   (dbUser => dbUser.Email == user.Email).ConfigureAwait(false) > 0;

            if (isEmailExists)
            {
                throw new ValidationException("Email already exists");
            }

            defaultDBContext.User.Add(user);
            await defaultDBContext.SaveChangesAsync()
                                   .ConfigureAwait(false);
        }

        public Task<List<User>> GetAllAsync()
        {
            return defaultDBContext.User.ToListAsync();
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            return defaultDBContext.User
                    .FirstOrDefaultAsync(dbUser => dbUser.Id == id);
        }
    }
}

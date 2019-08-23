using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ZipPay.Users.Entities;
using ZipPay.Users.DataServices.Repositories;

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
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task CreateAsync(User user)
        {
            await ValidateBeforeCreate(user)
                    .ConfigureAwait(false);

            await userRepository
                    .CreateAsync(user)
                    .ConfigureAwait(false);
        }

        private async Task ValidateBeforeCreate(User user)
        {
            var isIdExists = await userRepository.IsUserIdExistsAsync(user.Id)
                                                 .ConfigureAwait(false);

            if (isIdExists)
            {
                throw new ValidationException("User Id already exists");
            }

            var isEmailExists = await userRepository.IsEmailExistsAsync(user.Email)
                                                    .ConfigureAwait(false);

            if (isEmailExists)
            {
                throw new ValidationException("Email already exists");
            }
        }

        public Task<List<User>> GetAllAsync()
        {
            return userRepository.GetAllAsync();
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            return userRepository.GetByIdAsync(id);
        }
    }
}

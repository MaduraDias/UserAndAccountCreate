using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.Users.Entities;
using ZipPay.Users.DataServices.Repositories;
using ZipPay.Users.BusinessService.Exceptions;

namespace ZipPay.Users.BusinessService
{
    public interface IAccountService
    {
        Task CreateAsync(User user);
        Task<List<Account>> GetAllAsync();
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IUserRepository userRepository;

        public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task CreateAsync(User user)
        {
            await ValidateBeforeCreate(user).ConfigureAwait(false);
            await accountRepository.CreateAsync(user).ConfigureAwait(false);
        }

        private async Task ValidateBeforeCreate(User user)
        {
            var isUserExists = await userRepository.IsUserIdExistsAsync(user.Id)
                                                 .ConfigureAwait(false);

            if (!isUserExists)
            {
                throw new BusinessValidationException("User not found");
            }

            var isAccountExists = await accountRepository.IsAccountExists(user.Id)
                                                       .ConfigureAwait(false);

            if (isAccountExists)
            {
                throw new BusinessValidationException("Account already exists for the user");
            }

            //Can be configured in DB
            var creditValue = 1000;

            if (user.MonthlySalary - user.MonthlyExpenses < creditValue)
            {
                throw new BusinessValidationException($"Monthly Salary - Monthly Expenses less than {creditValue}");
            }
        }

        public Task<List<Account>> GetAllAsync()
        {
            return accountRepository.GetAllAsync();
        }

    }
}

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
        Task CreateAsync(Guid userId);
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

        public async Task CreateAsync(Guid userId)
        {
            await ValidateBeforeCreate(userId).ConfigureAwait(false);
            await accountRepository.CreateAsync(userId).ConfigureAwait(false);
        }

        private async Task ValidateBeforeCreate(Guid userId)
        {
            var isUserExists = await userRepository.IsUserIdExistsAsync(userId)
                                                 .ConfigureAwait(false);

            if (!isUserExists)
            {
                throw new BusinessValidationException("User not found");
            }

            var isAccountExists = await accountRepository.IsAccountExists(userId)
                                                       .ConfigureAwait(false);

            if (isAccountExists)
            {
                throw new BusinessValidationException("Account already exists for the user");
            }

            var user = await userRepository.GetByIdAsync(userId);

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

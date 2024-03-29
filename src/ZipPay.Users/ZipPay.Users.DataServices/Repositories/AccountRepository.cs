﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.Users.DataService;
using ZipPay.Users.Entities;

namespace ZipPay.Users.DataServices.Repositories
{
    public interface IAccountRepository
    {
        Task CreateAsync(Guid userId);
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

        public Task CreateAsync(Guid userId)
        {
            defaultDbContext.Account.Add(new Account()
            {
                UserId = userId
            }
          );

          return defaultDbContext.SaveChangesAsync();
        }

        public Task<List<Account>> GetAllAsync()
        {
            return defaultDbContext
                .Account
                .Include(acc => acc.User)
                .ToListAsync();
        }

        public async Task<bool> IsAccountExists(Guid userId)
        {
            var count = await defaultDbContext.Account.CountAsync
                                    (dbAccount => dbAccount.UserId == userId).ConfigureAwait(false);

            return count > 0;
        }
    }
}

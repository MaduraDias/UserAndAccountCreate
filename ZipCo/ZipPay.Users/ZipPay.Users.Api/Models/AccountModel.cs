using System;
using ZipPay.Users.Entities;

namespace ZipPay.Users.Api.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }

        public UserModel User { get; set; }

        public static explicit operator AccountModel(Account account)
        {
            if(account == null)
            {
                return null;
            }

            return new AccountModel()
            {
                Id = account.UserId,
                User =(UserModel)account.User
            };
        }

    }
}

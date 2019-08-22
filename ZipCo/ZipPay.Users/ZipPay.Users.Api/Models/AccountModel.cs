using System;
using ZipPay.Users.Entities;

namespace ZipPay.Users.Api.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }

        public UserModel User { get; set; }

        public static explicit operator AccountModel(User user)
        {
            if(user == null)
            {
                return null;
            }

            return new UserModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                MonthlySalary = user.MonthlySalary,
                MonthlyExpenses = user.MonthlyExpenses
            };
        }

        public User ToEntity()
        {
            if(this == null)
            {
                return null;
            }

            return new User()
            {
                Id = Id,
                Name = Name,
                Email = Email,
                MonthlySalary = MonthlySalary,
                MonthlyExpenses = MonthlyExpenses
            };
        }


    }
}

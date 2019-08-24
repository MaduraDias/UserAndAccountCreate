using DataAnnotationsExtensions;
using System;
using System.ComponentModel.DataAnnotations;
using ZipPay.Users.Entities;

namespace ZipPay.Users.Api.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Min(0,ErrorMessage ="Salary should be a positive value")]
        public decimal MonthlySalary { get; set; }

        [Min(0, ErrorMessage = "Monthly Expenses should be a positive value")]
        public decimal MonthlyExpenses { get; set; }

        public static explicit operator UserModel(User user)
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

﻿using System;
using ZipPay.Users.Entities;

namespace ZipPay.Users.Api.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public decimal MonthlySalary { get; set; }

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
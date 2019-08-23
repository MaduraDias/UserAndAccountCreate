using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ZipPay.Users.BusinessService;
using ZipPay.Users.DataServices.Repositories;
using ZipPay.Users.Entities;

namespace ZipPay.Users.BusinessService.UnitTest
{
    public class WhenCreateAccount
    {
        private AccountService AccountService;
        private Mock<IUserRepository> UserRepositoryMock;
        private Mock<IAccountRepository> AccountRepositoryMock;

        private User MockUserWithEnoughIncome = new User()
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "test",
            MonthlySalary = 8000,
            MonthlyExpenses = 5000,
        };

        private User MockUserWithoutEnoughIncome = new User()
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "test",
            MonthlySalary = 8000,
            MonthlyExpenses = 7500,
        };

        [SetUp]
        public void Setup()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
           
            AccountRepositoryMock = new Mock<IAccountRepository>();
            AccountRepositoryMock.Setup(repo => repo.CreateAsync(MockUserWithEnoughIncome));
            AccountService = new AccountService(AccountRepositoryMock.Object,UserRepositoryMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenAccountAlreadyExistsForTheUser()
        {
            UserRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);

            AccountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<ValidationException>
                                (async () => await AccountService.CreateAsync(MockUserWithEnoughIncome));

            Assert.That(exception.Message, Is.EqualTo("Account already exists for the user"));

            UserRepositoryMock.Verify(repo => repo.CreateAsync(MockUserWithEnoughIncome), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenInvalidUserId()
        {
           
            AccountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            UserRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(false);

            var exception = Assert.ThrowsAsync<ValidationException>
                                (async () => await AccountService.CreateAsync(MockUserWithEnoughIncome));

            Assert.That(exception.Message, Is.EqualTo("User not found"));

            UserRepositoryMock.Verify(repo => repo.CreateAsync(MockUserWithEnoughIncome), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenUserMonthlySavingLessThanThousand()
        {

            AccountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(false);

            UserRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<ValidationException>
                                (async () => await AccountService.CreateAsync(MockUserWithoutEnoughIncome));

            Assert.That(exception.Message, Is.EqualTo("Monthly Salary - Monthly Expenses less than 1000"));

            UserRepositoryMock.Verify(repo => repo.CreateAsync(MockUserWithoutEnoughIncome), Times.Never);
        }

        [Test]
        public async Task ShouldCreateGivenNoValidationErrors()
        {

            AccountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(false);

            UserRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);

            await AccountService.CreateAsync(MockUserWithEnoughIncome);

            AccountRepositoryMock.Verify(repo => repo.CreateAsync(MockUserWithEnoughIncome), Times.Once);
        }

    }
}

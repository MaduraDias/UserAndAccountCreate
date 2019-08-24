using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ZipPay.Users.BusinessService.Exceptions;
using ZipPay.Users.DataServices.Repositories;
using ZipPay.Users.Entities;

namespace ZipPay.Users.BusinessService.UnitTest
{
    public class WhenCreateAccount
    {
        private AccountService accountService;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IAccountRepository> accountRepositoryMock;

        private User mockUserWithEnoughIncome = new User()
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "test",
            MonthlySalary = 8000,
            MonthlyExpenses = 5000,
        };

        private User mockUserWithoutEnoughIncome = new User()
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
            userRepositoryMock = new Mock<IUserRepository>();
           
            accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(repo => repo.CreateAsync(mockUserWithEnoughIncome.Id));
            accountService = new AccountService(accountRepositoryMock.Object,userRepositoryMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenAccountAlreadyExistsForTheUser()
        {
            userRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);

            accountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            var exception = Assert.ThrowsAsync<BusinessValidationException>
                                (async () => await accountService.CreateAsync(mockUserWithEnoughIncome.Id));

            Assert.That(exception.Message, Is.EqualTo("Account already exists for the user"));

            userRepositoryMock.Verify(repo => repo.CreateAsync(mockUserWithEnoughIncome), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenInvalidUserId()
        {
           
            accountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(true);

            userRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(false);

            var exception = Assert.ThrowsAsync<BusinessValidationException>
                                (async () => await accountService.CreateAsync(mockUserWithEnoughIncome.Id));

            Assert.That(exception.Message, Is.EqualTo("User not found"));

            userRepositoryMock.Verify(repo => repo.CreateAsync(mockUserWithEnoughIncome), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenUserMonthlySavingLessThanThousand()
        {

            accountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(false);

            userRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);

            userRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(mockUserWithoutEnoughIncome);

            var exception = Assert.ThrowsAsync<BusinessValidationException>
                                (async () => await accountService.CreateAsync(mockUserWithoutEnoughIncome.Id));

            Assert.That(exception.Message, Is.EqualTo("Monthly Salary - Monthly Expenses less than 1000"));

            userRepositoryMock.Verify(repo => repo.CreateAsync(mockUserWithoutEnoughIncome), Times.Never);
        }

        [Test]
        public async Task ShouldCreateGivenNoValidationErrors()
        {
         
            accountRepositoryMock.Setup(repo => repo.IsAccountExists(It.IsAny<Guid>()))
                                .ReturnsAsync(false);

            userRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);

            userRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(mockUserWithEnoughIncome);

            await accountService.CreateAsync(mockUserWithEnoughIncome.Id);

            accountRepositoryMock.Verify(repo => repo.CreateAsync(mockUserWithEnoughIncome.Id), Times.Once);
        }

    }
}

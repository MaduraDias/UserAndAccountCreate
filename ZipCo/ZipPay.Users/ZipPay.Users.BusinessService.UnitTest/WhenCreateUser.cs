using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ZipPay.Users.BusinessService;
using ZipPay.Users.DataServices.Repositories;
using ZipPay.Users.Entities;

namespace Tests
{
    public class WhenCreateUser
    {
        private UserService UserService;
        private Mock<IUserRepository> UserRepositoryMock;

        private User MockUser = new User()
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "test",
            MonthlySalary = 8000,
            MonthlyExpenses = 5000,
        };

        [SetUp]
        public void Setup()
        {
            UserRepositoryMock = new Mock<IUserRepository>();
            UserRepositoryMock.Setup(repo => repo.CreateAsync(MockUser));

            UserService = new UserService(UserRepositoryMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenEmailAlreadyExists()
        {
            UserRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(false);

            UserRepositoryMock.Setup(repo => repo.IsEmailExistsAsync(It.IsAny<string>()))
                              .ReturnsAsync(true);


            var exception = Assert.ThrowsAsync<ValidationException>
                                (async () => await UserService.CreateAsync(MockUser));

            Assert.That(exception.Message, Is.EqualTo("Email already exists"));

            UserRepositoryMock.Verify(repo => repo.CreateAsync(MockUser), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenUserIdAlreadyExists()
        {
            UserRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);


            var exception = Assert.ThrowsAsync<ValidationException>
                                (async () => await UserService.CreateAsync(MockUser));

            Assert.That(exception.Message, Is.EqualTo("User Id already exists"));
            UserRepositoryMock.Verify(repo => repo.CreateAsync(MockUser), Times.Never);
        }

        [Test]
        public async Task ShouldCreateGivenNoValidationErrors()
        {
            UserRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(false);

            UserRepositoryMock.Setup(repo => repo.IsEmailExistsAsync(It.IsAny<string>()))
                              .ReturnsAsync(false);

            await UserService.CreateAsync(MockUser);

            UserRepositoryMock.Verify(repo => repo.CreateAsync(MockUser), Times.Once());
        }

       
    }
}
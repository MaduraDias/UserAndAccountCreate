using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ZipPay.Users.BusinessService;
using ZipPay.Users.BusinessService.Exceptions;
using ZipPay.Users.DataServices.Repositories;
using ZipPay.Users.Entities;

namespace Tests
{
    public class WhenCreateUser
    {
        private UserService userService;
        private Mock<IUserRepository> userRepositoryMock;

        private User mockUser = new User()
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
            userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.CreateAsync(mockUser));

            userService = new UserService(userRepositoryMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenEmailAlreadyExists()
        {
            userRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(false);

            userRepositoryMock.Setup(repo => repo.IsEmailExistsAsync(It.IsAny<string>()))
                              .ReturnsAsync(true);


            var exception = Assert.ThrowsAsync<BusinessValidationException>
                                (async () => await userService.CreateAsync(mockUser));

            Assert.That(exception.Message, Is.EqualTo("Email already exists"));

            userRepositoryMock.Verify(repo => repo.CreateAsync(mockUser), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAndNotSaveGivenUserIdAlreadyExists()
        {
            userRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(true);


            var exception = Assert.ThrowsAsync<BusinessValidationException>
                                (async () => await userService.CreateAsync(mockUser));

            Assert.That(exception.Message, Is.EqualTo("User Id already exists"));
            userRepositoryMock.Verify(repo => repo.CreateAsync(mockUser), Times.Never);
        }

        [Test]
        public async Task ShouldCreateGivenNoValidationErrors()
        {
            userRepositoryMock.Setup(repo => repo.IsUserIdExistsAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(false);

            userRepositoryMock.Setup(repo => repo.IsEmailExistsAsync(It.IsAny<string>()))
                              .ReturnsAsync(false);

            await userService.CreateAsync(mockUser);

            userRepositoryMock.Verify(repo => repo.CreateAsync(mockUser), Times.Once());
        }

       
    }
}
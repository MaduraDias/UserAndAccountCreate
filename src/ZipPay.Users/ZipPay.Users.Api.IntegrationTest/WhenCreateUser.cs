using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ZipPay.Users.Api.IntegrationTest;
using ZipPay.Users.Api.Models;

namespace Tests
{
    public class WhenCreateUser:BaseApiIntegrationTest
    {
        [Test]
        public async Task ShouldSaveAndAbleToGetBackGivenNoValidationErrors()
        {
             var userModel = new UserModel()
             {
                 Id = Guid.NewGuid(),
                 Name = "Test",
                 Email = $"{Guid.NewGuid()}@test.com.au",
                 MonthlySalary = 10000,
                 MonthlyExpenses = 5000
             };

           StringContent httpContent = TestHelper.CreateHttpContentFromModel(userModel);

            //Create User
            var createResult = await client.PostAsync(ApiUrl.UserApiUrl, httpContent);
            Assert.AreEqual(HttpStatusCode.Created, createResult.StatusCode, "Invalid Http Status Code");

            //Get User
            var getByIdResult = await client.GetAsync($"{ApiUrl.UserApiUrl}/{userModel.Id}");
            Assert.AreEqual(200, (int)getByIdResult.StatusCode, "Failed at retrieve");

            var bodyContent = await getByIdResult.Content.ReadAsStringAsync();
            var retrivedUser = JsonConvert.DeserializeObject<UserModel>(bodyContent);

            Assert.AreEqual(userModel.Id, retrivedUser.Id);
            Assert.AreEqual(userModel.Name, retrivedUser.Name);
            Assert.AreEqual(userModel.Email, retrivedUser.Email);
        }

      
        [Test]
        public async Task ShouldReturnBadRequestWithValidErrorMessageGivenEmailAlreadyExists()
        {
            var userModel = new UserModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = $"{Guid.NewGuid()}@test.com.au",
                MonthlySalary = 10000,
                MonthlyExpenses = 5000
            };

            StringContent httpContentForFirstTry = TestHelper.CreateHttpContentFromModel(userModel);

            var createResponseForFirstTry = await client.PostAsync(ApiUrl.UserApiUrl, httpContentForFirstTry);
            Assert.AreEqual(HttpStatusCode.Created, createResponseForFirstTry.StatusCode, "Failed at create");

            var secondUser = new UserModel()
            {
                Id = Guid.NewGuid(),
                Name = "TestName 2",
                Email = userModel.Email,
                MonthlySalary = 10000,
                MonthlyExpenses = 5000
            };

            StringContent httpContentForSecondTry = TestHelper.CreateHttpContentFromModel(secondUser);
            var createResponseForSecondTry = await client.PostAsync(ApiUrl.UserApiUrl, httpContentForSecondTry);

            Assert.AreEqual(HttpStatusCode.BadRequest,createResponseForSecondTry.StatusCode, "Invalid Http Status Code");

            var errorMessage = await TestHelper
                                    .GetErrorMessageFromBadRequestContent(createResponseForSecondTry.Content);

            Assert.AreEqual("Email already exists", errorMessage);
        }

        
    }
}
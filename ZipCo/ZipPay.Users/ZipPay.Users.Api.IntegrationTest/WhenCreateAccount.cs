using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ZipPay.Users.Api.IntegrationTest;
using ZipPay.Users.Api.Models;

namespace Tests
{
    public class WhenCreateAccount: BaseApiIntegrationTest
    {
        [Test]
        public async Task ShouldSaveGivenNoValidationErrors()
        {
            var userModel = new UserModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = $"{Guid.NewGuid()}@test.com.au",
                MonthlySalary = 10000,
                MonthlyExpenses = 5000
            };

            await CreateUser(userModel);

            //Create Account
            StringContent httpContent = TestHelper.CreateHttpContentFromUserModel(userModel);
            var accountCreateResponse = await client.PostAsync(ApiUrl.AccountApiUrl, httpContent);

            Assert.AreEqual(HttpStatusCode.Created, accountCreateResponse.StatusCode, "Account creation Failed");
            var accountListGetResponse = await client.GetAsync(ApiUrl.AccountApiUrl);

            Assert.AreEqual(HttpStatusCode.OK, accountListGetResponse.StatusCode);


            //Get Account List and search the created
            var getAccountReponseContent = await accountListGetResponse
                                                    .Content
                                                    .ReadAsStringAsync();

            var accountList = JsonConvert
                            .DeserializeObject<List<AccountModel>>(getAccountReponseContent);

            var isCreatedAccountFound = accountList
                                        .Count(acc => acc.Id == userModel.Id
                                                && acc.User.Email == userModel.Email) > 0;


            Assert.IsTrue(isCreatedAccountFound, "Created account not found in the list");
        }


        [Test]
        public async Task ShouldReturnBadRequestWithValidErrorMessageGivenUserFinancialSituationNotSatisfied()
        {
            var userModel = new UserModel()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = $"{Guid.NewGuid()}@test.com.au",
                MonthlySalary = 5500,
                MonthlyExpenses = 5000
            };

            await CreateUser(userModel);

            //Try creating Account
            StringContent httpContent = TestHelper.CreateHttpContentFromUserModel(userModel);
            var accountCreateResponse = await client.PostAsync(ApiUrl.AccountApiUrl, httpContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, accountCreateResponse.StatusCode, "Invalid Http Status Code");

            var errorMessage = await TestHelper
                                     .GetErrorMessageFromBadRequestContent(accountCreateResponse.Content);

            Assert.AreEqual("Monthly Salary - Monthly Expenses less than 1000", errorMessage);

        }

        private async Task CreateUser(UserModel userModel)
        {
            StringContent httpContent = TestHelper.CreateHttpContentFromUserModel(userModel);
            await client.PostAsync(ApiUrl.UserApiUrl, httpContent);
        }


    }
}
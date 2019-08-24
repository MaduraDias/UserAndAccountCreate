using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZipPay.Users.Api.Models;

namespace ZipPay.Users.Api.IntegrationTest
{
    public static class TestHelper
    {
        public static StringContent CreateHttpContentFromUserModel(UserModel userModel)
        {
            var user = JsonConvert.SerializeObject(userModel);
            var httpContent = new StringContent(user, System.Text.Encoding.UTF8, "application/json");
            return httpContent;
        }

        public static async Task<string> GetErrorMessageFromBadRequestContent(HttpContent httpContent)
        {
            var error = await httpContent.ReadAsStringAsync();
            var problemDetail = JsonConvert.DeserializeObject<ProblemDetails>(error);
            return problemDetail.Detail;
        }
    }
}

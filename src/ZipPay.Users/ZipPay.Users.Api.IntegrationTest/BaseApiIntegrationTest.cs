using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ZipPay.Users.Api.IntegrationTest
{
    public class BaseApiIntegrationTest
    {
        protected ApiWebApplicationFactory factory;
        protected HttpClient client;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            factory = new ApiWebApplicationFactory();
            client = factory.CreateClient();

        }
    }
}

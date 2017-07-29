using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace UnitTests
{

    [TestClass]
    public class AzureTests
    {
        [TestMethod]
        public void TestConnectionToAzureTables()
        {
            var client = new HttpClient { DefaultRequestHeaders = { { "ZUMO-API-VERSION", "2.0.0" } } };
            const string tablesUrl = "https://avesnz.azurewebsites.net/tables/SearchHistoryModel";

            var response = client.PostAsync(tablesUrl, null).Result.IsSuccessStatusCode;

            Assert.IsTrue(response);
        }
    }
}

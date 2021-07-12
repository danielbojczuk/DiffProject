using DiffProject.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DiffProject.Tests.IntegrationTests
{
    public class GetComparisonResultTest
    {
        private readonly TestServer _webServer;
        private readonly HttpClient _webClient;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        private string _LoremIpsumOnePath;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        private string _LoremIpsumTwoPath;

        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        private string LoremIpsumOneBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath));
            }
        }

        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        private string LoremIpsumTwoBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumTwoPath));
            }
        }

        public GetComparisonResultTest()
        {
            _LoremIpsumOnePath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum1.txt");
            _LoremIpsumTwoPath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum2.txt");
            _webServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _webClient = _webServer.CreateClient();
        }

 
        [Fact]
        public async void ComparisonResultGet()
        {
            string comparisonId = Guid.NewGuid().ToString();
            StringContent contentToSendRight = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");
            StringContent contentToSendLeft = new StringContent(JsonSerializer.Serialize(LoremIpsumTwoBase64), Encoding.UTF8, "application/json");

            //ExecutePost

            await _webClient.PostAsync($"/v1/diff/{comparisonId}/right", contentToSendRight);
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/left", contentToSendLeft);


            HttpResponseMessage responseMessage = await _webClient.GetAsync($"/v1/diff/{comparisonId}");
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal("{\"sidesEqual\":false,\"sameSize\":true,\"differences\":{\"12\":5,\"24\":2},\"comparisonId\":\""+comparisonId+"\"}", responseString);
        }

        [Fact]
        public async void ComparisonResultNotFoundGet()
        {
            string comparisonId = Guid.NewGuid().ToString();
            HttpResponseMessage responseMessage = await _webClient.GetAsync($"/v1/diff/{comparisonId}");
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }


    }
}

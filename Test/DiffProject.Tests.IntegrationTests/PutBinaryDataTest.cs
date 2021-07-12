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
    public class PutBinaryDataTest
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

        private string LoremIpsumTwoBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumTwoPath));
            }
        }

        public PutBinaryDataTest()
        {
            _LoremIpsumOnePath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum1.txt");
            _LoremIpsumTwoPath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum2.txt");
            _webServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _webClient = _webServer.CreateClient();
        }

 
        [Fact]
        public async void ValidUpdate()
        {
            //values to use in test
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);

            
            contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumTwoBase64), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _webClient.PutAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void NotExistingBinaryDataUpdate()
        {
            //values to use in test
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await _webClient.PutAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Comparison Id not found\"]", responseString);
        }

    }
}

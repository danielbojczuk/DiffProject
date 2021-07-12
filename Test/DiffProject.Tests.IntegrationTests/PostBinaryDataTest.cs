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
    public class PostBinaryDataTest
    {
        private readonly TestServer _webServer;
        private readonly HttpClient _webClient;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        private string _LoremIpsumOnePath;

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

        public PostBinaryDataTest()
        {
            _LoremIpsumOnePath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum1.txt");
            _webServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _webClient = _webServer.CreateClient();
        }

 
        [Fact]
        public async void InvalidIdPost()
        {
            //values to use in test
            string comparisonId = "InvalidId";
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString =  await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Supplied ID is invalid\"]", responseString);
        }

        [Fact]
        public async void InvalidSidePost()
        {
            //values to use in test
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "InvalidSide";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Supplied SIDE is invalid\"]", responseString);
        }

        [Fact]
        public async void InvalidIdAndSidePost()
        {
            //values to use in test
            string comparisonId = "InvalidId";
            string comparisonSide = "InvalidSide";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Supplied ID is invalid\",\"Supplied SIDE is invalid\"]", responseString);
        }

        [Fact]
        public async void InvalidBase64Post()
        {
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize("InvalidBase64"), Encoding.UTF8, "application/json");

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Invalid Base64 String\"]", responseString);
        }

        [Fact]
        public async void ValidPosting()
        {
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
        }

        [Fact]
        public async void DuplicatedPosting()
        {
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"There is already a Binary Data with this Comparison Id and Side\"]", responseString);
        }

        [Fact]
        public async void PostingBothSides()
        {
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);

            comparisonId = Guid.NewGuid().ToString();
            comparisonSide = "left";

            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
        }
    }
}

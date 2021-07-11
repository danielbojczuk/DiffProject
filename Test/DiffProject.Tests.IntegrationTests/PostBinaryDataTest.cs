using DiffProject.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace DiffProject.Tests.IntegrationTests
{
    public class PostBinaryDataTest
    {
        private readonly TestServer _webServer;
        private readonly HttpClient _webClient;

        public PostBinaryDataTest()
        {
            _webServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _webClient = _webServer.CreateClient();
        }

 
        [Fact]
        public async void InvalidIdPost()
        {
            //values to use in test
            string comparisonId = "InvalidId";
            string comparisonSide = "right";
            string contentToSend = string.Empty;

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", new StringContent(contentToSend));
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
            string contentToSend = string.Empty;

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", new StringContent(contentToSend));
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
            string contentToSend = string.Empty;

            //ExecutePost
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", new StringContent(contentToSend));
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Supplied ID is invalid\",\"Supplied SIDE is invalid\"]", responseString);
        }
    }
}

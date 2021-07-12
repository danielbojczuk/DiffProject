using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DiffProject.Tests.IntegrationTests
{
    /// <summary>
    /// Integration tests for Posting Binary Data
    /// </summary>
    public class PostBinaryDataTest : AbstractTestClass
    {
        [Fact]
        public async void InvalidIdPost()
        {
            string comparisonId = "InvalidId";
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            
            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Supplied ID is invalid\"]", responseString);
        }

        [Fact]
        public async void InvalidSidePost()
        {
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "InvalidSide";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
            Assert.Equal("[\"Supplied SIDE is invalid\"]", responseString);
        }

        [Fact]
        public async void InvalidIdAndSidePost()
        {
            string comparisonId = "InvalidId";
            string comparisonSide = "InvalidSide";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

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

            await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);

            comparisonId = Guid.NewGuid().ToString();
            comparisonSide = "left";

            HttpResponseMessage responseMessage = await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
        }
    }
}

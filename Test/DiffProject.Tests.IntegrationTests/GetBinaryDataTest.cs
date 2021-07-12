using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DiffProject.Tests.IntegrationTests
{
    /// <summary>
    /// Integration tests to Get the Binary Data
    /// </summary>
    public class GetBinaryDataTest : AbstractTestClass
    {
        [Fact]
        public async void BinaryDataGet()
        {
            string comparisonId = Guid.NewGuid().ToString();
            StringContent contentToSendRight = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");
            
            // Post a binary data
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/right", contentToSendRight);

            // Get the posted bynary data
            HttpResponseMessage responseMessage = await _webClient.GetAsync($"/v1/diff/{comparisonId}/right");
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void BinaryDataNotFoundGet()
        {
            string comparisonId = Guid.NewGuid().ToString();

            HttpResponseMessage responseMessage = await _webClient.GetAsync($"/v1/diff/{comparisonId}/right");
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }
    }
}

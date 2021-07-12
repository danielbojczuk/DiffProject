using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DiffProject.Tests.IntegrationTests
{
    /// <summary>
    /// Integration tests for updating Binary Data.
    /// </summary>
    public class PutBinaryDataTest : AbstractTestClass
    {
        [Fact]
        public async void ValidUpdate()
        {
            string comparisonId = Guid.NewGuid().ToString();
            string comparisonSide = "right";
            StringContent contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            await _webClient.PostAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);

            contentToSend = new StringContent(JsonSerializer.Serialize(LoremIpsumTwoBase64), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await _webClient.PutAsync($"/v1/diff/{comparisonId}/{comparisonSide}", contentToSend);
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Fact]
        public async void NotExistingBinaryDataUpdate()
        {
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

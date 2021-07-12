using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DiffProject.Tests.IntegrationTests
{
    /// <summary>
    /// Integration tests to get the Differences.
    /// </summary>
    public class GetComparisonResultTest : AbstractTestClass
    {
        [Fact]
        public async void ComparisonResultGet()
        {
            string comparisonId = Guid.NewGuid().ToString();
            StringContent contentToSendRight = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");
            StringContent contentToSendLeft = new StringContent(JsonSerializer.Serialize(LoremIpsumTwoBase64), Encoding.UTF8, "application/json");

            // Posting both sides.   
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/right", contentToSendRight);
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/left", contentToSendLeft);

            // Retrieving the differences
            HttpResponseMessage responseMessage = await _webClient.GetAsync($"/v1/diff/{comparisonId}");
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Equal("{\"sidesEqual\":false,\"sameSize\":true,\"differences\":{\"12\":5,\"24\":2},\"comparisonId\":\"" + comparisonId + "\"}", responseString);
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

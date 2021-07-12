using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DiffProject.Tests.IntegrationTests
{
    public class GetBinaryDataTest : AbstractTestClass
    {
        [Fact]
        public async void BinaryDataGet()
        {
            string comparisonId = Guid.NewGuid().ToString();
            StringContent contentToSendRight = new StringContent(JsonSerializer.Serialize(LoremIpsumOneBase64), Encoding.UTF8, "application/json");

            //ExecutePost
            await _webClient.PostAsync($"/v1/diff/{comparisonId}/right", contentToSendRight);


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

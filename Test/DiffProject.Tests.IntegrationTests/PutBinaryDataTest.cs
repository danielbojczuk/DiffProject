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
    public class PutBinaryDataTest:AbstractTestClass
    {
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

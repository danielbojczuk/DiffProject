using DiffProject.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Tests.IntegrationTests
{
    public abstract class AbstractTestClass
    {
        protected readonly TestServer _webServer;
        protected readonly HttpClient _webClient;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        protected readonly string _LoremIpsumOnePath;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        protected readonly string _LoremIpsumTwoPath;

        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        protected string LoremIpsumOneBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath));
            }
        }

        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        protected string LoremIpsumTwoBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumTwoPath));
            }
        }

        public AbstractTestClass()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testSettings.json")
                .Build();
            _LoremIpsumOnePath = configuration.GetSection("BinaryData").GetValue<string>("BinaryDataOne");
            _LoremIpsumTwoPath = configuration.GetSection("BinaryData").GetValue<string>("BinaryDataTwo");

            _webServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _webClient = _webServer.CreateClient();
        }

    }
}

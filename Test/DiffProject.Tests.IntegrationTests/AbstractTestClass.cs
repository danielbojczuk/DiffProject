using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using DiffProject.WebAPI;

namespace DiffProject.Tests.IntegrationTests
{
    /// <summary>
    /// Abstract class to provide the Web Client and Web Server to the integration testes as well the
    /// Binary Data to use in the tests.
    /// </summary>
    public abstract class AbstractTestClass
    {
        protected readonly TestServer _webServer;
        protected readonly HttpClient _webClient;

        protected readonly string _LoremIpsumOnePath;

        protected readonly string _LoremIpsumTwoPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTestClass"/> class.
        /// It will use de config file testSettings.json.
        /// </summary>
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

        protected string LoremIpsumOneBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath));
            }
        }

        protected string LoremIpsumTwoBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumTwoPath));
            }
        }


    }
}

using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;



namespace DiffProject.Tests.UnitTests
{
    public abstract class AbstractTestClass
    {
        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        protected string _LoremIpsumOnePath;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        protected string _LoremIpsumTwoPath;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        protected string _LoremIpsumDifferentSizePath;

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


        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        protected string LoremIpsumDifferentSizeBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumDifferentSizePath));
            }
        }

        /// <summary>
        /// Repository mocking to be used in the tests
        /// </summary>
        protected Mock<IBinaryDataRepository> _binaryDataRepositoryMock;
        protected Mock<IComparisonResultRepository> _comparisonResultRepository;

        /// <summary>
        /// Notification context required for the CommandHandler
        /// </summary>
        protected NotificationContext _notificationContext;
        public AbstractTestClass()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testSettings.json")
                .Build();
            _LoremIpsumOnePath = configuration.GetSection("BinaryData").GetValue<string>("BinaryDataOne");
            _LoremIpsumTwoPath = configuration.GetSection("BinaryData").GetValue<string>("BinaryDataTwo");
            _LoremIpsumDifferentSizePath = configuration.GetSection("BinaryData").GetValue<string>("BinaryDataDifferentSize");

            _binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
            _comparisonResultRepository = new Mock<IComparisonResultRepository>();
            _notificationContext = new NotificationContext();
        }
    }
}

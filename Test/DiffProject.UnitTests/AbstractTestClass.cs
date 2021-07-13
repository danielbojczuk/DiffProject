using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Moq;

namespace DiffProject.Tests.UnitTests
{
    /// <summary>
    /// Abstract class to provide the Binary Data sets to be used in the tests.
    /// </summary>
    public abstract class AbstractTestClass
    {
        protected readonly string _LoremIpsumOnePath;

        protected readonly string _LoremIpsumTwoPath;

        protected readonly string _LoremIpsumDifferentSizePath;

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


        protected string LoremIpsumDifferentSizeBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumDifferentSizePath));
            }
        }

        protected readonly Mock<IBinaryDataRepository> _binaryDataRepositoryMock;
        protected readonly Mock<IComparisonResultRepository> _comparisonResultRepository;

        protected readonly NotificationContext _notificationContext;

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
            _LoremIpsumDifferentSizePath = configuration.GetSection("BinaryData").GetValue<string>("BinaryDataDifferentSize");

            _binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
            _comparisonResultRepository = new Mock<IComparisonResultRepository>();
            _notificationContext = new NotificationContext();
        }
    }
}

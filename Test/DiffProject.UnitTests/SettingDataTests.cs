using System;
using Xunit;
using System.IO;
using DiffProject.Application.CommandHandlers;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using Moq;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;

namespace DiffProject.Tests.UnitTests
{
    
    public class SettingDataTests
    {
        private string _LoremIpsumOne = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum1.txt");


        [Fact]
        public async void SetNewComparisonRightData()
        {
            Mock<IDiffComparisonRepository> diffComparisonMock = new Mock<IDiffComparisonRepository>();
            diffComparisonMock.Setup(d => d.RetrieveDiffComparisonById(It.IsAny<Guid>())).Returns<DiffComparison>(null);
            diffComparisonMock.Setup(d => d.Add(It.IsAny<DiffComparison>()));

            SetDataCommandHandler commandHandler = new SetDataCommandHandler(diffComparisonMock.Object);
            Guid? newId = await commandHandler.ExecuteAsync(new SetDataCommand
            {
                ComparisonID = Guid.NewGuid(),
                ComparisonSide = ComparisonSideEnum.Left,
                Base64BinaryData = Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOne))
            });

            Assert.IsType<Guid>(newId);
        }
    }
}

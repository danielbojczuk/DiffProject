using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using DiffProject.Application.CommandHandlers;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Moq;

namespace DiffProject.Tests.UnitTests
{
    /// <summary>
    /// Unit tests to Set the Binary Data.
    /// </summary>
    public class SettingBinaryDataTests : AbstractTestClass
    {
        private async Task<SetBinaryDataResponse> ExecuteCommand(Guid comparisonId, SideEnum comparisonSide, string base64EncodedString)
        {
            SetBinaryDataCommandHandler commandHandler = new SetBinaryDataCommandHandler(_binaryDataRepositoryMock.Object, _comparisonResultRepository.Object,_notificationContext);
            return await commandHandler.Handle(new SetBinaryDataCommand
            {
                ComparisonID = comparisonId,
                ComparisonSide = comparisonSide,
                Base64BinaryData = base64EncodedString
            }, new CancellationToken());
        }

        [Fact]
        public async void SetNewBinaryDataNewIdAndNewSide()
        {
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.left;

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            SetBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            Assert.IsType<SetBinaryDataResponse>(newBinaryData);
        }

        [Fact]
        public async void SetNewBinaryDataExistingIdAndNewSide()
        {
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath)), comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Left)).ReturnsAsync(binaryData);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Right)).Returns(Task.FromResult<BinaryData>(null));

            SetBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            
            Assert.IsType<SetBinaryDataResponse>(newBinaryData);
        }


        [Fact]
        public async void SetBinaryDataAlreadyExists()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.left;

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Left)).ReturnsAsync(binaryData);          
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));
            
            SetBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);
            
            Assert.Null(newBinaryData);
        }

        [Fact]
        public async void SetBinaryDataWithInvalidBase64()
        {
            Mock<IBinaryDataRepository> binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
            binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns<Task<BinaryData>>(null);

            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            SetBinaryDataResponse newBinaryData = await ExecuteCommand(Guid.NewGuid(), SideEnum.left, "NotABase64");

            Assert.Null(newBinaryData);
        }
    }
}

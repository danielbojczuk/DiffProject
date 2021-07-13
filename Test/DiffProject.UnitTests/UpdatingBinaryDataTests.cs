using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using DiffProject.Application.CommandHandlers;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using Moq;


namespace DiffProject.Tests.UnitTests
{
    /// <summary>
    /// Unit tests to Update the Binary Data.
    /// </summary>
    public class UpdatingBinaryDataTests : AbstractTestClass
    {
        private async Task<UpdateBinaryDataResponse> ExecuteCommand(Guid comparisonId, SideEnum comparisonSide, string base64EncodedString)
        {
            UpdateBinaryDataCommandHandler commandHandler = new UpdateBinaryDataCommandHandler(_binaryDataRepositoryMock.Object, _notificationContext, _comparisonResultRepository.Object);
            return await commandHandler.Handle(new UpdateBinaryDataCommand
            {
                CurrentComparisonID = comparisonId,
                CurrentComparisonSide = comparisonSide,
                NewBase64BinaryData = base64EncodedString
            }, new CancellationToken());
        }

        [Fact]
        public async void UpdateBinaryData()
        {
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).ReturnsAsync(binaryData);
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            UpdateBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            Assert.IsType<UpdateBinaryDataResponse>(newBinaryData);
        }

        [Fact]
        public async void UpdateBinaryDataNoDataToUpdate()
        {
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;


            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), ComparisonSideEnum.Left)).ReturnsAsync(binaryData);
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            UpdateBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            Assert.Null(newBinaryData);
        }

        [Fact]
        public async void UpdateBinaryDataInvalidBase64()
        {
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;
            string base64String = "NotABase64";

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult(binaryData));
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            UpdateBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, base64String);

            Assert.Null(newBinaryData);
        }
    }
}

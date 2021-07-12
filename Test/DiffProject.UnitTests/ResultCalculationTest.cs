using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using DiffProject.Application.CommandHandlers;
using DiffProject.Application.Commands;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using Moq;

namespace DiffProject.Tests.UnitTests
{

    public class ResultCalculationTest : AbstractTestClass
    {
        private async Task<ComparisonResultResponse> ExecuteCommand(Guid comparisonId)
        {
            CompareBinaryDataCommandHandler commandHandler = new CompareBinaryDataCommandHandler(_binaryDataRepositoryMock.Object, _notificationContext, _comparisonResultRepository.Object);
            return await commandHandler.Handle(new CompareBinaryDataCommand
            {
                ComparisonIs = comparisonId,
            }, new CancellationToken());
        }


        [Fact]
        public async void TryToCompareWithJustOneSide()
        {
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            var result = await ExecuteCommand(comparisonId);

            Assert.Null(result);
        }

        [Fact]
        public async void TryToCompareWithDuplicatedSide()
        {
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            var result = await ExecuteCommand(comparisonId);

            Assert.Null(result);
        }

        [Fact]
        public async void TryToCompareWithThreeBinaryData()
        {
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            var result = await ExecuteCommand(comparisonId);

            Assert.Null(result);
        }

        [Fact]
        public async void CompareEqualFiles()
        {
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            ComparisonResultResponse result = await ExecuteCommand(comparisonId);

            Assert.True(result.SidesEqual);
            Assert.True(result.SameSize);
            Assert.Empty(result.Differences);
        }

        [Fact]
        public async void CompareDifferentSizeFiles()
        {
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumDifferentSizeBase64, comparisonId, _binaryDataRepositoryMock.Object));

            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            ComparisonResultResponse result = await ExecuteCommand(comparisonId);

            Assert.False(result.SidesEqual);
            Assert.False(result.SameSize);
            Assert.Empty(result.Differences);
        }

        [Fact]
        public async void CompareEqualSizeDifferentFiles()
        {
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumTwoBase64, comparisonId, _binaryDataRepositoryMock.Object));
            Dictionary<long, long> expectedDifferences = new Dictionary<long, long>();
            expectedDifferences.Add(12, 5);
            expectedDifferences.Add(24, 2);


            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            ComparisonResultResponse result = await ExecuteCommand(comparisonId);

            Assert.False(result.SidesEqual);
            Assert.True(result.SameSize);
            Assert.Equal(expectedDifferences, result.Differences);

        }

    }
}

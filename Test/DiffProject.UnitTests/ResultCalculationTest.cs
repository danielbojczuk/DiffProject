using DiffProject.Application.CommandHandlers;
using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DiffProject.Tests.UnitTests
{

    public class ResultCalculationTest: AbstractTestClass
    {
      

        /// <summary>
        /// Method to execute the command to set the new BinaryData
        /// </summary>
        /// <param name="comparisonId">Id of the comparison</param>
        /// <param name="comparisonSide">Comparison's side</param>
        /// <param name="base64EncodedString"> Binary data Base64 encoded string to be compared</param>
        /// <returns></returns>
        private async Task<ComparisonResultResponse> ExecuteCommand(Guid comparisonId)
        {
            CalculationCommandHandler commandHandler = new CalculationCommandHandler(_binaryDataRepositoryMock.Object, _notificationContext, _comparisonResultRepository.Object);
            return await commandHandler.Handle(new ComparisonResultCommand
            {
                ComparisonID = comparisonId,
            }, new CancellationToken());
        }


        [Fact]
        public async void TryToCompareWithJustOneSide()
        {
            //Setting values to be uded in the testss
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            //Mocking the return of the binary data to compare
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            //Executing commnad
            var result = await ExecuteCommand(comparisonId);

            Assert.Null(result);
        }

        [Fact]
        public async void TryToCompareWithDuplicatedSide()
        {
            //Setting values to be uded in the testss
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            //Mocking the return of the binary data to compare
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            //Executing commnad
            var result = await ExecuteCommand(comparisonId);

            Assert.Null(result);
        }

        [Fact]
        public async void TryToCompareWithThreeBinaryData()
        {
            //Setting values to be uded in the testss
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            //Mocking the return of the binary data to compare
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            //Executing commnad
            var result = await ExecuteCommand(comparisonId);

            Assert.Null(result);
        }

        [Fact]
        public async void CompareEqualFiles()
        {
            //Setting values to be uded in the testss
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));

            //Mocking the return of the binary data to compare
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            //Executing commnad
            ComparisonResultResponse result = await ExecuteCommand(comparisonId);

            Assert.True(result.SidesEqual);
            Assert.True(result.SameSize);
            Assert.Empty(result.Differences);
        }

        [Fact]
        public async void CompareDifferentSizeFiles()
        {
            //Setting values to be uded in the testss
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumDifferentSizeBase64, comparisonId, _binaryDataRepositoryMock.Object));

            //Mocking the return of the binary data to compare
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            //Executing commnad
            ComparisonResultResponse result = await ExecuteCommand(comparisonId);

            Assert.False(result.SidesEqual);
            Assert.False(result.SameSize);
            Assert.Empty(result.Differences);
        }

        [Fact]
        public async void CompareEqualSizeDifferentFiles()
        {
            //Setting values to be uded in the testss
            Guid comparisonId = Guid.NewGuid();
            List<BinaryData> binarryDataList = new List<BinaryData>();
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object));
            binarryDataList.Add(new BinaryData(ComparisonSideEnum.Right, LoremIpsumTwoBase64, comparisonId, _binaryDataRepositoryMock.Object));
            Dictionary<long, long> expectedDifferences = new Dictionary<long, long>();
            expectedDifferences.Add(12, 5);
            expectedDifferences.Add(24, 2);


            //Mocking the return of the binary data to compare
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonId(comparisonId)).ReturnsAsync(binarryDataList);

            //Executing commnad
            ComparisonResultResponse result = await ExecuteCommand(comparisonId);

            Assert.False(result.SidesEqual);
            Assert.True(result.SameSize);
            Assert.Equal(expectedDifferences, result.Differences);

        }

    }
}

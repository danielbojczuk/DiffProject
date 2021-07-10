using DiffProject.Application.CommandHandlers;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DiffProject.Tests.UnitTests
{

    public class ResultCalculationTest
    {
        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        private string _LoremIpsumOnePath;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        private string _LoremIpsumTwoPath;

        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        private string _LoremIpsumDifferentSizePath;

        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        private string LoremIpsumOneBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath));
            }
        }

        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        private string LoremIpsumTwoBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumTwoPath));
            }
        }


        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        private string LoremIpsumDifferentSizeBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumTwoPath));
            }
        }

        /// <summary>
        /// Repository mocking to be used in the tests
        /// </summary>
        Mock<IBinaryDataRepository> _binaryDataRepositoryMock;

        public ResultCalculationTest()
        {
            _LoremIpsumOnePath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum1.txt");
            _LoremIpsumTwoPath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum2.txt");
            _LoremIpsumDifferentSizePath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsumDifferentSize.txt");
            _binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
        }

        /// <summary>
        /// Method to execute the command to set the new BinaryData
        /// </summary>
        /// <param name="comparisonId">Id of the comparison</param>
        /// <param name="comparisonSide">Comparison's side</param>
        /// <param name="base64EncodedString"> Binary data Base64 encoded string to be compared</param>
        /// <returns></returns>
        private async Task<ComparisonResult> ExecuteCommand(Guid comparisonId)
        {
            CalculationCommandHandler commandHandler = new CalculationCommandHandler(_binaryDataRepositoryMock.Object);
            return await commandHandler.ExecuteAsync(new CalculationCommand
            {
                ComparisonID = comparisonId,
            });
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
        public async void TryToCompareWithThreeSides()
        {
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
        public async void CompareRqualFiles()
        {
            Assert.Equal(1, 2);
        }

        [Fact]
        public async void CompareDifferentSizeFiles()
        {
            Assert.Equal(1, 2);
        }

        [Fact]
        public async void CompareIqualSizeDifferentFiles()
        {
            Assert.Equal(1, 2);
        }

        public async void CompareAgain()
        {
            Assert.Equal(1, 2);
        }


    }
}

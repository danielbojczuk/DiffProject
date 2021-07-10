using System;
using Xunit;
using System.IO;
using DiffProject.Application.CommandHandlers;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using Moq;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using System.Threading.Tasks;

namespace DiffProject.Tests.UnitTests
{
    
    public class UpdatingBinaryDataTests
    {
        /// <summary>
        /// Field with the LoeremImpsum file path to be used in the tests
        /// </summary>
        private string _LoremIpsumOnePath;

        /// <summary>
        /// Property to return the base64 encoded string to be used in the tests
        /// </summary>
        private string LoremIpsumOneBase64 { get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath));
            }
        }

        /// <summary>
        /// Repository mocking to be used in the tests
        /// </summary>
        Mock<IBinaryDataRepository> _binaryDataRepositoryMock;

        public UpdatingBinaryDataTests()
        {
            _LoremIpsumOnePath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum1.txt");
            _binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
        }

        /// <summary>
        /// Method to execute the command to set the new BinaryData
        /// </summary>
        /// <param name="comparisonId">Id of the comparison</param>
        /// <param name="comparisonSide">Comparison's side</param>
        /// <param name="base64EncodedString"> Binary data Base64 encoded string to be compared</param>
        /// <returns></returns>
        private async Task<BinaryData> ExecuteCommand(Guid comparisonId, SideEnum comparisonSide, string base64EncodedString)
        {
            UpdateBinaryDataCommandHandler commandHandler = new UpdateBinaryDataCommandHandler(_binaryDataRepositoryMock.Object);
            return await commandHandler.ExecuteAsync(new UpdateBinaryDataCommand
            {
                CurrentComparisonID = comparisonId,
                CurrentComparisonSide = comparisonSide,
                NewBase64BinaryData = base64EncodedString
            });
        }

        [Fact]
        public async void UpdateBinaryData()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.Right;


            //Mocking the repository to return null for every query in order to create a valid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to get BinaryData to update
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult(binaryData));

            //Mocking the repository to return a BinaryData on update statement
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).Returns(Task.FromResult(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object)));

            //Executing the command
            BinaryData newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything is OK the Repository Persisted the data and returned the entity
            Assert.IsType<BinaryData>(newBinaryData);
        }

        [Fact]
        public async void UpdateBinaryDataNoDataToUpdate()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.Right;


            //Mocking the repository to return null for every query
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to return a BinaryData on update statement
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).Returns(Task.FromResult(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object)));

            //Executing the command
            BinaryData newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            
            Assert.Null(newBinaryData);
        }

        [Fact]
        public async void UpdateBinaryDataInvalidBase64()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.Right;


            //Mocking the repository to return null for every query in order to create a valid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to get BinaryData to update
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult(binaryData));

            //Mocking the repository to return a BinaryData on update statement
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).Returns(Task.FromResult(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object)));

            //Executing the command
            BinaryData newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, "NotABase64");

            
            Assert.Null(newBinaryData);
        }


    }
}

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
    
    public class SettingBinaryDataTests
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

        public SettingBinaryDataTests()
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
            SetBinaryDataCommandHandler commandHandler = new SetBinaryDataCommandHandler(_binaryDataRepositoryMock.Object);
            return await commandHandler.ExecuteAsync(new SetBinaryDataCommand
            {
                ComparisonID = comparisonId,
                ComparisonSide = comparisonSide,
                Base64BinaryData = base64EncodedString
            });
        }

        [Fact]
        public async void SetNewBinaryDataNewIdAndNewSide()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.Left;


            //Mocking the repository to return null for every query
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns<Task<BinaryData>>(null);

            //Mocking the repository to return the new added entity
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).Returns(Task.FromResult(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(),_binaryDataRepositoryMock.Object)));

            //Executing the command
            BinaryData newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything is OK the Repository Persisted the data and returned the entity
            Assert.IsType<BinaryData>(newBinaryData);
        }

        [Fact]
        public async void SetNewBinaryDataExistingIdAndNewSide()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.Right;



            //Mocking the repository to return null for every query to create a vlid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns<Task<BinaryData>>(null);

            //Mocking the repository to return the new added entity
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).Returns(Task.FromResult(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object)));

            //Mocking to return the existing entity for left side and null for right side
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath)), comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Left)).Returns(Task.FromResult(binaryData));
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Right)).Returns<Task<BinaryData>>(null);

            //Executing the command

            BinaryData newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything is OK the Repository Persisted the data and returned the entity
            Assert.IsType<BinaryData>(newBinaryData);
        }


        [Fact]
        public async void SetBinaryDataAlreadyExists()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.Left;

            //Mocking the repository to return null for every query to create a vlid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns<Task<BinaryData>>(null);

            //Mocking to return a new entity for left side
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult(binaryData));

            //Executing the command
            BinaryData newBinaryData = await ExecuteCommand (comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything happens as exprected the entity would not be valid and the Repository will not persist it
            Assert.Null(newBinaryData);
        }

        [Fact]
        public async void SetBinaryDataWithInvalidBase64()
        {
            //Mocking the repository to return null for every query
            Mock<IBinaryDataRepository> binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
            binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns<Task<BinaryData>>(null);

            BinaryData newBinaryData = await ExecuteCommand(Guid.NewGuid(), SideEnum.Left, "NotABase64");

            Assert.Null(newBinaryData);
        }


    }
}

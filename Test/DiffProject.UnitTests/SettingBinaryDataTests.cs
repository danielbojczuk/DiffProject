using DiffProject.Application.CommandHandlers;
using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using Moq;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
        private string LoremIpsumOneBase64
        {
            get
            {
                return Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath));
            }
        }

        /// <summary>
        /// Repository mocking to be used in the tests
        /// </summary>
        Mock<IBinaryDataRepository> _binaryDataRepositoryMock;
        Mock<IComparisonResultRepository> _comparisonResultRepository;

        /// <summary>
        /// Notification context required for the CommandHandler
        /// </summary>
        NotificationContext _notificationContext;

        
        public SettingBinaryDataTests()
        {
            _LoremIpsumOnePath = Path.GetFullPath(@"..\..\..\..\Resources\LoremIpsum1.txt");
            _binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
            _comparisonResultRepository = new Mock<IComparisonResultRepository>();
            _notificationContext = new NotificationContext();
        }

        /// <summary>
        /// Method to execute the command to set the new BinaryData
        /// </summary>
        /// <param name="comparisonId">Id of the comparison</param>
        /// <param name="comparisonSide">Comparison's side</param>
        /// <param name="base64EncodedString"> Binary data Base64 encoded string to be compared</param>
        /// <returns></returns>
        private async Task<SetBinaryDataResponse> ExecuteCommand(Guid comparisonId, SideEnum comparisonSide, string base64EncodedString)
        {
            SetBinaryDataCommandHandler commandHandler = new SetBinaryDataCommandHandler(_binaryDataRepositoryMock.Object, _notificationContext, _comparisonResultRepository.Object);
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
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.left;


            //Mocking the repository to return null for every query
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to return the new added entity
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            //Executing the command
            SetBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything is OK the Repository Persisted the data and returned the entity
            Assert.IsType<SetBinaryDataResponse>(newBinaryData);
        }

        [Fact]
        public async void SetNewBinaryDataExistingIdAndNewSide()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;

            //Mocking the repository to return null for every query to create a vlid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to return the new added entity
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            //Mocking to return the existing entity for left side and null for right side
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, Convert.ToBase64String(File.ReadAllBytes(_LoremIpsumOnePath)), comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Left)).ReturnsAsync(binaryData);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Right)).Returns(Task.FromResult<BinaryData>(null));

            //Executing the command

            SetBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything is OK the Repository Persisted the data and returned the entity
            Assert.IsType<SetBinaryDataResponse>(newBinaryData);
        }


        [Fact]
        public async void SetBinaryDataAlreadyExists()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.left;

            //Mocking the repository to return null for every query to create a vlid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking to return a new entity for left side
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(comparisonId, ComparisonSideEnum.Left)).ReturnsAsync(binaryData);

            //Mocking the repository to return a new entity if the Add method was called (It should not be).
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            //Executing the command
            SetBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything happens as exprected the entity would not be valid and the Repository will not persist it
            Assert.Null(newBinaryData);
        }

        [Fact]
        public async void SetBinaryDataWithInvalidBase64()
        {
            //Mocking the repository to return null for every query
            Mock<IBinaryDataRepository> binaryDataRepositoryMock = new Mock<IBinaryDataRepository>();
            binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns<Task<BinaryData>>(null);

            //Mocking the repository to return a new entity if the Add method was called (It should not be).
            _binaryDataRepositoryMock.Setup(x => x.Add(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            SetBinaryDataResponse newBinaryData = await ExecuteCommand(Guid.NewGuid(), SideEnum.left, "NotABase64");

            Assert.Null(newBinaryData);
        }


    }
}

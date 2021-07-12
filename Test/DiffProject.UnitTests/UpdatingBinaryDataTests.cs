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

    public class UpdatingBinaryDataTests
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

        public UpdatingBinaryDataTests()
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
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;

            //Mocking the repository to return null for every query in order to create a valid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to get BinaryData to update
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).ReturnsAsync(binaryData);

            //Mocking the repository to return a BinaryData on update statement
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            //Executing the command
            UpdateBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);

            //If eveything is OK the Repository Persisted the data and returned the entity
            Assert.IsType<UpdateBinaryDataResponse>(newBinaryData);
        }

        [Fact]
        public async void UpdateBinaryDataNoDataToUpdate()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;

            //Mocking the repository to return null for every query in order to create a valid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to already have a Left Binary Data
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), ComparisonSideEnum.Left)).ReturnsAsync(binaryData);

            //Mocking the repository to return a BinaryData on update statement
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            //Executing the command
            UpdateBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, LoremIpsumOneBase64);


            Assert.Null(newBinaryData);
        }

        [Fact]
        public async void UpdateBinaryDataInvalidBase64()
        {
            //Data to be used in the test
            Guid comparisonId = Guid.NewGuid();
            SideEnum comparisonSide = SideEnum.right;
            string base64String = "NotABase64";


            //Mocking the repository to return null for every query in order to create a valid entity for the test
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult<BinaryData>(null));

            //Mocking the repository to get BinaryData to update
            BinaryData binaryData = new BinaryData(ComparisonSideEnum.Right, LoremIpsumOneBase64, comparisonId, _binaryDataRepositoryMock.Object);
            _binaryDataRepositoryMock.Setup(x => x.RetrieveDBinaryDataByComparisonIdAndSide(It.IsAny<Guid>(), It.IsAny<ComparisonSideEnum>())).Returns(Task.FromResult(binaryData));

            //Mocking the repository to return a BinaryData on update statement
            _binaryDataRepositoryMock.Setup(x => x.Update(It.IsAny<BinaryData>())).ReturnsAsync(new BinaryData(ComparisonSideEnum.Left, LoremIpsumOneBase64, Guid.NewGuid(), _binaryDataRepositoryMock.Object));

            //Executing the command
            UpdateBinaryDataResponse newBinaryData = await ExecuteCommand(comparisonId, comparisonSide, base64String);


            Assert.Null(newBinaryData);
        }


    }
}

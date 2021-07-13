using System;
using System.Threading;
using System.Threading.Tasks;
using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using MediatR;

namespace DiffProject.Application.CommandHandlers
{
    /// <summary>
    /// It Handles the Command <see cref="SetBinaryDataCommand"/> to set a new Binary Data to be compared.
    /// </summary>
    public class SetBinaryDataCommandHandler : AbstractCommandHandler, IRequestHandler<SetBinaryDataCommand, SetBinaryDataResponse>
    {
        private IBinaryDataRepository _binaryDataRepository;

        private IComparisonResultRepository _comparisonResultRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetBinaryDataCommandHandler"/> class.
        /// </summary>
        /// <param name="binaryDataRepository">An instance of <see cref="IBinaryDataRepository"/> implementation.</param>
        /// <param name="comparisonResultRepository">An instance of <see cref="IComparisonResultRepository"/> implementation.</param>
        /// <param name="notificationContext">An instance of <see cref="INotificationContext"/> implementation.</param>

        public SetBinaryDataCommandHandler(IBinaryDataRepository binaryDataRepository, IComparisonResultRepository comparisonResultRepository, INotificationContext notificationContext) 
            : base(notificationContext)
        {
            _binaryDataRepository = binaryDataRepository;
            _comparisonResultRepository = comparisonResultRepository;
        }

        /// <summary>
        /// Handle the command <see cref="SetBinaryDataCommand"/>.
        /// It will set a new Binary Data to be compared and ask to start a new comparison.
        /// </summary>
        /// <param name="command">An instance of <see cref="SetBinaryDataCommand"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>If it is provided a valid Binary Data to be compered it will return an instance of <see cref="SetBinaryDataResponse"/>. If the Binary Data is invalid it will return <see cref="null"/>.</returns>
        public async Task<SetBinaryDataResponse> Handle(SetBinaryDataCommand command, CancellationToken cancellationToken)
        {
            BinaryData binaryData = new BinaryData(ConvertCommandEnumToEntityEnum(command.ComparisonSide), command.Base64BinaryData, command.ComparisonID, _binaryDataRepository);
            if (!binaryData.ValidationResult.IsValid)
            {
                NotificationContext.AddNotifications(binaryData.ValidationResult);
                return null;
            }

            BinaryData newBinaryData = await _binaryDataRepository.Add(binaryData);

            StartComparison(newBinaryData.ComparisonId, cancellationToken);

            return new SetBinaryDataResponse { Id = newBinaryData.Id, Base64BinaryData = newBinaryData.Base64BinaryData, ComparisonId = newBinaryData.ComparisonId, ComparisonSide = ConvertEntityEnumToCommandEnum(binaryData.ComparisonSide).ToString() };
        }

        private static SideEnum ConvertEntityEnumToCommandEnum(ComparisonSideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (SideEnum)integerEnumValue;
        }

        private static ComparisonSideEnum ConvertCommandEnumToEntityEnum(SideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (ComparisonSideEnum)integerEnumValue;
        }

        private void StartComparison(Guid comparisonId, CancellationToken cancellationToken)
        {
            CompareBinaryDataCommandHandler calculationCommandHandler = new CompareBinaryDataCommandHandler(_binaryDataRepository, new NotificationContext(), _comparisonResultRepository);
            calculationCommandHandler.Handle(new CompareBinaryDataCommand { ComparisonIs = comparisonId }, cancellationToken).GetAwaiter();
        }
    }
}

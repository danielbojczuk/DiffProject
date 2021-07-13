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
    /// It Handles the Command <see cref="UpdateBinaryDataCommand"/> to calculate de differences between the Binary Data.
    /// </summary>
    public class UpdateBinaryDataCommandHandler : AbstractCommandHandler, IRequestHandler<UpdateBinaryDataCommand, UpdateBinaryDataResponse>
    {
        private IBinaryDataRepository _binaryDataRepository;

        private IComparisonResultRepository _comparisonResultRepository;

        public UpdateBinaryDataCommandHandler(IBinaryDataRepository binaryDataRepository, INotificationContext notificationContext, IComparisonResultRepository comparisonResultRepository) : base(notificationContext)
        {
            _binaryDataRepository = binaryDataRepository;
            _comparisonResultRepository = comparisonResultRepository;
        }

        /// <summary>
        /// Handle the command <see cref="UpdateBinaryDataCommand"/>.
        /// It will update the binary data to be compared
        /// </summary>
        /// <param name="command">An instance of <see cref="UpdateBinaryDataCommand"/> implementation.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/></param>
        /// <returns>If a valid Binary Data was provided it will return an instance of <see cref="UpdateBinaryDataResponse"/>. Otherwise it will return <see cref="null"/>.</returns>
        public async Task<UpdateBinaryDataResponse> Handle(UpdateBinaryDataCommand command, CancellationToken cancellationToken)
        {
            BinaryData binaryData = await _binaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(command.CurrentComparisonID, ConvertCommandEnumToEntityEnum(command.CurrentComparisonSide));
            if (binaryData == null)
            {
                NotificationContext.AddNotification("Comparison Id not found");
                return null;
            }

            binaryData.UpdateBase64BinaryFile(command.NewBase64BinaryData);
            if (!binaryData.ValidationResult.IsValid)
            {
                NotificationContext.AddNotifications(binaryData.ValidationResult);
                return null;
            }

            BinaryData updatedBinaryData = await _binaryDataRepository.Update(binaryData);

            StartComparison(updatedBinaryData.ComparisonId, cancellationToken);

            return new UpdateBinaryDataResponse
            {
                Id = updatedBinaryData.Id,
                ComparisonId = updatedBinaryData.ComparisonId,
                Base64BinaryData = updatedBinaryData.Base64BinaryData,
                ComparisonSide = ConvertEntityEnumToCommandEnum(updatedBinaryData.ComparisonSide).ToString(),
            };
        }

        private void StartComparison(Guid comparisonId, CancellationToken cancellationToken)
        {
            CompareBinaryDataCommandHandler calculationCommandHandler = new CompareBinaryDataCommandHandler(_binaryDataRepository, new NotificationContext(), _comparisonResultRepository);
            calculationCommandHandler.Handle(new CompareBinaryDataCommand { ComparisonIs = comparisonId }, cancellationToken).GetAwaiter();
        }

        private ComparisonSideEnum ConvertCommandEnumToEntityEnum(SideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (ComparisonSideEnum)integerEnumValue;
        }

        private SideEnum ConvertEntityEnumToCommandEnum(ComparisonSideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (SideEnum)integerEnumValue;
        }
    }
}

using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class UpdateBinaryDataCommandHandler : AbstractCommandHandler<UpdateBinaryDataCommand, UpdateBinaryDataResponse>, IRequestHandler<UpdateBinaryDataCommand, UpdateBinaryDataResponse>
    {
        public IBinaryDataRepository BinaryDataRepository { get; private set; }
        public IComparisonResultRepository ComparisonResultRepository { get; private set; }

        public UpdateBinaryDataCommandHandler(IBinaryDataRepository binaryDataRepository, INotificationContext notificationContext, IComparisonResultRepository comparisonResultRepository) : base(notificationContext)
        {
            BinaryDataRepository = binaryDataRepository;
            ComparisonResultRepository = comparisonResultRepository;
        }

        ///<summary>
        ///Execute Async the 'Update Data' Command
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<UpdateBinaryDataResponse> Handle(UpdateBinaryDataCommand command, CancellationToken cancellationToken)
        {
            BinaryData binaryData = await BinaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(command.CurrentComparisonID, ConvertCommandEnumToEntityEnum(command.CurrentComparisonSide));
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

            BinaryData updatedBinaryData = await BinaryDataRepository.Update(binaryData);

            StartComparingSides(updatedBinaryData.ComparisonId, cancellationToken);

            return new UpdateBinaryDataResponse
            {
                Id = updatedBinaryData.Id,
                ComparisonId = updatedBinaryData.ComparisonId,
                Base64BinaryData = updatedBinaryData.Base64BinaryData,
                ComparisonSide = ConvertEntityEnumToCommandEnum(updatedBinaryData.ComparisonSide)
            };
        }

        private void StartComparingSides(Guid comparisonId, CancellationToken cancellationToken)
        {
            CalculationCommandHandler calculationCommandHandler = new CalculationCommandHandler(BinaryDataRepository, new NotificationContext(), ComparisonResultRepository);
            calculationCommandHandler.Handle(new CalculationCommand { ComparisonID = comparisonId }, cancellationToken).GetAwaiter();
        }

        /// <summary>
        /// Method to convert the Application Enum to the Domain Enum
        /// </summary>
        /// <param name="commandEnum">The application Enum</param>
        /// <returns></returns>
        private ComparisonSideEnum ConvertCommandEnumToEntityEnum(SideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (ComparisonSideEnum)integerEnumValue;
        }

        /// <summary>
        /// Method to convert the  Domain Enum to the Application Enum
        /// </summary>
        /// <param name="commandEnum">The Entity Side Enum</param>
        /// <returns></returns>
        private SideEnum ConvertEntityEnumToCommandEnum(ComparisonSideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (SideEnum)integerEnumValue;
        }
    }
}

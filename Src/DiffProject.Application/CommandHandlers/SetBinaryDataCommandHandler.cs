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
    public class SetBinaryDataCommandHandler : AbstractCommandHandler<SetBinaryDataCommand, SetBinaryDataResponse>, IRequestHandler<SetBinaryDataCommand, SetBinaryDataResponse>
    {
        public IBinaryDataRepository BinaryDataRepository { get; private set; }
        public IComparisonResultRepository ComparisonResultRepository { get; private set; }

        public SetBinaryDataCommandHandler(IBinaryDataRepository binaryDataRepository, INotificationContext notificationContext, IComparisonResultRepository comparisonResultRepository) : base(notificationContext)
        {
            BinaryDataRepository = binaryDataRepository;
            ComparisonResultRepository = comparisonResultRepository;
        }

        ///<summary>
        ///Creates a new Binary Data and sart the comparing.
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<SetBinaryDataResponse> Handle(SetBinaryDataCommand command, CancellationToken cancellationToken)
        {
            BinaryData binaryData = new BinaryData(ConvertCommandEnumToEntityEnum(command.ComparisonSide), command.Base64BinaryData, command.ComparisonID, BinaryDataRepository);
            if (!binaryData.ValidationResult.IsValid)
            {
                NotificationContext.AddNotifications(binaryData.ValidationResult);
                return null;
            }

            BinaryData newBinaryData = await BinaryDataRepository.Add(binaryData);

            StartComparison(newBinaryData.ComparisonId, cancellationToken);

            return new SetBinaryDataResponse { Id = newBinaryData.Id, Base64BinaryData = newBinaryData.Base64BinaryData, ComparisonId = newBinaryData.ComparisonId, ComparisonSide = ConvertEntityEnumToCommandEnum(binaryData.ComparisonSide).ToString() };

        }


        private void StartComparison(Guid comparisonId, CancellationToken cancellationToken)
        {
            CalculationCommandHandler calculationCommandHandler = new CalculationCommandHandler(BinaryDataRepository, new NotificationContext(), ComparisonResultRepository);
            calculationCommandHandler.Handle(new ComparisonResultCommand { ComparisonID = comparisonId }, cancellationToken).GetAwaiter();
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

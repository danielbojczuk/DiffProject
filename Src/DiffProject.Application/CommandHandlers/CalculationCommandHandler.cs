using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class CalculationCommandHandler : AbstractCommandHandler<CalculationCommand, CalculationResponse>
    {
        public IComparisonResultRepository ComparisonResultRepository { get; private set; }
        public IBinaryDataRepository BinaryDataRepository { get; private set; }

        public CalculationCommandHandler(IBinaryDataRepository binaryDataRepository, INotificationContext notificationContext, IComparisonResultRepository comparisonDataRepository) : base(notificationContext)
        {
            ComparisonResultRepository = comparisonDataRepository;
            BinaryDataRepository = binaryDataRepository;
        }

        ///<summary>
        ///Execute Async the Calculation Command
        ///Using the comparison ID It will get the BinaryData previously recorded and calculate de diferences.
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<CalculationResponse> Handle(CalculationCommand command, CancellationToken cancellationToken)
        {
            List<BinaryData> binaryDataList = await BinaryDataRepository.RetrieveDBinaryDataByComparisonId(command.ComparisonID);
            if (binaryDataList == null || binaryDataList.Count == 0)
            {
                NotificationContext.AddNotification("Comparison Id not found");
                return null;
            }

            ComparisonResult comparisonResult = new ComparisonResult(command.ComparisonID);
            comparisonResult.AddDataToCompare(binaryDataList);
            if (!comparisonResult.ValidationResult.IsValid)
            {
                NotificationContext.AddNotifications(comparisonResult.ValidationResult);
                return null;
            }

            comparisonResult.Compare();
            if (!comparisonResult.ValidationResult.IsValid)
            {
                NotificationContext.AddNotifications(comparisonResult.ValidationResult);
                return null;
            }

            await ComparisonResultRepository.Add(comparisonResult);

            Dictionary<long, long> diferences = new Dictionary<long, long>();
            comparisonResult.Differences.ForEach(x => diferences.Add(x.Position, x.Size));

            return new CalculationResponse
            {
                ComparisonId = comparisonResult.ComparisonId,
                Differences = diferences,
                SameSize = comparisonResult.SameSize,
                SidesEqual = comparisonResult.SidesEqual
            };
        }

    }
}

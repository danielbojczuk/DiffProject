using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;

namespace DiffProject.Application.CommandHandlers
{
    /// <summary>
    /// It Handles the Command <see cref="CompareBinaryDataCommand"/> to calculate de differences between the Binary Data sets.
    /// </summary>
    public class CompareBinaryDataCommandHandler : AbstractCommandHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompareBinaryDataCommandHandler"/> class.
        /// </summary>
        /// <param name="binaryDataRepository">An instance of <see cref="IBinaryDataRepository"/> implementation.</param>
        /// <param name="notificationContext">An instance of <see cref="INotificationContext"/> implementation.</param>
        /// <param name="comparisonResultRepository">An instance of <see cref="IComparisonResultRepository"/> implementation.</param>
        public CompareBinaryDataCommandHandler(IBinaryDataRepository binaryDataRepository, INotificationContext notificationContext, IComparisonResultRepository comparisonResultRepository)
            : base(notificationContext)
        {
            ComparisonResultRepository = comparisonResultRepository;
            BinaryDataRepository = binaryDataRepository;
        }

        /// <summary>
        /// Gets the <see cref="IComparisonResultRepository"/>.
        /// </summary>
        public IComparisonResultRepository ComparisonResultRepository { get; private set; }

        /// <summary>
        /// Gets the <see cref="IBinaryDataRepository"/>.
        /// </summary>
        public IBinaryDataRepository BinaryDataRepository { get; private set; }

        /// <summary>
        /// Handle the command <see cref="CompareBinaryDataCommand"/>.
        /// It will get the binary data, create a new result entity, calculate de differences and return them.
        /// </summary>
        /// <param name="command">An instance of <see cref="IBinaryDataRepository"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/></param>
        /// <returns>If there is a difference comparison calculated it will return an instance of <see cref="ComparisonResultResponse"/>. Otherwise it will return <see cref="null"/>.</returns>
        public async Task<ComparisonResultResponse> Handle(CompareBinaryDataCommand command, CancellationToken cancellationToken)
        {
            List<BinaryData> binaryDataList = await BinaryDataRepository.RetrieveDBinaryDataByComparisonId(command.ComparisonIs);
            if (binaryDataList.Count == 0)
            {
                NotificationContext.AddNotification("Comparison Id not found");
                return null;
            }

            ComparisonResult comparisonResult = new ComparisonResult(command.ComparisonIs);
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

            return CreateComparisonResultResponse(comparisonResult);
        }

        private ComparisonResultResponse CreateComparisonResultResponse(ComparisonResult comparisonResult)
        {
            Dictionary<long, long> diferences = new Dictionary<long, long>();
            comparisonResult.Differences.ForEach(x => diferences.Add(x.Position, x.Size));

            return new ComparisonResultResponse
            {
                ComparisonId = comparisonResult.ComparisonId,
                Differences = diferences,
                SameSize = (bool)comparisonResult.SameSize,
                SidesEqual = (bool)comparisonResult.SidesEqual,
            };
        }

    }
}

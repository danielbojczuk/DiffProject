using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using MediatR;

namespace DiffProject.Application.CommandHandlers
{
    /// <summary>
    /// It Handles the Command <see cref="GetComparisonResultCommand"/> to calculate de differences between the Binary Data set.
    /// </summary>
    public class GetCalculationResultCommandHandler : AbstractCommandHandler, IRequestHandler<GetComparisonResultCommand, ComparisonResultResponse>
    {
        private readonly IComparisonResultRepository _comparisonResultRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCalculationResultCommandHandler"/> class.
        /// </summary>
        /// <param name="notificationContext">An instance of <see cref="INotificationContext"/> instance.</param>
        /// <param name="comparisonResultRepository">An instance of <see cref="IComparisonResultRepository"/> instance.</param>
        public GetCalculationResultCommandHandler(INotificationContext notificationContext, IComparisonResultRepository comparisonResultRepository) 
            : base(notificationContext)
        {
            _comparisonResultRepository = comparisonResultRepository;
        }

        /// <summary>
        /// Handle the command <see cref="GetComparisonResultCommand"/>.
        /// It will get the binary data, create a new result entity, calculate de differences and return them.
        /// </summary>
        /// <param name="command">An instance of <see cref="GetComparisonResultCommand"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/></param>
        /// <returns>If there is a difference comparison calculated it will return an instance of <see cref="ComparisonResultResponse"/>. Otherwise it will return <see cref="null"/>.</returns>
        public async Task<ComparisonResultResponse> Handle(GetComparisonResultCommand command, CancellationToken cancellationToken)
        {
            ComparisonResult comparisonResult = await _comparisonResultRepository.RetrieveResultByComparisonId(command.ComparisonID);

            if (comparisonResult == null)
            {
                return null;
            }

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

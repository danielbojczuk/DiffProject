using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class GetCalculationResultCommandHandler : AbstractCommandHandler<GetComparisonResultCommand, ComparisonResultResponse>, IRequestHandler<GetComparisonResultCommand, ComparisonResultResponse>
    {
        public IComparisonResultRepository ComparisonResultRepository { get; private set; }

        public GetCalculationResultCommandHandler(INotificationContext notificationContext, IComparisonResultRepository comparisonResultRepository) : base(notificationContext)
        {
            ComparisonResultRepository = comparisonResultRepository;
        }

        ///<summary>
        ///Execute Async the 'Update Data' Command
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<ComparisonResultResponse> Handle(GetComparisonResultCommand command, CancellationToken cancellationToken)
        {
            ComparisonResult comparisonResult = await ComparisonResultRepository.RetrieveResultByComparisonId(command.ComparisonID);
            
            if (comparisonResult == null)
                return null;

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
                SameSize = comparisonResult.SameSize,
                SidesEqual = comparisonResult.SidesEqual
            };
        }
    }
}

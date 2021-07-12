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
    /// It handles the command <see cref="GetBinaryDataCommand"/> to return a already set Binary Data
    /// </summary>
    public class GetBinaryDataCommandHandler : AbstractCommandHandler, IRequestHandler<GetBinaryDataCommand, GetBinaryDataResponse>
    {
 
        private readonly IBinaryDataRepository _binaryDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBinaryDataCommandHandler"/> class.
        /// </summary>
        /// <param name="notificationContext">An instance of <see cref="INotificationContext"/>.</param>
        /// <param name="binaryDataRepository">An instance of <see cref="IBinaryDataRepository"/> implementation.</param>
        public GetBinaryDataCommandHandler(INotificationContext notificationContext, IBinaryDataRepository binaryDataRepository) 
            : base(notificationContext)
        {
            _binaryDataRepository = binaryDataRepository;
        }

         /// <summary>
        /// Handle the command <see cref="GetBinaryDataCommand"/>.
        /// It will get return the bynary data that has been already set.
        /// </summary>
        /// <param name="command">An instance of <see cref="GetBinaryDataCommand"/>.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/></param>
        /// <returns>If ther is a Binary Data set as in <see cref="GetBinaryDataCommand"/> it will return an instance of <see cref="GetBinaryDataResponse"/>. Otherwhise it will return <see cref="null"/>.</returns>
        public async Task<GetBinaryDataResponse> Handle(GetBinaryDataCommand command, CancellationToken cancellationToken)
        {
            BinaryData binaryData = await _binaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(command.ComparisonId, ConvertCommandEnumToEntityEnum(command.ComparisonSide));

            if (binaryData == null)
            {
                return null;
            }

            return new GetBinaryDataResponse { Id = binaryData.Id, Base64BinaryData = binaryData.Base64BinaryData, ComparisonId = binaryData.ComparisonId, ComparisonSide = ConvertEntityEnumToCommandEnum(binaryData.ComparisonSide).ToString() };
        }

        private static ComparisonSideEnum ConvertCommandEnumToEntityEnum(SideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (ComparisonSideEnum)integerEnumValue;
        }

        private static SideEnum ConvertEntityEnumToCommandEnum(ComparisonSideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (SideEnum)integerEnumValue;
        }

    }
}

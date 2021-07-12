﻿using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class GetBinaryDataCommandHandler : AbstractCommandHandler<GetBinaryDataCommand, GetBinaryDataResponse>, IRequestHandler<GetBinaryDataCommand, GetBinaryDataResponse>
    {
        public IBinaryDataRepository BinaryDataRepository { get; private set; }

        public GetBinaryDataCommandHandler(INotificationContext notificationContext, IBinaryDataRepository binaryDataRepository) : base(notificationContext)
        {
            BinaryDataRepository = binaryDataRepository;
        }

        ///<summary>
        ///Execute Async the 'Update Data' Command
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<GetBinaryDataResponse> Handle(GetBinaryDataCommand command, CancellationToken cancellationToken)
        {
            BinaryData binaryData = await BinaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(command.ComparisonID, ConvertCommandEnumToEntityEnum(command.ComparisonSide));

            if (binaryData == null)
                return null;

            return new GetBinaryDataResponse { Id = binaryData.Id, Base64BinaryData = binaryData.Base64BinaryData, ComparisonId = binaryData.ComparisonId, ComparisonSide = ConvertEntityEnumToCommandEnum(binaryData.ComparisonSide).ToString() };
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
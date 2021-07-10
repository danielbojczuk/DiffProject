﻿using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class SetBinaryDataCommandHandler : AbstractCommandHandler<BinaryData, SetBinaryDataCommand>
    {
        public IBinaryDataRepository BinaryDataRepository { get; private set; }

        public SetBinaryDataCommandHandler(IBinaryDataRepository binaryDataRepository)
        {
            BinaryDataRepository = binaryDataRepository;
        }

        ///<summary>
        ///Execute Async the 'Set Data' Command
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<BinaryData> ExecuteAsync(SetBinaryDataCommand command)
        {
            BinaryData binaryData = new BinaryData(ConvertCommandEnumToEntityEnum(command.ComparisonSide), command.Base64BinaryData, command.ComparisonID, BinaryDataRepository);
            if (!binaryData.ValidationResult.IsValid)
                return null;

            return await BinaryDataRepository.Add(binaryData);

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

    }
}

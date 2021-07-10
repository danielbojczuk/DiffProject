using System;
using System.Threading.Tasks;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class UpdateBinaryDataCommandHandler : AbstractCommandHandler<BinaryData, UpdateBinaryDataCommand>
    {
        public IBinaryDataRepository BinaryDataRepository { get; private set; }

        public UpdateBinaryDataCommandHandler(IBinaryDataRepository binaryDataRepository)
        {
            BinaryDataRepository = binaryDataRepository;
        }

        ///<summary>
        ///Execute Async the 'Update Data' Command
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<BinaryData> ExecuteAsync(UpdateBinaryDataCommand command)
        {
            BinaryData binaryData = await BinaryDataRepository.RetrieveDBinaryDataByComparisonIdAndSide(command.CurrentComparisonID, ConvertCommandEnumToEntityEnum(command.CurrentComparisonSide));
            
            if (binaryData == null)
                return null;

            binaryData.UpdateBase64BinaryFile(command.NewBase64BinaryData);
            if (!binaryData.ValidationResult.IsValid)
                return null;

            return await BinaryDataRepository.Update(binaryData);
            
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

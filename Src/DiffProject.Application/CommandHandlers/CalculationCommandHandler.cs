using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class CalculationCommandHandler : AbstractCommandHandler<CalculationResponse, CalculationCommand>
    {
        public IBinaryDataRepository BinaryDataRepository { get; private set; }

        public CalculationCommandHandler(IBinaryDataRepository binaryDataRepository)
        {
            BinaryDataRepository = binaryDataRepository;
        }

        ///<summary>
        ///Execute Async the Calculation Command
        ///Using the comparison ID It will get the BinaryData previously recorded and calculate de diferences.
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public override async Task<CalculationResponse> ExecuteAsync(CalculationCommand command)
        {
            List<BinaryData> binaryDataList = await BinaryDataRepository.RetrieveDBinaryDataByComparisonId(command.ComparisonID);
            if (binaryDataList == null || binaryDataList.Count == 0)
                return null;

            ComparisonResult comparisonResult = new ComparisonResult(command.ComparisonID);
            comparisonResult.AddDataToCompare(binaryDataList);
            if (!comparisonResult.ValidationResult.IsValid)
                return null;

            comparisonResult.Compare();
            if (!comparisonResult.ValidationResult.IsValid)
                return null;

            return new CalculationResponse
            {
                ComparisonId = comparisonResult.ComparisonId,
                Differences = comparisonResult.Differences,
                SameSize = comparisonResult.SameSize,
                SidesEqual = comparisonResult.SidesEqual
            };
        }

    }
}

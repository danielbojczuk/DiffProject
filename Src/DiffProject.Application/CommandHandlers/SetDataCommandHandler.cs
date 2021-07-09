using System;
using System.Threading.Tasks;
using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
using DiffProject.Domain.AggregateModels.ComparisonAggregate;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public class SetDataCommandHandler
    {
        /// <summary>
        /// Property with the DiffComparisonRepository to be used with the DiffComparison Entity
        /// </summary>
        private IDiffComparisonRepository _diffComparisonRepository;

        public SetDataCommandHandler(IDiffComparisonRepository diffComparisonRepository)
        {
            _diffComparisonRepository = diffComparisonRepository;
        }

        ///<summary>
        ///Execute Async the 'Set Data' Command
        ///</summary>
        ///<param name="command">Command to be handled with the Comparison Id and the Bas64 Binary Data</param>
        public async Task<Guid?> ExecuteAsync(SetDataCommand command)
        {
            DiffComparison diffComparision = new DiffComparison(command.ComparisonID, _diffComparisonRepository);
            if (!diffComparision.ValidationResult.IsValid)
                return null;

            BinaryData binaryData = new BinaryData(ConvertCommandEnumToEntityEnum(command.ComparisonSide), command.Base64BinaryData);
            if (!diffComparision.ValidationResult.IsValid)
                return null;

            diffComparision.AddBinaryData(binaryData);

            _diffComparisonRepository.Add(diffComparision);

            return diffComparision.Id;
            
        }

        private Domain.AggregateModels.ComparisonAggregate.Enums.ComparisonSideEnum ConvertCommandEnumToEntityEnum(ComparisonSideEnum commandEnum)
        {
            int integerEnumValue = (int)commandEnum;
            return (Domain.AggregateModels.ComparisonAggregate.Enums.ComparisonSideEnum)integerEnumValue;
        }
    }
}

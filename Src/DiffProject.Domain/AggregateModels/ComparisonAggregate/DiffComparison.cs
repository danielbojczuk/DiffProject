using System;
using System.Collections.Generic;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Validators;
using DiffProject.Domain.AggregateModels.SeedWork;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate
{
    ///<summary>
    ///Aggregate Root of the context. Should be the only entity in the Comparison context to be used by others contexts or layers.
    ///It is responsible to check de differences.
    ///</summary>
    public class DiffComparison: Entity
    {
        /// <summary>
        /// A set of the binary data to be compared
        /// </summary>
        public List<BinaryData> BinaryData { get; private set; }

        /// <summary>
        /// Result of the difference between the to sides.
        /// </summary>
        public object Result { get; private set; }

        /// <summary>
        /// DiffComparison Repository property to be used in the duplicity validation
        /// </summary>
        private IDiffComparisonRepository _diffComparisonRepository;

        public DiffComparison(Guid id, IDiffComparisonRepository diffComparisonRepository) :base(id)
        {
            _diffComparisonRepository = diffComparisonRepository;
            Validate(this, new DiffComparisonDuplicityValidator(diffComparisonRepository));
        }

        /// <summary>
        /// Add Binary data to be validated.
        /// </summary>
        /// <param name="data">Data to be validated</param>
        public void AddBinaryData(BinaryData data)
        {
            BinaryData.Add(data);
            Validate(this, new DiffComparisonValidator());
        }
    }
}

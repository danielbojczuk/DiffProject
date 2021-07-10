using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces
{
    /// <summary>
    /// Repository to the DiffComparison Entity
    /// </summary>
    public interface IBinaryDataRepository
    {

        /// <summary>
        /// Persist the new BinaryData entity
        /// </summary>
        /// <param name="diffComparison">DiffComparisonEntity</param>
        Task<BinaryData> Add(BinaryData binaryData);

        /// <summary>
        /// Persist the updates in the BinaryData entity
        /// </summary>
        /// <param name="diffComparison">DiffComparisonEntity</param>
        Task<BinaryData> Update(BinaryData binaryData);

        /// <summary>
        /// Retrieve the Binary Data By Comparison ID and Side
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BinaryData> RetrieveDBinaryDataByComparisonIdAndSide(Guid id, ComparisonSideEnum side);

    }
}

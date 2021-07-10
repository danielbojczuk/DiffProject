using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Retrieve the Active Binary Data By Comparison ID and Side
        /// </summary>
        /// <param name="comparisonid"></param>
        /// <returns></returns>
        Task<BinaryData> RetrieveDBinaryDataByComparisonIdAndSide(Guid comparisonid, ComparisonSideEnum side);

        /// <summary>
        /// Retrieve the Active Binary Data By Comparison ID and Side
        /// </summary>
        /// <param name="comparisonid"></param>
        /// <returns></returns>
        Task<List<BinaryData>> RetrieveDBinaryDataByComparisonId(Guid comparisonid);

    }
}

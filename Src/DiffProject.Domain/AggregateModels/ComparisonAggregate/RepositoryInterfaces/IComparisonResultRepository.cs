using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces
{
    /// <summary>
    /// Repository to the DiffComparison Entity
    /// </summary>
    public interface IComparisonResultRepository
    {

        /// <summary>
        /// Persist the new IComparisonResult entity
        /// </summary>
        /// <param name="diffComparison">DiffComparisonEntity</param>
        Task<ComparisonResult> Add(ComparisonResult comparisonResult);

        /// <summary>
        /// Retrieve the Active Binary Data By Comparison ID and Side
        /// </summary>
        /// <param name="comparisonid"></param>
        /// <returns></returns>
        Task<ComparisonResult> RetrieveResultByComparisonId(Guid comparisonid);

    }
}

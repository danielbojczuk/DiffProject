using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces
{
    /// <summary>
    /// Repository to the DiffComparison Entity
    /// </summary>
    public interface IDiffComparisonRepository
    {
        /// <summary>
        /// Retrieves the DiffComparison entity
        /// </summary>
        /// <param name="id">Id of DiffComparison entity</param>
        /// <returns></returns>
        DiffComparison RetrieveDiffComparisonById(Guid id);

        void Add(DiffComparison diffComparison);
    }
}

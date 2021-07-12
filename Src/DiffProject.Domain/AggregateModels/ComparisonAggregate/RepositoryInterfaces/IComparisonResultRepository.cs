using System;
using System.Threading.Tasks;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces
{
    /// <summary>
    /// Comparison Result's Repository
    /// </summary>
    public interface IComparisonResultRepository
    {

        /// <summary>
        /// Persist a new Comparison Result only if the entity is valid.
        /// </summary>
        /// <param name="comparisonResult">Instance of <see cref="ComparisonResult"/>.</param>
        /// <returns>If it is a Valid Entity it returns the persisted Comparison Result. Otherwhise trhows an <see cref="InvalidOperationException"/>.</returns>
        Task<ComparisonResult> Add(ComparisonResult comparisonResult);

        /// <summary>
        /// Retrieves a Comparison Result by Comparison Id and Side.
        /// </summary>
        /// <param name="comparisonid">The Comparison Id. Instance of a <see cref="Guid"/>.</param>
        /// <returns>A Comparison Result whith the given Comparison Id, or null if there isn't any.</returns>
        Task<ComparisonResult> RetrieveResultByComparisonId(Guid comparisonid);

    }
}

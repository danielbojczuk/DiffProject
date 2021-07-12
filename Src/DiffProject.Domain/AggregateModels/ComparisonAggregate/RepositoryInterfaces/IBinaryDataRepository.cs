using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.Enums;

namespace DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces
{
    /// <summary>
    /// Bynary Data's Repository Interface.
    /// </summary>
    public interface IBinaryDataRepository
    {
        /// <summary>
        /// Persist a new Binary Data only if the entity is valid.
        /// </summary>
        /// <param name="binaryData">Instance of <see cref="BinaryData"/>.</param>
        /// <returns>If it is a Valid Entity it returns the persisted Binary Data. Otherwhise trhows an <see cref="InvalidOperationException"/>.</returns>
        Task<BinaryData> Add(BinaryData binaryData);

        /// <summary>
        /// Persist the chenges in aBinary Data only if the entity is valid.
        /// </summary>
        /// <param name="binaryData">Instance of <see cref="BinaryData"/>.</param>
        /// <returns>If it is a Valid Entity it returns the persisted Binary Data. Otherwhise trhows an <see cref="InvalidOperationException"/>.</returns>
        Task<BinaryData> Update(BinaryData binaryData);

        /// <summary>
        /// Retrieves a Binary Data by Comparison Id and Side.
        /// </summary>
        /// <param name="comparisonid">The Comparison Id. Instance of a <see cref="Guid"/>.</param>
        /// <param name="side">The Comparison side. <see cref="ComparisonSideEnum"/>enum data.</param>
        /// <returns>A Bynary Data whith the given Comparison Id and Side, or null if there isn't any.</returns>
        Task<BinaryData> RetrieveDBinaryDataByComparisonIdAndSide(Guid comparisonid, ComparisonSideEnum side);

        /// <summary>
        /// Retrieves a Binary Data by Comparison Id.
        /// </summary>
        /// <<param name="comparisonid">The Comparison Id. Instance of a <see cref="Guid"/>.</param>
        /// <returns>A list with the Bynary Data whith the given Comparison Id.</returns>
        Task<List<BinaryData>> RetrieveDBinaryDataByComparisonId(Guid comparisonid);

    }
}

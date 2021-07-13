using System;
using System.Collections.Generic;

namespace DiffProject.Application.Responses
{
    /// <summary>
    /// The response of <see cref="CompareBinaryDataCommand"/>.
    /// </summary>
    public class ComparisonResultResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the both comparison sides are equal or not.
        /// </summary>
        public bool SidesEqual { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the both comparison sides have the same sizes or not.
        /// </summary>
        public bool SameSize { get; set; }

        /// <summary>
        /// Gets or sets the differences between the sides. It should be set only if both sides have the same sizes.
        /// </summary>
        /// <typeparam name="long">The fist type parameter is the offset</typeparam>
        /// <typeparam name="long">The second type parameter is the lenght</typeparam>
        public Dictionary<long, long> Differences { get; set; }

        /// <summary>
        /// Gets or sets the Comparison ID used in both sides and result.
        /// </summary>
        public Guid ComparisonId { get; set; }

    }
}

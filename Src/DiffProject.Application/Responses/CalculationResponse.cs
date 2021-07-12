using System;
using System.Collections.Generic;

namespace DiffProject.Application.Responses
{
    public class ComparisonResultResponse
    {
        /// <summary>
        /// It will be true only if both sides are totaly equal. It will be null if it has not been calculated yet.
        /// </summary>
        public bool? SidesEqual { get; set; }

        /// <summary>
        /// It will be true only if both sides are totaly equal. It will be null if it has not been calculated yet.
        /// </summary>
        public bool? SameSize { get; set; }

        /// <summary>
        /// It will have the differences between the sides if they have the same size.
        /// </summary>
        /// <typeparam name="long">The fist type parameter is the offset</typeparam>
        /// <typeparam name="long">The second type parameter is the lenght</typeparam>
        public Dictionary<long, long> Differences { get; set; }

        /// <summary>
        /// ComparisonId used in both sides and result.
        /// </summary>
        public Guid ComparisonId { get; set; }

    }
}

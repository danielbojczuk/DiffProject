using System;

namespace DiffProject.Application.Responses
{
    public class GetBinaryDataResponse
    {
        /// <summary>
        /// Side de data should be on comparison
        /// </summary>
        public string ComparisonSide { get; set; }

        /// <summary>
        /// Base64 encoded binary Data
        /// </summary>
        public string Base64BinaryData { get; set; }

        /// <summary>
        /// ComparisonId used in both sides and result.
        /// </summary>
        public Guid ComparisonId { get; set; }

        /// <summary>
        /// Binary Data Created ID
        /// </summary>
        public Guid Id { get; set; }
    }
}

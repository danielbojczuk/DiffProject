using System;

namespace DiffProject.Application.Responses
{
    /// <summary>
    /// The repsponse of <see cref="GetBinaryDataCommand"/>.
    /// </summary>
    public class GetBinaryDataResponse
    {
        /// <summary>
        /// Gets or sets the side of the bynary data side in the comparison.
        /// </summary>
        public string ComparisonSide { get; set; }

        /// <summary>
        /// Gets or sets the Bynary Data to be compared. It should be a valid Base64 encoded string.
        /// </summary>
        public string Base64BinaryData { get; set; }

        /// <summary>
        /// Gets or sets the Comparison Id. It should be the same for the both Binary Data to be compared.
        /// </summary>
        public Guid ComparisonId { get; set; }

        /// <summary>
        /// Gets or sets de Binary Data Id.
        /// </summary>
        public Guid Id { get; set; }
    }
}

using System;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using MediatR;

namespace DiffProject.Application.Commands
{
    /// <summary>
    /// Command to set a new Binary Data.
    /// </summary>
    public class SetBinaryDataCommand : IRequest<SetBinaryDataResponse>
    {
        /// <summary>
        /// Gets or sets the Comparison Id. It should be the same for the both Binary Data to be compared.
        /// </summary>
        public Guid ComparisonID { get; set; }

        /// <summary>
        /// Gets or sets the Bynary Data to be compared. It should be a valid Base64 encoded string.
        /// </summary>
        public string Base64BinaryData { get; set; }

        /// <summary>
        /// Gets or sets the side of the bynary data side in the comparison.
        /// </summary>
        public SideEnum ComparisonSide { get; set; }
    }
}

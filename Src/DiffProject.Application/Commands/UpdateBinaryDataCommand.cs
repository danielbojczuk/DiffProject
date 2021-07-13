using System;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using MediatR;

namespace DiffProject.Application.Commands
{
    /// <summary>
    /// Command to set update a Binary Data.
    /// </summary>
    public class UpdateBinaryDataCommand : IRequest<UpdateBinaryDataResponse>
    {
        /// <summary>
        /// Gets or sets the current Comparison Id. It should be the same for the both Binary Data to be compared.
        /// </summary>
        public Guid CurrentComparisonID { get; set; }

        /// <summary>
        /// Gets or sets the new Bynary Data to be compared. It should be a valid Base64 encoded string.
        /// </summary>
        public string NewBase64BinaryData { get; set; }

        /// <summary>
        /// Gets or sets the side of the Bynary Data update.
        /// </summary>
        public SideEnum CurrentComparisonSide { get; set; }
    }
}

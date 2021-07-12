using System;
using DiffProject.Application.Enums;
using DiffProject.Application.Responses;
using MediatR;

namespace DiffProject.Application.Commands
{
    /// <summary>
    /// Command to get the Binary Data already set.
    /// </summary>
    public class GetBinaryDataCommand : IRequest<GetBinaryDataResponse>
    {
        /// <summary>
        /// Gets or sets the Bynary Data Comparison Id to query.
        /// </summary>
        public Guid ComparisonId { get; set; }

        /// <summary>
        /// Gets or sets the Binary Data Side to query.
        /// </summary>
        public SideEnum ComparisonSide { get; set; }
    }
}

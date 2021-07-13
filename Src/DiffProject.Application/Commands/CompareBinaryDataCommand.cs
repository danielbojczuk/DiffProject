using System;
using DiffProject.Application.Responses;
using MediatR;

namespace DiffProject.Application.Commands
{
    /// <summary>
    /// Command to calculate the differences between the Binary Data sets.
    /// </summary>
    public class CompareBinaryDataCommand : IRequest<ComparisonResultResponse>
    {
        /// <summary>
        /// Gets or sets comparison Id used in the Binary Data and result.
        /// </summary>
        public Guid ComparisonIs { get; set; }
    }
}

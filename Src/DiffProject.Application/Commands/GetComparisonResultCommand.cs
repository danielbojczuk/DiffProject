using DiffProject.Application.Responses;
using MediatR;
using System;

namespace DiffProject.Application.Commands
{
    /// <summary>
    /// Command to get the calculated Comparison Result.
    /// </summary>
    public class GetComparisonResultCommand : IRequest<ComparisonResultResponse>
    {
        /// <summary>
        /// Gets or sets the Comparison Id to query.
        /// </summary>
        public Guid ComparisonID { get; set; }
    }
}

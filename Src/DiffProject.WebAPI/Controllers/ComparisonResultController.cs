using DiffProject.Application.Commands;
using DiffProject.Application.Responses;
using DiffProject.WebAPI.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DiffProject.WebAPI.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [ServiceFilter(typeof(NotificationsFilter))]
    [IdValidationFilter]
    [ApiController]
    [Route("v1/diff/{ComparisonId}")]
    public class ComparisonResultController : ControllerBase
    {

        private readonly ILogger<BinaryDataController> _logger;

        private readonly IMediator _mediator;

        public ComparisonResultController(ILogger<BinaryDataController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string comparisonId)
        {
            ComparisonResultResponse response = await _mediator.Send(new GetComparisonResultCommand { ComparisonID = Guid.Parse(comparisonId) });
            if (response == null)
                return NotFound();
            else
                return Ok(response);
        }
    }
}

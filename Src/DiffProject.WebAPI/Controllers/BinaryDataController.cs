using DiffProject.Application.Commands;
using DiffProject.Application.Enums;
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
    [IdAndSideValidationFilter]
    [ApiController]
    [Route("v1/diff/{ComparisonId}/{ComparisonSide}")]
    public class BinaryDataController : ControllerBase
    {

        private readonly ILogger<BinaryDataController> _logger;

        private readonly IMediator _mediator;

        public BinaryDataController(ILogger<BinaryDataController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string comparisonId, string comparisonSide, [FromBody] string base64Encoded)
        {
            SetBinaryDataResponse response = await _mediator.Send(new SetBinaryDataCommand { ComparisonID = Guid.Parse(comparisonId), ComparisonSide = Enum.Parse<SideEnum>(comparisonSide), Base64BinaryData = base64Encoded });
            return Created($"v1/diff/{comparisonId}/{comparisonSide}", response);
        }

        [HttpPut]
        public async Task<ActionResult> Put(string comparisonId, string comparisonSide, [FromBody] string base64Encoded)
        {
            UpdateBinaryDataResponse response = await _mediator.Send(new UpdateBinaryDataCommand { CurrentComparisonID = Guid.Parse(comparisonId), CurrentComparisonSide = Enum.Parse<SideEnum>(comparisonSide), NewBase64BinaryData = base64Encoded });
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> Get(string comparisonId, string comparisonSide)
        {
            GetBinaryDataResponse response = await _mediator.Send(new GetBinaryDataCommand { ComparisonID = Guid.Parse(comparisonId), ComparisonSide = Enum.Parse<SideEnum>(comparisonSide) });
            if (response == null)
                return NotFound();
            else
                return Ok(response);
        }
    }
}

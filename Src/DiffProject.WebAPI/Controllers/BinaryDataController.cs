using DiffProject.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiffProject.WebAPI.Controllers
{
    [IdAndSideValidationFilter]
    [ApiController]
    [Route("v1/diff/{ComparisonId}/{ComparisonSide}")]
    public class BinaryDataController : ControllerBase
    {

        private readonly ILogger<BinaryDataController> _logger;

        public BinaryDataController(ILogger<BinaryDataController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post()
        {
            return Ok();
        }
    }
}

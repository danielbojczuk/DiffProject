using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace DiffProject.WebAPI.Filters
{
    public class ExceptionFilter: ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public ExceptionFilter(ILoggerFactory loggerFactory) : base()
        {
            _logger = loggerFactory.CreateLogger<ExceptionFilter>();

        }
        public override void OnException(ExceptionContext context)
        {
            _logger.LogError($"{DateTime.Now} - context.Exception.ToString()");
            ObjectResult result = new ObjectResult(null);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = result;
        }
    }
}

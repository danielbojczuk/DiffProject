using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;


namespace DiffProject.WebAPI.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public ExceptionFilter(ILoggerFactory loggerFactory) : base()
        {
            _logger = loggerFactory.CreateLogger<ExceptionFilter>();

        }
        public override async  void OnException(ExceptionContext context)
        {
            StreamReader streamReader = new StreamReader(context.HttpContext.Request.Body);

            StringBuilder errorLog = new StringBuilder();
            errorLog.AppendLine($"Time: {DateTime.Now}");
            errorLog.AppendLine($"Request Path: {context.HttpContext.Request.Path.ToString()}");
            errorLog.AppendLine($"Request Body: {await streamReader.ReadToEndAsync()}");
            errorLog.AppendLine($"Exception: {context.Exception.ToString()}");

            _logger.LogError(errorLog.ToString());
            ObjectResult result = new ObjectResult(null);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = result;
        }
    }
}

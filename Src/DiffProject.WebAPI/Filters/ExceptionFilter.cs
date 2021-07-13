using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DiffProject.WebAPI.Filters
{
    /// <summary>
    /// Logs the exception and return
    /// Internal Server Error with an empty body.
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFilter"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of <see cref="ILoggerFactory"/> implementation.</param>
        public ExceptionFilter(ILoggerFactory loggerFactory) 
            : base()
        {
            _logger = loggerFactory.CreateLogger<ExceptionFilter>();
        }

        ///<inheritdoc cref="OnException"/>
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

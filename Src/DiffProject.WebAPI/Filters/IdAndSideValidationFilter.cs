using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DiffProject.WebAPI.Filters
{
    public class IdAndSideValidationFilter : AbstractValidationFilter
    {
        /// <summary>
        /// This filter will let the request continue only if the Id and Side are in extpected format. 
        /// If the supplied data isn't as expected it will return a Bad Request
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            List<string> validationResult = new List<string>();

            if (!IsIdValid(context.RouteData.Values.ElementAt(2).Value.ToString()))
                validationResult.Add("Supplied ID is invalid");

            if (!IsSideValid(context.RouteData.Values.ElementAt(3).Value.ToString()))
                validationResult.Add("Supplied SIDE is invalid");

            if (validationResult.Count > 0)
            {
                ObjectResult result = new ObjectResult(validationResult);
                result.StatusCode = (int)HttpStatusCode.BadRequest;
                result.ContentTypes.Add(MediaTypeHeaderValue.Parse("application/json;"));
                context.Result = result;

            }
        }
    }
}

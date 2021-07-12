using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace DiffProject.WebAPI.Filters
{
    public class IdValidationFilter : AbstractValidationFilter
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

            if (validationResult.Count > 0)
                context.Result = CreateBadRequestResult(validationResult);
        }
    }
}

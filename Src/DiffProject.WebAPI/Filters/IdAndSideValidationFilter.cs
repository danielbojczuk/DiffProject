using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace DiffProject.WebAPI.Filters
{
    /// <summary>
    /// This filter will only let the request continue if the Id and Side are as expected.
    /// </summary>
    public class IdAndSideValidationFilter : AbstractValidationFilter
    {
        ///<inheritdoc cref="OnActionExecuting"/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            List<string> validationResult = new List<string>();

            if (!IsValidId(context.RouteData.Values.ElementAt(2).Value.ToString()))
            {
                validationResult.Add("Supplied ID is invalid");
            }

            if (!IsSideValid(context.RouteData.Values.ElementAt(3).Value.ToString()))
            {
                validationResult.Add("Supplied SIDE is invalid");
            }

            if (validationResult.Count > 0)
            {
                context.Result = CreateBadRequestResult(validationResult);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiffProject.WebAPI.Filters
{
    /// <summary>
    /// Let the request continue only if the Id is as expected.
    /// </summary>
    public class IdValidationFilter : AbstractValidationFilter
    {
        ///<inheritdoc cref="OnActionExecuting"/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            List<string> validationResult = new List<string>();

            if (!IsValidId(context.RouteData.Values.ElementAt(2).Value.ToString()))
            {
                context.Result = CreateBadRequestResult(new List<string>() { "Supplied ID is invalid" });
            }
        }
    }
}

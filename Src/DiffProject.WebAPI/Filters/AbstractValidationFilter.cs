using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DiffProject.WebAPI.Filters
{
    /// <summary>
    /// Provide the Filter melhtods to validate the Id and the Side provided in the request
    /// </summary>
    public class AbstractValidationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Check if the provided string is a GUID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool IsIdValid(string id)
        {
            Guid guid = new Guid();
            return Guid.TryParse(id, out guid);
        }
        /// <summary>
        /// Profide if the provided side comparison is valid
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        protected bool IsSideValid(string side)
        {
            return (side == "left" || side == "right");
        }
    }
}

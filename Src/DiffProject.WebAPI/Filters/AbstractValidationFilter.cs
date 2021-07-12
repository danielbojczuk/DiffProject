using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace DiffProject.WebAPI.Filters
{
    /// <summary>
    /// Abstract Validation Filter.
    /// </summary>
    public class AbstractValidationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Check if the provided string is a GUID.
        /// </summary>
        /// <param name="id">String with a Guid to validate.</param>
        /// <returns>Validation result.</returns>
        protected bool IsValidId(string id)
        {
            Guid guid = new Guid();
            return Guid.TryParse(id, out guid);
        }
        /// <summary>
        /// Check if the provided side is valid.
        /// </summary>
        /// <param name="side">The side to validate</param>
        /// <returns>Validation result.</returns>
        protected bool IsSideValid(string side)
        {
            return side == "left" || side == "right";
        }

        /// <summary>
        /// Create te BadRequest result to be returnd by the filtes.
        /// </summary>
        /// <param name="validationResult">List of validation messages.</param>
        /// <returns>The result to be returned</returns>
        protected ObjectResult CreateBadRequestResult(List<string> validationResult)
        {
            ObjectResult result = new ObjectResult(validationResult);
            result.StatusCode = (int)HttpStatusCode.BadRequest;
            result.ContentTypes.Add(MediaTypeHeaderValue.Parse("application/json;"));
            return result;

        }
    }
}

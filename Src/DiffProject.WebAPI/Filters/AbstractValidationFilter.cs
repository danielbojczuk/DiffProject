using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;

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

        protected ObjectResult CreateBadRequestResult(List<string> validationResult)
        {
            ObjectResult result = new ObjectResult(validationResult);
            result.StatusCode = (int)HttpStatusCode.BadRequest;
            result.ContentTypes.Add(MediaTypeHeaderValue.Parse("application/json;"));
            return result;

        }
    }
}

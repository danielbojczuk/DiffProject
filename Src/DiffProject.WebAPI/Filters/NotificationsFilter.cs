using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DiffProject.Application.CommandHandlers.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiffProject.WebAPI.Filters
{
    /// <summary>
    /// This filter will get the notifications in the command handler
    /// and send create a response with a Bad Request.
    /// </summary>
    public class NotificationsFilter : IAsyncResultFilter
    {
        private readonly INotificationContext _notificationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsFilter"/> class.
        /// </summary>
        /// <param name="notificationContext">Instance of <see cref="INotificationContext"/> implementation</param>
        public NotificationsFilter(INotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        ///<inheritdoc cref="OnResultExecutionAsync"/>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_notificationContext.HasNotifications)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.HttpContext.Response.ContentType = "application/json";

                var notifications = JsonSerializer.Serialize(_notificationContext.Notifications);
                await context.HttpContext.Response.WriteAsync(notifications);

                return;
            }

            await next();
        }
    }
}

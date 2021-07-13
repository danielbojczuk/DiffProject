using DiffProject.Application.CommandHandlers.Notifications;


namespace DiffProject.Application.CommandHandlers
{
    /// <summary>
    /// It provides the Notification Context containing the business validation results.
    /// </summary>
    public abstract class AbstractCommandHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCommandHandler"/> class.
        /// </summary>
        /// <param name="notificationContext">dddd.</param>
        public AbstractCommandHandler(INotificationContext notificationContext)
        {
            NotificationContext = notificationContext;
        }

        /// <summary>
        /// Gets the business logic validation results.
        /// </summary>
        public INotificationContext NotificationContext { get; private set; }
    }
}

using DiffProject.Application.CommandHandlers.Notifications;


namespace DiffProject.Application.CommandHandlers
{
    /// <summary>
    /// Abstract 'Command Handler'. It provides the Notification context to be used in order to Add the business validation results.
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

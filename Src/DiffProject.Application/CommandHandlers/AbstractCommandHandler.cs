using DiffProject.Application.CommandHandlers.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers
{
    ///<summary>
    ///Handles the Command Set Data to perform an inclusion of a binary data to compare.
    ///</summary>
    public abstract class AbstractCommandHandler<T, K>
    {
        public INotificationContext NotificationContext { get; private set; }

        public AbstractCommandHandler(INotificationContext notificationContext)
        {
            NotificationContext = notificationContext;
        }
        public abstract Task<K> Handle(T command, CancellationToken cancellationToken);

    }
}

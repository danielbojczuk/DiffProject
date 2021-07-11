using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffProject.Application.CommandHandlers.Notifications
{
    public class NotificationContext : INotificationContext
    {
        public List<string> Notifications { get; private set; }

        public NotificationContext()
        {
            Notifications = new List<string>();
        }

        public bool HasNotifications
        {
            get
            {
                return Notifications.Any();
            }
        }

        public void AddNotification(string message)
        {
            Notifications.Add(message);
        }

        public void AddNotifications(ValidationResult validationResult)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                AddNotification(error.ErrorMessage);
            }
        }
    
    }
}

using FluentValidation.Results;
using System.Collections.Generic;

namespace DiffProject.Application.CommandHandlers.Notifications
{
    public interface INotificationContext
    {
        List<string> Notifications { get; }

        bool HasNotifications { get; }

        void AddNotification(string message);

        void AddNotifications(ValidationResult validationResult);
    }
}

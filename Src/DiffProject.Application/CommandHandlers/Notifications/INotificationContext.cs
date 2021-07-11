using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

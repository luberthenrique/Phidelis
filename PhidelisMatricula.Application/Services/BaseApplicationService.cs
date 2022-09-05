using FluentValidation.Results;
using MediatR;
using PhidelisMatricula.Domain.Core.Notifications;

namespace PhidelisMatricula.Application.Interfaces
{
    public abstract class BaseApplicationService
    {
        protected readonly DomainNotificationHandler _notifications;

        protected BaseApplicationService(
            INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        protected void NotifyValidationErrors(ValidationResult validation, string type)
        {
            foreach (var error in validation.Errors)
            {
                _notifications.AddNotification(type, error.ErrorMessage);
            }
        }
    }
}

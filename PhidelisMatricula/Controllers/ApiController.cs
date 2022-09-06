using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhidelisMatricula.Domain.Core.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace PhidelisMatricula.Presentation.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected readonly DomainNotificationHandler _notifications;

        protected ApiController(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        protected void NotifyModelStateErrors()
        {
            var erros = _notifications.GetNotifications();
            foreach (var erro in erros)
            {
                NotifyError(erro.Key, erro.Value);
            }
        }

        protected void NotifyError(string code, string message)
        {
            ModelState.AddModelError(code, message);
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }

        protected bool HasNotifications
        {
            get
            {
                NotifyModelStateErrors();
                return !ModelState.IsValid;
            }
        }
    }
}

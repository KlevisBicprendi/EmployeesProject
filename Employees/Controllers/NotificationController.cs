using BLL_Punonjes.Evente.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Controllers
{
    public class NotificationController (INotificationService notificationService) : Controller
    {
        public IActionResult Index()
        {
            var listWithNotification = notificationService.GetNotifications().ToList();
            return View(listWithNotification);
        }
    }
}

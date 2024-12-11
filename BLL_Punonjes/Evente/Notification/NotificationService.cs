using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Evente.Notification
{
    public interface INotificationService 
    {
        void AddNotification(NotificationDTO notificationDTO);
        IEnumerable<NotificationDTO> GetNotifications();
    }
    public class NotificationDTO()
    {
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class NotificationService : INotificationService
    {
        private readonly List<NotificationDTO> _notifications = new List<NotificationDTO>();
        public void AddNotification(NotificationDTO notificationDTO)
        {
            _notifications.Add(notificationDTO);
        }

        public IEnumerable<NotificationDTO> GetNotifications()
        {
            return _notifications;
        }
    }
}

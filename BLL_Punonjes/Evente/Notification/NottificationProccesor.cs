using BLL_Punonjes.Services.Scoped;
using DAL_Punonjes.Entities;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Evente.Notification
{
    public class NottificationProccesor (INotificationService notificationService) : IHostedService
    {
        private readonly INotificationService _notificationService = notificationService;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ReviewService.OnReviewEvent += AddReviewNotification;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            ReviewService.OnReviewEvent -= AddReviewNotification;
        }
        public void AddReviewNotification(int id)
        {
            _notificationService.AddNotification(new NotificationDTO
            {
                Message = $"Review with ID : {id} id added with success",
                CreatedOn = DateTime.Now,
            });
        }
    }
}
using BLL_Punonjes.Evente.Notification;
using BLL_Punonjes.Services.Hosted;
using BLL_Punonjes.Services.Scoped;
using BLL_Punonjes.Services.Singletone;
using DAL_Punonjes.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes
{
    public static class BLL_Startup
    {
        public static void RegisterBLLServices(this IServiceCollection services) 
        {
            services.AddHostedService<NottificationProccesor>();
            services.AddHostedService<AuditLogServiceHosted>();
            services.AddScoped<IInternalAuditService,AuditLogService>();
            services.AddScoped<IEmployerService,EmployerService>();
            services.AddScoped<IEmployerToUserService,EmployerToUserService>();
            services.AddScoped<IReviewService,ReviewService>();
            services.AddScoped<IProvaCache,ProvaCache>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IReservationService,ReservationService>();
            services.AddSingleton<INotificationService,NotificationService>();
            services.AddScoped<IServiceManager,ServiceManager>();

            services.AddSingleton<ICacheService,CacheService>();
            services.AddSingleton<ILoggerService,LoggerService>();
            services.AddScoped<EmployerEventNotification>();
            services.AddHttpContextAccessor();
        }
    }
}

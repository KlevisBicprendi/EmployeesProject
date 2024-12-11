using DAL_Punonjes.Entities;
using DAL_Punonjes.Repositories;
using DAL_Punonjes.UNITOFWORK;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes
{
    public static class DAL_Startup
    {
        public static void RegisterDALServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<EmployerDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDefaultIdentity<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<EmployerDbContext>();
             

            services.AddScoped<IEmployerRepository, EmployerRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IEmployerToUserRepository, EmployerToUserRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
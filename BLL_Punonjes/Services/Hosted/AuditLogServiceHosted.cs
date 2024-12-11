using BLL_Punonjes.Services.Scoped;
using DAL_Punonjes.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Hosted
{
    public class AuditLogServiceHosted (IServiceProvider serviceProvider) : IHostedService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        
        public void AddLog(AuditLog auditLog)
        {
            var scope = _serviceProvider.CreateScope();
            var auditLogService = scope.ServiceProvider.GetService<IInternalAuditService>();
            auditLogService.Add(auditLog);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            EmployerService.OnEntityAdded += AddLog;
            EmployerService.OnEntityUpdated += AddLog;
            EmployerService.OnEntityRemoved += AddLog;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            EmployerService.OnEntityAdded -= AddLog;
            EmployerService.OnEntityUpdated -= AddLog;
            EmployerService.OnEntityRemoved -= AddLog;
        }
    }
}

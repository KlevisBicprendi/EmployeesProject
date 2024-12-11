using DAL_Punonjes.Entities;
using DAL_Punonjes.Repositories;
using DAL_Punonjes.UNITOFWORK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Scoped
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLog>> GetAuditLogs();
    }
    public interface IInternalAuditService : IAuditLogService
    {
        Task Add(AuditLog log);
    }
    
    public delegate void OnEntityAdded(AuditLog log);
    public delegate void OnEntityUpdated(AuditLog log);
    public delegate void OnEntityRemoved(AuditLog log);

    public class AuditLogService : IAuditLogService, IInternalAuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AuditLogService(IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork) 
        {
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Add(AuditLog log)
        {
            await _auditLogRepository.Create(log);
            await _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogs()
        {
            return await _auditLogRepository.GetAuditLogs();
        }
    }
}

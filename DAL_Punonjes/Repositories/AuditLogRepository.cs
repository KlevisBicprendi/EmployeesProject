using DAL_Punonjes.Entities;
using DAL_Punonjes.UNITOFWORK;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.Repositories
{
    public interface IAuditLogRepository
    {
        Task Create(AuditLog auditLog);
        Task Delete(AuditLog auditLog);
        Task<IEnumerable<AuditLog>> GetAuditLogs();
        Task<AuditLog> GetAuditLogById(long id);
    }
    public class AuditLogRepository (EmployerDbContext dbContext,IUnitOfWork unitOfWork) : IAuditLogRepository
    {
        public async Task Create(AuditLog auditLog)
        {
            dbContext.Set<AuditLog>().Add(auditLog);
            await unitOfWork.SaveChanges();
        }

        public async Task Delete(AuditLog auditLog)
        {
            dbContext.Set<AuditLog>().Remove(auditLog);
            await unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogs()
        {
            return dbContext.Set<AuditLog>().ToList();
        }

        public async Task<AuditLog> GetAuditLogById(long id)
        {
            return dbContext.Set<AuditLog>()
                .FirstOrDefault(x=>x.Id == id) ?? throw new Exception("This AuditLog doesn't exist !!!");
        }
    }
}
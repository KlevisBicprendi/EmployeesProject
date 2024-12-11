using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.UNITOFWORK
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
        Task<T> ExecuteTransaction<T>(Func<Task<T>> func);
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployerDbContext _dbContext;
        public UnitOfWork(EmployerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> ExecuteTransaction<T>(Func<Task<T>> func)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var result = await func();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
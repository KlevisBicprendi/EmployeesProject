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
    public interface IEmployerToUserRepository
    {
        Task AddEmployerToUser(int userId, int employerId);
        Task Delete(int userId, int employerId);
        Task<IEnumerable<int>> GetUserEmployersById(int userId);
    }
    public class EmployerToUserRepository : IEmployerToUserRepository
    {
        private readonly EmployerDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public EmployerToUserRepository(EmployerDbContext dbContext,IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public async Task AddEmployerToUser(int userId, int employerId)
        {
           _dbContext.Set<EmployerToUser>().Add(new EmployerToUser { UserId = userId, EmployerId = employerId });
           await _unitOfWork.SaveChanges();
        }

        public async Task Delete(int userId,int employerId)
        {
            var itemForDelete = _dbContext.Set<EmployerToUser>()
                .FirstOrDefault(x=> x.UserId == userId && x.EmployerId == employerId);
            if (itemForDelete != null)
            {
                _dbContext.Remove(itemForDelete);
                await _unitOfWork.SaveChanges();
            }
        }

        public async Task<IEnumerable<int>> GetUserEmployersById(int userId)
        {
            return _dbContext.Set<EmployerToUser>().AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.EmployerId)
                .ToList();
        }
    }
}
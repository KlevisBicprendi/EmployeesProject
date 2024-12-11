using DAL_Punonjes.Entities;
using DAL_Punonjes.UNITOFWORK;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.Repositories
{
    public interface IEmployerRepository
    {
        Task Create(Employer employer);
        Task Delete(int id);
        Task<IEnumerable<Employer>> GetAllEmployees();
        Task<Employer> GetEmployerById(int id);
    }
    public class EmployerRepository : IEmployerRepository
    {
        private readonly EmployerDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public EmployerRepository(EmployerDbContext dbContext, IUnitOfWork unitOfWork) 
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public async Task Create(Employer employer)
        {
            _dbContext.Set<Employer>().Add(employer);
            await _unitOfWork.SaveChanges();
        }
        public async Task Delete(int id)
        {
            var employer = _dbContext.Set<Employer>().FirstOrDefault(x => x.Id == id);
            _dbContext.Set<Employer>().Remove(employer);
            await _unitOfWork.SaveChanges();
        }
        public async Task<IEnumerable<Employer>> GetAllEmployees()
        {
            return _dbContext.Set<Employer>().ToList();
        }
        public async Task<Employer> GetEmployerById(int id)
        {
            return _dbContext.Set<Employer>().Find(id) ?? throw new Exception("This Employer Does not exists");
        }
    }
}
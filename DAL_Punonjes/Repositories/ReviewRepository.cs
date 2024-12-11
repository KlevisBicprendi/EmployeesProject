using DAL_Punonjes.Entities;
using DAL_Punonjes.UNITOFWORK;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.Repositories
{
    public interface IReviewRepository
    {
        Task Create(Review review);
        Task Delete(Review review);
        Task<IEnumerable<Review>> GetAllReview();
        Task<Review> GetReviewById(int id);
        Task<Review> GetReviewByUserId(int id);

    }
    public class ReviewRepository : IReviewRepository
    {
        private readonly EmployerDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public ReviewRepository(EmployerDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        protected Microsoft.EntityFrameworkCore.DbSet<Review> _dbSet => _dbContext.Set<Review>();
        public async Task Create(Review review)
        {
            _dbSet.Add(review);
            await _unitOfWork.SaveChanges();
        }

        public async Task Delete(Review review)
        {
            _dbSet.Remove(review);
            await _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<Review>> GetAllReview()
        {
            return _dbSet.ToList();
        }

        public async Task<Review> GetReviewById(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Review> GetReviewByUserId(int id)
        {
            return _dbSet.FirstOrDefault(x=>x.UserId == id);
        }
    }
}
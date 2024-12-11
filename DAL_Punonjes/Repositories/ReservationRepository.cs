using DAL_Punonjes.Entities;
using DAL_Punonjes.UNITOFWORK;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.Repositories
{
    public interface IReservationRepository
    {
        Task Create(Reservation reservation);
        Task Delete(Reservation reservation);
        Task <IEnumerable<Reservation>> GetAllReservations(StatusEnum status,int userId);
        Task<IEnumerable<Reservation>> GetReservationsForAdmin();
        Task <Reservation> GetReservationById(int id);
    }
    public class ReservationRepository (EmployerDbContext dbContext,IUnitOfWork unitOfWork): IReservationRepository
    {
        private readonly EmployerDbContext _dbContext = dbContext;
        private readonly IUnitOfWork _unitofwork = unitOfWork;
        public async Task Create(Reservation reservation)
        {
            _dbContext.Set<Reservation>().Add(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Reservation reservation)
        {
            _dbContext.Set<Reservation>().Remove(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations(StatusEnum status,int userId)
        {
            return _dbContext.Set<Reservation>().Where(x=>x.Status == status && x.UserId == userId).ToList();
        }
        public async Task<IEnumerable<Reservation>> GetReservationsForAdmin()
        {
            return _dbContext.Set<Reservation>().ToList();
        }

        public async Task<Reservation> GetReservationById(int id)
        {
            return _dbContext.Set<Reservation>().FirstOrDefault(x=>x.Id == id)
                ?? throw new Exception("This Reservation doesn't exists");
        }
    }
}
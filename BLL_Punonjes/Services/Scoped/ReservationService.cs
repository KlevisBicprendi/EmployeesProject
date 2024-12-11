using BLL_Punonjes.Requests.AddOrEditRequests;
using DAL_Punonjes.Entities;
using DAL_Punonjes.Repositories;
using DAL_Punonjes.UNITOFWORK;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Scoped
{
    public interface IReservationService
    {
        Task <bool> AddReservation(ReservationDTO reservationDTO,int userId,int employerId);
        Task Delete(int id);
        Task<Reservation> GetReservationById(int id);
        Task<IEnumerable<Reservation>> GetAllReservations(StatusEnum status, int userId);
    }
    public class ReservationService(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork,
        IUserService userService
        ): IReservationService
    {
        private readonly IReservationRepository _reservationRepository = reservationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserService _userService = userService;

        public static event OnEntityAdded OnReservationAdded;
        public static event OnEntityUpdated OnReservationUpdated;
        public static event OnEntityRemoved OnReservationRemoved;

        public async Task Delete(int id)
        {
           var ReservationForDelete = await _reservationRepository.GetReservationById(id);
            if (ReservationForDelete != null) 
            {
                OnReservationRemoved?.Invoke(new AuditLog
                {
                    EntityName = "Reservation",
                    EntityId = ReservationForDelete.Id.ToString(),
                    LogType = AuditLogType.Delete,
                    CreatedOn = DateTime.UtcNow,
                    Details = Newtonsoft.Json.JsonConvert.SerializeObject(ReservationForDelete)
                });
                await _reservationRepository.Delete(ReservationForDelete);
            }
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations(StatusEnum status,int userId)
        {
            if (await _userService.IsAdmin())
            {
                return await _reservationRepository.GetReservationsForAdmin();
            }
            else
            {
                return await _reservationRepository.GetAllReservations(status, userId);
            }
        }

        public async Task<Reservation> GetReservationById(int id)
        {
            return await _reservationRepository.GetReservationById(id);
        }
        public async Task<bool> AddReservation(ReservationDTO reservationDTO,int userId,int employerId)
        {
            var ReservationForAdd = _reservationRepository.Create(
                new Reservation
                {
                    Offer = reservationDTO.Offer,
                    EmployerId = employerId,
                    UserId = userId,
                    Purpose = reservationDTO.Purpose,
                    Status = StatusEnum.Pending
                });
            OnReservationAdded?.Invoke(
                new AuditLog
                {
                    EntityName = "Reservation",
                    EntityId = ReservationForAdd.Id.ToString(),
                    LogType = AuditLogType.Create,
                    CreatedOn = DateTime.UtcNow,
                    Details = Newtonsoft.Json.JsonConvert.SerializeObject(ReservationForAdd)
                });
            return true;
        }
        public async Task Confirmed(int id)
        {
           var ReservationConfirmed = await _reservationRepository.GetReservationById(id);
            if (ReservationConfirmed != null)
            {
                ReservationConfirmed.Status = StatusEnum.Confirmed;
                await _unitOfWork.SaveChanges();
                OnReservationAdded?.Invoke(
             new AuditLog
             {
                 EntityName = "Reservation",
                 EntityId = ReservationConfirmed.Id.ToString(),
                 LogType = AuditLogType.Update,
                 CreatedOn = DateTime.UtcNow,
                 Details = Newtonsoft.Json.JsonConvert.SerializeObject(ReservationConfirmed)
             });
            }
        }
        public async Task Rejected(int id)
        {
            var ReservationForRejected = await _reservationRepository.GetReservationById(id);
            if (ReservationForRejected != null) 
            {
                ReservationForRejected.Status = StatusEnum.Rejected;
                OnReservationAdded?.Invoke(
             new AuditLog
             {
                 EntityName = "Reservation",
                 EntityId = ReservationForRejected.Id.ToString(),
                 LogType = AuditLogType.Update,
                 CreatedOn = DateTime.UtcNow,
                 Details = Newtonsoft.Json.JsonConvert.SerializeObject(ReservationForRejected)
             });
            }
        }
        public async Task Canceled(int id)
        {
            var ReservationForCanceled = await _reservationRepository.GetReservationById(id);
            if (ReservationForCanceled != null)
            {
                ReservationForCanceled.Status = StatusEnum.Canceled;
                OnReservationAdded?.Invoke(
             new AuditLog
             {
                 EntityName = "Reservation",
                 EntityId = ReservationForCanceled.Id.ToString(),
                 LogType = AuditLogType.Update,
                 CreatedOn = DateTime.UtcNow,
                 Details = Newtonsoft.Json.JsonConvert.SerializeObject(ReservationForCanceled)
             });
            }
        }
    }
}
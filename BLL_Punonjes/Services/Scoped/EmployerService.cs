using BLL_Punonjes.Evente;
using BLL_Punonjes.Requests.AddOrEditRequests;
using BLL_Punonjes.Services.Singletone;
using DAL_Punonjes.Entities;
using DAL_Punonjes.Repositories;
using DAL_Punonjes.UNITOFWORK;
using MathNet.Numerics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Scoped
{
    public delegate void EmployerChangedEventHandler(object source, EmployerEventArg e);
    public interface IEmployerService
    {
        public event EmployerChangedEventHandler EmployerChanged;
        Task Create(EmployerDTO employerAddOrEditRequests);
        Task Delete(int id);
        Task Update(EmployerDTO employerAddOrEditRequests, int id);
        Task<Employer> GetEmployerById(int id);
        Task<IEnumerable<Employer>> GetAllEmployees();
        Task UpdateAvailability(int id);
    }

    public class EmployerService(
        IEmployerRepository employerRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService
        ) : IEmployerService
    {

        public static event OnEntityAdded OnEntityAdded;
        public static event OnEntityUpdated OnEntityUpdated;
        public static event OnEntityRemoved OnEntityRemoved;

        public event EmployerChangedEventHandler EmployerChanged;
        public async Task Create(EmployerDTO employerAddOrEditRequests)
        {
            var newEmployer = new Employer
            {
                Name = employerAddOrEditRequests.Name,
                Adresa = employerAddOrEditRequests.Adresa,
                Description = employerAddOrEditRequests.Description,
                Email = employerAddOrEditRequests.Email,
                PhoneNumber = employerAddOrEditRequests.PhoneNumber
            };
            await employerRepository.Create(newEmployer);
            OnEntityAdded?.Invoke(
                new AuditLog
                {
                    EntityId = $"{newEmployer.Id}",
                    EntityName = "Employer",
                    Details = Newtonsoft.Json.JsonConvert.SerializeObject(newEmployer),
                    LogType = AuditLogType.Create
                });
            cacheService.RemoveCache("Employees");
            OnEmployerChanged(newEmployer, "Create");
        }

        public async Task Delete(int id)
        {
            var EmployerForDelete = await employerRepository.GetEmployerById(id);
            await employerRepository.Delete(id);
            OnEntityRemoved?.Invoke(
                new AuditLog
                {
                    EntityId = $"{EmployerForDelete.Id}",
                    EntityName = "Employer",
                    Details = Newtonsoft.Json.JsonConvert.SerializeObject(EmployerForDelete),
                    LogType = AuditLogType.Delete
                });
            cacheService.RemoveCache("Employees");
            OnEmployerChanged(EmployerForDelete,"Delete");
        }

        public async Task<IEnumerable<Employer>> GetAllEmployees()
        {
            return await cacheService.AddOrEdit("Employees", async () =>
            {
                var employees = await employerRepository.GetAllEmployees();
                return employees.Select(x => new Employer
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Adresa = x.Adresa,
                    Email = x.Email,
                    IsAvailable = x.IsAvailable,
                    PhoneNumber = x.PhoneNumber,
                }).ToList();
            });
        }
        public async Task<Employer> GetEmployerById(int id)
        {
            var Employees = GetAllEmployees().Result;
            return Employees.FirstOrDefault(x => x.Id == id)?? throw new Exception("Employer with this Id doesn't exist !!!");
        }
        

        public async Task Update(EmployerDTO employerAddOrEditRequests, int id)
        {
            var EmployerForUpdate = employerRepository.GetEmployerById(id).Result ?? throw new("This employer with this Id doesn't exists");
            EmployerForUpdate.Name = employerAddOrEditRequests.Name;
            EmployerForUpdate.Email = employerAddOrEditRequests.Email;
            EmployerForUpdate.Description = employerAddOrEditRequests.Description;
            EmployerForUpdate.PhoneNumber = employerAddOrEditRequests.PhoneNumber;
            EmployerForUpdate.Adresa = employerAddOrEditRequests.Adresa;
            await unitOfWork.SaveChanges();
            OnEntityRemoved?.Invoke(
             new AuditLog
             {
                 EntityId = $"{EmployerForUpdate.Id}",
                 EntityName = "Employer",
                 Details = Newtonsoft.Json.JsonConvert.SerializeObject(EmployerForUpdate),
                 LogType = AuditLogType.Update
             });
            cacheService.RemoveCache("Employees");
            OnEmployerChanged(EmployerForUpdate,"Update");
        }
        protected virtual void OnEmployerChanged(Employer employer, string action)
        {
            EmployerChanged?.Invoke(employer, new EmployerEventArg { Action = action, employer = employer });
        }

        public async Task UpdateAvailability(int id)
        {
            try
            {
   
                var employer = await employerRepository.GetEmployerById(id);

                if (employer == null)
                {
                    throw new Exception("Employer not found!");
                }

                employer.IsAvailable = !employer.IsAvailable;
                await unitOfWork.SaveChanges();


                OnEntityRemoved?.Invoke(new AuditLog
                {
                    EntityId = employer.Id.ToString(),
                    EntityName = "Employer",
                    Details = Newtonsoft.Json.JsonConvert.SerializeObject(employer),
                    LogType = AuditLogType.Update
                });
                cacheService.RemoveCache("Employees");
            }
            catch
            {
                throw;
            }
        }

    }
}
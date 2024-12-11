using DAL_Punonjes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Scoped
{
    public interface IEmployerToUserService
    {
        Task Toggle(int userId, int employerId);
        Task <IEnumerable<int>> GetUserEmployers(int userId);
        Task<bool> IsFavor(int userId,int employerId);
    }
    public class EmployerToUserService(IEmployerToUserRepository employerToUserRepository) : IEmployerToUserService
    {
        public async Task<IEnumerable<int>> GetUserEmployers(int userId)
        {
            return await employerToUserRepository.GetUserEmployersById(userId);
        }

        public async Task<bool> IsFavor(int userId, int employerId)
        {
            var UserEmployers = await employerToUserRepository.GetUserEmployersById(userId);
            foreach (var item in UserEmployers)
            {
                if (item == employerId)
                {
                    return true;
                }
            }  
            return false;
        }

        public async Task Toggle(int userId, int employerId)
        {
            var userEmployer = await GetUserEmployers(userId);
            if (userEmployer.Contains(employerId))
            {
                await employerToUserRepository.Delete(userId, employerId);
            }
            else
            {
                await employerToUserRepository.AddEmployerToUser(userId, employerId);
            }
        }
    }
}

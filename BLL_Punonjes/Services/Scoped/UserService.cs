using DAL_Punonjes.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Scoped
{
    public interface IUserService
    {
        Task<int> GetIdByCurrentUser();
        Task<bool> IsAdmin();
    }
    public class UserService (
        Microsoft.AspNetCore.Identity.UserManager<User> userManager,
        IHttpContextAccessor httpContext) 
        : IUserService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager = userManager;
        private readonly IHttpContextAccessor _contextAccessor = httpContext;
        public async Task<int> GetIdByCurrentUser()
        {
            var user = _contextAccessor.HttpContext?.User ?? throw new Exception("You are not logged in!");
            var userIdString = _userManager.GetUserId(user);

            if (!int.TryParse(userIdString, out var userId))
            {
                throw new Exception("Invalid user ID format!");
            }

            return userId;
        }
        public async Task<bool> IsAdmin()
        {
            var User = _contextAccessor.HttpContext?.User ?? throw new Exception("User not Logged in !!!");
            if (User.IsInRole("Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

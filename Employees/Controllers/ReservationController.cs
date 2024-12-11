using BLL_Punonjes.Requests.AddOrEditRequests;
using BLL_Punonjes.Services.Scoped;
using DAL_Punonjes.Entities;
using DAL_Punonjes.Repositories;
using Employees.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace Employees.Controllers
{
    public class ReservationController(
        IReservationService reservationService,
        IUserService userService,
        IServiceManager serviceManager,
        IEmployerService employerService
        ) : Controller
    {
        private readonly IEmployerService _employerService = employerService;
        private readonly IReservationService _reservationService = reservationService;
        private readonly IUserService _userService = userService;
        [HttpGet]
        public async Task<IActionResult> Create(int employerId)
        {
            return View(new ReservationDTO { EmployerId = employerId });
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReservationDTO reservationDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(reservationDTO);
            }

            try
            {
                var userId = await _userService.GetIdByCurrentUser();
                return await serviceManager.ExecuteTransactions(async () =>
                {
                    await _reservationService.AddReservation(reservationDTO, userId, reservationDTO.EmployerId);
                    await _employerService.UpdateAvailability(reservationDTO.EmployerId);
                    return RedirectToAction("Index");
                });

            }
            catch
            {   
                ModelState.AddModelError("", "Një gabim ndodhi gjatë përpunimit. Ju lutemi provoni përsëri.");
                return View(reservationDTO);
            }
        }

        public async Task<IActionResult> Index()
        {
            var ReservationsList = await _reservationService.GetAllReservations(StatusEnum.Pending, await _userService.GetIdByCurrentUser());
            List<ReservationViewModel> list = new List<ReservationViewModel>();
            foreach (var item in ReservationsList.ToList())
            {
                list.Add(new ReservationViewModel
                {
                    Id = item.Id,
                    CreatedOn = $"{item.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")}",
                    EmployerId = item.EmployerId,
                    Offer = item.Offer,
                    Purpose = item.Purpose,
                    Status = item.Status,
                    UserId = item.UserId
                });
            }
            return View(list);
        }
    }
}
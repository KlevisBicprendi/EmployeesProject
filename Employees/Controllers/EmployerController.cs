using BLL_Punonjes.Evente.Notification;
using BLL_Punonjes.Requests.AddOrEditRequests;
using BLL_Punonjes.Services.Scoped;
using BLL_Punonjes.Services.Singletone;
using DAL_Punonjes.Entities;
using Employees.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Employees.Controllers
{
    public class EmployerController : Controller
    {
        private readonly IEmployerService _employerService;
        private readonly IUserService _userService;
        private readonly ILoggerService _loggerService;
        private readonly IEmployerToUserService _employerToUserService;
        private readonly EmployerEventNotification _employerEventNotification;
        public EmployerController(
        IEmployerService employerService,
        IUserService userService,
        ILoggerService loggerService,
        IEmployerToUserService employerToUserService,
        EmployerEventNotification employerEventNotification
        )
        {
            _employerService = employerService;
            _userService = userService;
            _employerToUserService = employerToUserService;
            _loggerService = loggerService;
            _employerEventNotification = employerEventNotification;
            Initialize();
        }
        public void Initialize()
        {
            if (_employerEventNotification != null)
            {
                _employerService.EmployerChanged += _employerEventNotification.OnEmployerChanged;
            }
        }

        ////Delegat per shtim Punonjesi
        //public delegate void EmployerAdded(int id);
        ////Eventi per shtim Punonjesi i lidhur me delegatin me siper
        //public static event EmployerAdded OnEmployerAdded;

        ////Eventet
        //public static event OnEntityAdded OnEntityAdded;
        //public static event OnEntityUpdated OnEntityUpdated;
        //public static event OnEntityRemoved OnEntityRemoved;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var AllEmployees = _employerService.GetAllEmployees().Result.ToList();
                List<EmployerViewModel> employerViewModels = new List<EmployerViewModel>();
                int UserId = await _userService.GetIdByCurrentUser();
                foreach (var employee in AllEmployees)
                {
                    employerViewModels.Add(new EmployerViewModel
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Description = employee.Description,
                        Adresa = employee.Adresa,
                        Email = employee.Email,
                        PhoneNumber = employee.PhoneNumber,
                        IsFavourite = await _employerToUserService.IsFavor(UserId, employee.Id),
                        IsAvailable = employee.IsAvailable
                    });
                }
               
                _loggerService.Log("Info", $"AllEmployer : {JsonConvert.SerializeObject(AllEmployees)}");
                return View(employerViewModels.ToList());
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new EmployerDTO { });
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployerDTO employerAddOrEditRequests)
        {
            if (!ModelState.IsValid)
            {
                return View(employerAddOrEditRequests);
            }
            else
            {
                try
                {
                    await _employerService.Create(employerAddOrEditRequests);
                    return RedirectToAction("Index");
                }
                catch
                {
                    throw;
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var EmployerForUpdate = await _employerService.GetEmployerById(id);
                return View(new EmployerViewModel
                {
                    Id = EmployerForUpdate.Id,
                    Name = EmployerForUpdate.Name,
                    Email = EmployerForUpdate.Email,
                    Description = EmployerForUpdate.Description,
                    Adresa = EmployerForUpdate.Adresa,
                    PhoneNumber = EmployerForUpdate.PhoneNumber
                });
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployerDTO employerAddOrEditRequests, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(employerAddOrEditRequests);
            }
            try
            {
                await _employerService.Update(employerAddOrEditRequests, id);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var EmployerForDetails = await _employerService.GetEmployerById(id);

                return View(new EmployerViewModel
                {
                    Id = EmployerForDetails.Id,
                    Name = EmployerForDetails.Name,
                    Description = EmployerForDetails.Description,
                    Adresa = EmployerForDetails.Adresa,
                    Email = EmployerForDetails.Email,
                    PhoneNumber = EmployerForDetails.PhoneNumber,
                });
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employerService.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ToggleEmployer(int id)
        {

            var userId = await _userService.GetIdByCurrentUser();
            if (userId != null)
            {
                await _employerToUserService.Toggle(userId, id);
            }
            else
            {
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
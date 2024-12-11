using BLL_Punonjes.Requests.AddOrEditRequests;
using BLL_Punonjes.Services.Scoped;
using Employees.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace Employees.Controllers
{
    public class ReviewController(IReviewService reviewService) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(reviewService.GetAllReviews().Result.Select(
                x => new ReviewViewModels
                {
                    Id = x.Id,
                    Comment = x.Comment,
                    Rate = x.Rate,
                    UserId = x.UserId,
                    UserName = x.UserName
                }).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new ReviewDTO { });
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReviewDTO requests)
        {
            if (!ModelState.IsValid)
            {
                return View(requests);
            }
            else
            {
                try
                {
                    await reviewService.Create(requests);
                }
                catch
                {
                    throw;
                }
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ReviewForUpdate = await reviewService.GetReviewById(id);
                return View(new ReviewViewModels
                {
                    Id = ReviewForUpdate.Id,
                    Comment = ReviewForUpdate.Comment,
                    Rate = ReviewForUpdate.Rate,
                    UserId = ReviewForUpdate.UserId
                });
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ReviewDTO reviewAddOrEditRequests, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(reviewAddOrEditRequests);
            }
            try
            {
                await reviewService.Update(id,reviewAddOrEditRequests);
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
                var ReviewForDetails = await reviewService.GetReviewById(id);

                return View(new ReviewViewModels
                {
                    Id = ReviewForDetails.Id,
                    Rate = ReviewForDetails.Rate,
                    Comment = ReviewForDetails.Comment,
                    UserId = ReviewForDetails.UserId,
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
                await reviewService.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }
    }
}
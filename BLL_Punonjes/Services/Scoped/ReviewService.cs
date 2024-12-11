using BLL_Punonjes.Requests.AddOrEditRequests;
using BLL_Punonjes.Services.Singletone;
using DAL_Punonjes.Entities;
using DAL_Punonjes.Repositories;
using DAL_Punonjes.UNITOFWORK;
using ExtendedNumerics.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Scoped
{
    public interface IReviewService
    {
        Task Create(ReviewDTO reviewAddOrEditRequests);
        Task Update(int id, ReviewDTO reviewAddOrEditRequests);
        Task Delete(int id);
        Task<IEnumerable<Review>> GetAllReviews();
        Task<Review> GetReviewById(int id);
        Task<Review> GetReviewByUserId(int id);
    }
    public class ReviewService : IReviewService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProvaCache _provaCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IProvaCache provaCache,
        Microsoft.AspNetCore.Identity.UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor
        )
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _provaCache = provaCache;
            _httpContextAccessor = httpContextAccessor;
        }
        public delegate void OnReviewAdded(int id);

        public static event OnReviewAdded OnReviewEvent;
        public async Task Create(ReviewDTO reviewAddOrEditRequests)
        {
            var _user = _httpContextAccessor.HttpContext.User ?? throw new Exception("Useri nuk Ekziston");
            var _userIdString = _userManager.GetUserId(_user) ?? throw new Exception("Useri nuk Ekziston");
            var _userIdInt = int.Parse(_userIdString);
            var _userName = _userManager.GetUserName(_user) ?? throw new Exception("Useri nuk Ekziston");
            var NewReview = new Review
            {
                Comment = reviewAddOrEditRequests.Comment,
                Rate = reviewAddOrEditRequests.Rate,
                UserId = _userIdInt,
                UserName = _userName
            };
            await _reviewRepository.Create(NewReview);
            OnReviewEvent?.Invoke(NewReview.Id);
            _provaCache.Remove("Review");
        }
        public async Task Delete(int id)
        {
            var ReviewForDelete = _reviewRepository.GetReviewById(id);
            await _reviewRepository.Delete(ReviewForDelete.Result);
            _provaCache.Remove("Review");
        }
        public async Task<IEnumerable<Review>> GetAllReviews()
        {
            _provaCache.Remove("Review");
            return _provaCache.AddEdit("Review", () => (
                 _reviewRepository.GetAllReview().Result.Select(x =>
                    new Review
                    {
                        Id = x.Id,
                        Comment = x.Comment,
                        Rate = x.Rate,
                        UserId = x.UserId,
                        UserName = x.UserName,
                    }
                ).ToList()
            ));
        }

        public async Task<Review> GetReviewById(int id)
        {
            return await _reviewRepository.GetReviewById(id);
        }

        public async Task<Review> GetReviewByUserId(int id)
        {
            return await _reviewRepository.GetReviewByUserId(id);
        }
        public async Task Update(int id, ReviewDTO reviewAddOrEditRequests)
        {
            var ReviewForDelete = await _reviewRepository.GetReviewById(id);
            ReviewForDelete.Rate = reviewAddOrEditRequests.Rate;
            ReviewForDelete.Comment = reviewAddOrEditRequests.Comment;
            await _unitOfWork.SaveChanges();
            _provaCache.Remove("Review");
        }
    }
}
using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IReviewsLogic : IDisposable
    {
        Task CreateReview(UserReview userReview);

        Task RemoveUserReview(int reviewId);

        Task EditUserReview(EditUserReviewView editedView);

        Task<List<UserReview>> GetAllReviews();

        Task<EditUserReviewView> StartEditUserReview(int? reviewId, string userId);

        Task<UserReview> StartUserReviewRemoving(int? reviewId, string userId, bool isAdmin);
    }
}
using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IReviewsLogic
    {
        Task CreateReview(UserReview userReview);

        Task RemoveUserReview(int reviewId);

        Task EditUserReview(EditUserReviewView editedView);

        Task<UserReview> FindUserReview(int? reviewId);
        Task<IEnumerable<UserReview>> GetUserViews();
    }
}
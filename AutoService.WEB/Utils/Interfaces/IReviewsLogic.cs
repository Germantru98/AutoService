using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IReviewsLogic
    {
        Task CreateReview(CreateReviewView createdView,string userId);
        Task RemoveUserReview(UserReview review);
        Task EditUserReview(EditUserReviewView editedView);
        Task<UserReview> FindUserReview(int? reviewId);

    }
}

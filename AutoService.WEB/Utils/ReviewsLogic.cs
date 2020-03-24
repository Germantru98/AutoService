using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AutoService.WEB.Utils
{
    public class ReviewsLogic : IReviewsLogic
    {
        private ApplicationDbContext _db;
        public ReviewsLogic()
        {

        }
        public ReviewsLogic(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateReview(CreateReviewView createdView, string userId)
        {
            var newReview = new UserReview()
            {
                DateOfCreation = DateTime.Now,
                OwnerId = userId,
                ReviewText = createdView.Text
            };
            _db.UserRewiews.Add(newReview);
            await _db.SaveChangesAsync();
        }

        public async Task EditUserReview(EditUserReviewView editedView)
        {
            if (editedView==null)
            {
                throw new ArgumentNullException($"Ошибка, объект editedView = {editedView}");
            }
            else
            {
                var review = new UserReview(editedView.ReviewId, editedView.EditedText,editedView.UserId);
                _db.Entry(review).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<UserReview> FindUserReview(int? reviewId)
        {
            if (reviewId == null)
            {
                throw new ArgumentNullException($"Ошибка,параметр reviewId = {reviewId}");
            }
            else
            {
                var userReview = await _db.UserRewiews.FindAsync(reviewId);
                if (userReview == null)
                {
                    throw new NullReferenceException($"Объект с reviewId = {reviewId} отсутствует в базе данных");
                }
                else
                {
                    return userReview;
                }
            }
        }

        public async Task RemoveUserReview(UserReview review)
        {
            if (review == null)
            {
                throw new ArgumentNullException("Ошибка,review = null");
            }
            else
            {
                _db.UserRewiews.Remove(review);
                await _db.SaveChangesAsync();
            }
        }
    }
}
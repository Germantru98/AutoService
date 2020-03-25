using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

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

        public async Task CreateReview(UserReview userReview)
        {
            _db.UserRewiews.Add(userReview);
            await _db.SaveChangesAsync();
        }

        public async Task EditUserReview(EditUserReviewView editedView)
        {
            if (editedView == null)
            {
                throw new ArgumentNullException($"Ошибка, объект editedView = {editedView}");
            }
            else
            {
                var review = await _db.UserRewiews.FindAsync(editedView.ReviewId);
                review.ReviewText = editedView.EditedText;
                review.DateOfCreation = DateTime.Now;
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

        public async Task<IEnumerable<UserReview>> GetUserViews()
        {
            var reviews = await _db.UserRewiews.ToListAsync();
            return reviews;
        }

        public async Task RemoveUserReview(int reviewId)
        {
            var review = await _db.UserRewiews.FindAsync(reviewId);
            _db.UserRewiews.Remove(review);
            await _db.SaveChangesAsync();
        }

    }
}
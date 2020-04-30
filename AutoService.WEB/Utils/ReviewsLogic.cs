using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            var review = await _db.UserRewiews.FindAsync(editedView.ReviewId);
            review.ReviewText = editedView.EditedText;
            review.DateOfCreation = DateTime.Now;
            _db.Entry(review).State = EntityState.Modified;
            await _db.SaveChangesAsync();
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
        public async Task<List<UserReview>> GetAllReviews()
        {
            var userReviewsList = await _db.UserRewiews.OrderByDescending(r => r.DateOfCreation).ToListAsync();
            return userReviewsList;
        }

        public async Task<EditUserReviewView> StartEditUserReview(int? reviewId, string userId)
        {
            if (reviewId == null || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Параметр reviewId или userId равен null");
            }
            var review = await _db.UserRewiews.FindAsync(reviewId);
            if (review == null)
            {
                throw new NullReferenceException($"Объект с reviewId = {reviewId} отсутствует в бд");
            }
            if (review.OwnerId != userId)
            {
                throw new Exception("Идентификатор текущего пользователя и владельца не равны");
            }
            return new EditUserReviewView(review.UserReviewId, review.ReviewText);
        }
        public async Task<UserReview> StartUserReviewRemoving(int? reviewId, string userId, bool isAdmin)
        {
            if (reviewId == null || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Параметр reviewId или userId равен null");
            }
            var review = await _db.UserRewiews.FindAsync(reviewId);
            if (review == null)
            {
                throw new NullReferenceException($"Объект с reviewId = {reviewId} отсутствует в бд");
            }
            if (review.OwnerId != userId && !isAdmin)
            {
                throw new Exception("Идентификатор текущего пользователя и владельца не равны");
            }
            return review;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
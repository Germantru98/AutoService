using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    /// <summary>
    /// Класс содержит основную логику для контроллера UserReviews
    /// </summary>
    public class ReviewsLogic : IReviewsLogic
    {
        private ApplicationDbContext _db;
        private bool _disposed = false;
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
        /// <summary>
        /// Изменяет содержимое комментария пользователя
        /// </summary>
        public async Task EditUserReview(EditUserReviewView editedReview)
        {
            var review = await _db.UserRewiews.FindAsync(editedReview.ReviewId);
            review.ReviewText = editedReview.EditedText;
            review.DateOfCreation = DateTime.Now;
            _db.Entry(review).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        /// <summary>
        /// Удаляет отзыв пользователя
        /// </summary>
        /// <param name="reviewId">Идентификатор отзыва</param>
        public async Task RemoveUserReview(int reviewId)
        {
            var review = await _db.UserRewiews.FindAsync(reviewId);
            _db.UserRewiews.Remove(review);
            await _db.SaveChangesAsync();
        }
        /// <summary>
        /// Возвращает список всех отзывов
        /// </summary>
        public async Task<List<UserReview>> GetAllReviews()
        {
            return await _db.UserRewiews.OrderByDescending(review => review.DateOfCreation).ToListAsync();
        }
        /// <summary>
        /// Возвращает модель для редактирования отзыва пользователя. 
        /// Вызывает исключение ArgumentNullException в случае,если отсутствует или равен null один из параметров.
        /// Вызывает исключение NullReferenceException, если объект с указанным reviewId отсутствует в базе данных.
        /// Вызывает исключение InvalidOperationException, если идентификатор владельца отзыва и текущего пользователя не совпадают.
        /// </summary>
        /// <param name="reviewId">Идентификатор отзыва</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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
                throw new InvalidOperationException("Идентификатор текущего пользователя и владельца не совпадают");
            }
            return new EditUserReviewView(review.UserReviewId, review.ReviewText);
        }
        /// <summary>
        /// Возвращает модель для удаления отзыва пользователя. 
        /// Вызывает исключение ArgumentNullException в случае,если отсутствует или равен null один из параметров.
        /// Вызывает исключение NullReferenceException, если объект с указанным reviewId отсутствует в базе данных.
        /// Вызывает исключение InvalidOperationException, если идентификатор владельца отзыва и текущего пользователя не совпадают или пользователь не имеет роли администратора.
        /// </summary>
        /// <param name="reviewId">Идентификатор отзыва</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="isAdmin">Флаг администратора</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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
                throw new InvalidOperationException("Идентификатор текущего пользователя и владельца не совпадают");
            }
            return review;
        }



        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
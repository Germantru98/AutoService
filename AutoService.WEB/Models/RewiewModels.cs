using System;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class UserReview
    {
        [Key]
        public int UserReviewId { get; set; }

        public string ReviewText { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string OwnerId { get; set; }
        public string UserName { get; set; }

        public UserReview()
        {
        }

        public UserReview(string text, string ownerId, string userName)
        {
            ReviewText = text;
            OwnerId = ownerId;
            DateOfCreation = DateTime.Now;
            UserName = userName;
        }

        public UserReview(int reviewId, string text, string ownerId, string userName)
        {
            UserReviewId = reviewId;
            ReviewText = text;
            OwnerId = ownerId;
            DateOfCreation = DateTime.Now;
            UserName = userName;
        }
    }

    public class CreateReviewView
    {
        [Required(ErrorMessage = "Поле с отзывом пользователя не может быть пустым")]
        public string Text { get; set; }
    }

    public class EditUserReviewView
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Поле с отзывом пользователя не может быть пустым")]
        public string EditedText { get; set; }
    }
}
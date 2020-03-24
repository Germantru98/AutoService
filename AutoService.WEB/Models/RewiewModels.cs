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
        public UserReview()
        {

        }
        public UserReview(string text, string ownerId)
        {
            ReviewText = text;
            OwnerId = ownerId;
            DateOfCreation = DateTime.Now;

        }
        public UserReview(int reviewId,string text, string ownerId)
        {
            UserReviewId = reviewId;
            ReviewText = text;
            OwnerId = ownerId;
            DateOfCreation = DateTime.Now;
        }
    }
    public class UserReviewView
    {
        public string UserName { get; set; }
        public string ReviewText { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
    public class CreateReviewView 
    {
        [Required(ErrorMessage ="Поле с отзывом пользователя не может быть пустым")]
        public string Text { get; set; }
    }
    public class EditUserReviewView
    {
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public string EditedText { get; set; }
    }
}
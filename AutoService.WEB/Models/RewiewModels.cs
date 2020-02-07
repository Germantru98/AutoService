using System;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class UserRewiew
    {
        public int UserRewiewId { get; set; }

        [Required(ErrorMessage = "Данное поле не может быть пустым")]
        public string RewiewText { get; set; }

        public DateTime Date { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
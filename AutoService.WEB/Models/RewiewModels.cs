using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoService.WEB.Models
{
  public class UserRewiew
    {
        public int UserRewiewId { get; set; }
        [Required(ErrorMessage ="Данное поле не может быть пустым")]
        public string RewiewText { get; set; }
        public DateTime Date { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
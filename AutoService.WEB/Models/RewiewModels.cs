using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoService.WEB.Models
{
  public class UserRewiew
    {
        public int UserRewiewId { get; set; }
        public string RewiewText { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
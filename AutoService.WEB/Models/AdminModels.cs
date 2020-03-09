using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class AdminMenuView
    {
        public List<UserAdminView> Users { get; set; }
        public List<ServiceView> Discounts { get; set; }
    }

    public class UserAdminView
    {
        public string UserRealName { get; set; }
        public string UserMail { get; set; }
        public string PhoneNumber { get; set; }

        public UserAdminView(string realName, string userMail, string phoneNumber)
        {
            UserRealName = realName;
            UserMail = userMail;
            PhoneNumber = phoneNumber;
        }
    }
    public class AddNewDiscount
    {
        [Required]
        public int DiscountValue { get; set; }
        [Required]
        public int ServiceId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FinishDate { get; set; }

    }
    public class ExtendDiscount
    {
        public int? DiscountId { get; set; }
        public int Days { get; set; }
        public ExtendDiscount()
        {

        }
        public ExtendDiscount(int? id, int days)
        {
            DiscountId = id;
            Days = days;
        }
    }
}
using System.Collections.Generic;

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
}
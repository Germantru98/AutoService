using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class AdminMenuView
    {
        public List<ServiceView> Discounts { get; set; }
        public List<ServicesSummaryAdminView> CurrentOrders { get; set; }
        public List<ServicesSummaryAdminView> Archive { get; set; }
        public SettingsView SettingsView { get; set; }

        public AdminMenuView(List<ServiceView> discounts, List<ServicesSummaryAdminView> currentOrders, List<ServicesSummaryAdminView> archive, SettingsView settingsView)
        {
            Discounts = discounts;
            CurrentOrders = currentOrders;
            Archive = archive;
            SettingsView = settingsView;
        }
    }

    public class SettingsView
    {
        public List<CarBrand> CarBrands { get; set; }

        public SettingsView(List<CarBrand> carBrands)
        {
            CarBrands = carBrands;
        }
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
        [Range(1, 100, ErrorMessage = "Не указано значение")]
        [Display(Name = "Размер скидки")]
        public int DiscountValue { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Не указана дата начала действия акции")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'mm'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Не указана дата окончания действия акции")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'mm'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FinishDate { get; set; }
    }

    public class ExtendDiscount
    {
        public int? DiscountId { get; set; }

        [Required(ErrorMessage = "Введите количество дней")]
        [Range(1, 1000, ErrorMessage = "Количество дней должно превышать 0")]
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
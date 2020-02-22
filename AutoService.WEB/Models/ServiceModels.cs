using System;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public int Price { get; set; }
        public string ServiceImageHref { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
    }

    public class ServiceView
    {
        public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Название услуги")]
        public string ServiceName { get; set; }

        [Required]
        [Display(Name = "Цена на услугу")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Ссылка на изображение")]
        public string ServiceImageHref { get; set; }
    }

    public class Discount
    {
        public int DiscountId { get; set; }
        public int Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool isActive { get; set; }
    }
}
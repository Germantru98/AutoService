using System;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class Service : IComparable<Service>
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public int Price { get; set; }
        public string ServiceImageHref { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public int Counter { get; set; }

        public int CompareTo(Service other)
        {
            if (Counter < other.Counter)
            {
                return 1;
            }
            else if (Counter > other.Counter)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class ServiceView
    {
        [ScaffoldColumn(false)]
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

        [ScaffoldColumn(false)]
        public Discount Discount { get; set; }

        public ServiceView(int serviceId, string name, int price, string imageHref, Discount discount)
        {
            ServiceName = name;
            Price = price;
            ServiceImageHref = imageHref;
            ServiceId = serviceId;
            Discount = discount;
        }
    }

    public class ServiceLiteView
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public Discount Discount { get; set; }

        public ServiceLiteView()
        {
        }

        public ServiceLiteView(int id, string serviceName, Discount discount)
        {
            ServiceId = id;
            ServiceName = serviceName;
            Discount = discount;
        }
    }

    public class Discount
    {
        public int DiscountId { get; set; }
        public int Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public void SetNewFinishDate(int daysCount)
        {
            FinishDate = FinishDate.AddDays(daysCount);
        }

        public bool isRelevant()
        {
            if (FinishDate >= DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DiscountView
    {
        [ScaffoldColumn(false)]
        public int DiscountId { get; set; }

        [Display(Name = "Размер скидки:")]
        public int Value { get; set; }

        [Display(Name = "Дата начала:")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Дата окончания:")]
        public DateTime FinishDate { get; set; }

        public DiscountView()
        {
        }

        public DiscountView(int id, int value, DateTime startDate, DateTime finishDate)
        {
            DiscountId = id;
            Value = value;
            StartDate = startDate;
            FinishDate = finishDate;
        }
    }
}
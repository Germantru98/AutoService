using System;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class Service : IComparable<Service>
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public int Price { get; set; }
        public int PriceWithDiscount { get; set; }
        public string ServiceImageHref { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public Service()
        {
        }

        public Service(string serviceName, int price, string serviceImageHref)
        {
            ServiceName = serviceName;
            Price = price;
            PriceWithDiscount = price;
            ServiceImageHref = serviceImageHref;
        }

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

        public override string ToString()
        {
            if (Discount == null)
            {
                return $"{ServiceName} Цена: {Price} ₽";
            }
            else
            {
                return $"{ServiceName} Цена: {PriceWithDiscount} ₽ Скидка: {Discount.Value}%";
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
        public int PriceWithDiscount { get; set; }
        [Required]
        [Display(Name = "Ссылка на изображение")]
        public string ServiceImageHref { get; set; }

        [ScaffoldColumn(false)]
        public Discount Discount { get; set; }

        public bool DiscountRelevancy { get; set; }
        public ServiceView()
        {
        }

        public ServiceView(int serviceId, string serviceName, int price, int priceWithDiscount, string serviceImageHref, Discount discount)
        {
            ServiceId = serviceId;
            ServiceName = serviceName;
            Price = price;
            PriceWithDiscount = priceWithDiscount;
            ServiceImageHref = serviceImageHref;
            Discount = discount;
            DiscountRelevancy = false;
        }
    }

    public class EditServiceView
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int Price { get; set; }
        public string ServiceImageHref { get; set; }

        public EditServiceView()
        {
        }

        public EditServiceView(int serviceId, string serviceName, int price, string imageHref)
        {
            ServiceId = serviceId;
            ServiceName = serviceName;
            Price = price;
            ServiceImageHref = imageHref;
        }
    }

    public class RemoveDiscountFromServiceView
    {
        public string ServiceName { get; set; }
        public Discount Discount { get; set; }

        public RemoveDiscountFromServiceView()
        {
        }

        public RemoveDiscountFromServiceView(string serviceName, Discount discount)
        {
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
            var now = DateTime.Today;
            if (now >= StartDate && now <= FinishDate || now <= StartDate && now <= FinishDate)
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
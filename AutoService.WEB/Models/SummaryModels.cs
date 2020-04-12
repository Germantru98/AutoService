using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class ServicesSummary
    {
        [Key]
        public int SummaryId { get; set; }

        public string UserId { get; set; }
        public int? UserCarId { get; set; }
        public string ServiceList { get; set; }
        public DateTime DateOfCreating { get; set; }
        public DateTime DayOfWork { get; set; }
        public int TotalPrice { get; set; }
    }

    public class ServicesSummaryView
    {
        public IEnumerable<Service> ServicesList { get; set; }
        public int TotalPrice { get; set; }

        [Required(ErrorMessage = "Не выбран автомобиль")]
        [Display(Name = "Выбор автомобиля: ")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Не выбрана дата начала работ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'mm'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата оказания выбранных услуг")]
        public DateTime selectedDateTime { get; set; }
    }

    public class ServicesSummaryAdminView
    {
        public ApplicationUser User { get; set; }
        public string ServicesList { get; set; }
        public int TotalPrice { get; set; }
        public CarView Car { get; set; }

        public DateTime Date { get; set; }

        public ServicesSummaryAdminView()
        {
        }

        public ServicesSummaryAdminView(ApplicationUser user,string servicesList, int totalPrice, CarView car, DateTime date)
        {
            User = user;
            ServicesList = servicesList;
            TotalPrice = totalPrice;
            Car = car;
            Date = date;
        }
    }
}
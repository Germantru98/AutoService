using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        public bool IsCompleted { get; set; }

        public ServicesSummary()
        {
        }
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
        [Display(Name = "Дата оказания выбранных услуг")]
        public DateTime selectedDateTime { get; set; }

        public ServicesSummaryView()
        {
        }

        public ServicesSummaryView(IEnumerable<Service> servicesList, int totalPrice, int carId, DateTime selectedDateTime)
        {
            ServicesList = servicesList;
            TotalPrice = totalPrice;
            CarId = carId;
            this.selectedDateTime = selectedDateTime;
        }
    }

    public class ServicesSummaryAdminView
    {
        public int SummaryId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Service> ServicesList { get; set; }
        public int TotalPrice { get; set; }
        public CarView Car { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }

        public ServicesSummaryAdminView()
        {
        }

        public ServicesSummaryAdminView(int summaryId, ApplicationUser user, List<Service> servicesList, int totalPrice, CarView car, DateTime date, bool status)
        {
            SummaryId = summaryId;
            User = user;
            ServicesList = servicesList;
            TotalPrice = totalPrice;
            Car = car;
            Date = date;
            IsCompleted = status;
        }
    }

    public class CompletedSummariesHistory
    {
        [Key]
        public int Id { get; set; }

        public int SummaryId { get; set; }
        public DateTime DateOfCompletion { get; set; }
    }

    public class EditSummaryView
    {
        public int SummaryId { get; set; }
        public int UserCarId { get; set; }

        [Required(ErrorMessage = "Не указана дата")]
        [DataType(DataType.Date)]
        public DateTime DayOfWork { get; set; }

        public IList<string> SelectedServices { get; set; }
        public IList<SelectListItem> AllServices { get; set; }

        public EditSummaryView()
        {
        }

        public EditSummaryView(int summaryId, int userCarId, DateTime dayOfWork, string[] servisesId, List<SelectListItem> allServices)
        {
            SummaryId = summaryId;
            UserCarId = userCarId;
            DayOfWork = dayOfWork;
            SelectedServices = servisesId;
            AllServices = allServices;
        }
    }

    public class GetPeriodView
    {
        [Display(Name = "Начало периода")]
        [Required(ErrorMessage = "Не указана дата начала периода")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'mm'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Конец периода")]
        [Required(ErrorMessage = "Не указана дата окончания периода")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'mm'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FinishDate { get; set; }
    }

    public class SelectDateView
    {
        [Required(ErrorMessage = "Не указана дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'mm'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }

    public class UserOrderView
    {
        public int OrderId { get; set; }
        public List<Service> ServicesList { get; set; }
        public int TotalPrice { get; set; }
        public CarView UserCar { get; set; }
        public string selectedDateTime { get; set; }

        public UserOrderView()
        {
        }

        public UserOrderView(int orderId, List<Service> servicesList, int totalPrice, CarView userCar, DateTime selectedDateTime)
        {
            OrderId = orderId;
            ServicesList = servicesList;
            TotalPrice = totalPrice;
            UserCar = userCar;
            this.selectedDateTime = selectedDateTime.ToShortDateString();
        }
    }
}
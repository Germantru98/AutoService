using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Название услуги")]
        public string ServiceName { get; set; }

        [Required]
        [Display(Name = "Цена на услугу")]
        public int Price { get; set; }
        [Required]
       [Display(Name ="Ссылка на изображение")]
        public string ServiceImageHref { get; set; }
    }
}
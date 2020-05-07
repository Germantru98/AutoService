using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoService.WEB.Models
{
    public class HomeView
    {
        public List<CarBrand> CarBrands { get; set; }
        public List<ServiceView> Discounts { get; set; }
        public List<HomeMainCarouselItem> HomeMainCarouselItems { get; set; }
    }

    public class CarBrand
    {
        [Key]
        public int BrandId { get; set; }

        public string BrandName { get; set; }
        public string ImageHref { get; set; }
    }

    public class HomeMainCarouselItem
    {
        [ForeignKey("News")]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageHref { get; set; }
        public virtual News News { get; set; }
        public HomeMainCarouselItem()
        {
        }
    }

    public class EditCarouselItemView
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Заголовок слайда")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание слайда")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Ссылка на изображение слайда")]
        public string ImageHref { get; set; }

        [Required]
        [Display(Name = "Номер новости для перехода")]
        public int? NewsId { get; set; }
    }

    public class AddNewCarouselItemView
    {
        [Required]
        [Display(Name = "Заголовок слайда")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание слайда")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Ссылка на изображение слайда")]
        public string ImageHref { get; set; }

        [Required]
        [Display(Name = "Переход из слайда")]
        public int NewsId { get; set; }
    }

    public class HomeMainCarouselItemView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageHref { get; set; }
        public int? NewsId { get; set; }

        public HomeMainCarouselItemView()
        {
        }

        public HomeMainCarouselItemView(int id, string title, string description, string imageHref, int? newsId)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageHref = imageHref;
            NewsId = newsId;
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class HomeView
    {
        public List<Service> PopularServices { get; set; }
        public List<CarBrand> CarBrands { get; set; }
        public List<Service> Discounts { get; set; }
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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageHref { get; set; }
        public string RouteHref { get; set; }
    }
}
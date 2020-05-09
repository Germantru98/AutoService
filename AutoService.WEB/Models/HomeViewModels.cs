using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoService.WEB.Models
{
    public class HomeView
    {
        public List<CarBrand> CarBrands { get; set; }
        public List<ServiceView> Discounts { get; set; }
        public List<Slide> HomeMainCarouselItems { get; set; }
    }

    public class CarBrand
    {
        [Key]
        public int BrandId { get; set; }

        public string BrandName { get; set; }
        public string ImageHref { get; set; }
    }
}
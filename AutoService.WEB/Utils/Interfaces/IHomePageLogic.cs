using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IHomePageLogic
    {
        Task<List<HomeMainCarouselItem>> GetMainCarousel();
        Task AddNewMainCarouselItem(AddNewCarouselItemView newItem);
        Task RemoveMainCarouselItem(int? itemId);
        Task EditMainCarouselItem(EditCarouselItemView editView);
        Task<HomeMainCarouselItemView> FindCarouselItem(int? itemId);
    }
}

using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IHomePageLogic : IDisposable
    {
        Task<List<Slide>> GetMainCarousel();

        Task<List<CarBrand>> GetCarBrandsCarousel();

        Task RemoveCarBrand(int? brandId);

        Task AddNewCarBrand(CarBrand newCarBrand);

        Task<CarBrand> FindCarBrand(int? brandId);

        Task<List<ServiceView>> GetRelevantDiscountsForHomePage();
    }
}
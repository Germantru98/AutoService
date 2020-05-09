using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class HomePageLogic : IHomePageLogic
    {
        private ApplicationDbContext _db;
        private IServicesLogic _servicesLogic;
        private INewsPageLogic _newsPageLogic;

        public HomePageLogic(ApplicationDbContext db, IServicesLogic servicesLogic, INewsPageLogic newsPageLogic)
        {
            _db = db;
            _servicesLogic = servicesLogic;
            _newsPageLogic = newsPageLogic;
        }

        public async Task<List<Slide>> GetMainCarousel()
        {
            return await _newsPageLogic.GetNewsSlides();
        }

        public async Task<List<CarBrand>> GetCarBrandsCarousel()
        {
            return await _db.CarBrands.ToListAsync();
        }

        public async Task RemoveCarBrand(int? brandId)
        {
            if (brandId == null)
            {
                throw new ArgumentNullException("brandId == null");
            }
            var brand = await _db.CarBrands.FindAsync(brandId);
            if (brand == null)
            {
                throw new NullReferenceException($"Объект с brandId = {brandId} отсутствует в базе данных");
            }
            _db.CarBrands.Remove(brand);
            await _db.SaveChangesAsync();
        }

        public async Task AddNewCarBrand(CarBrand newCarBrand)
        {
            _db.CarBrands.Add(newCarBrand);
            await _db.SaveChangesAsync();
        }

        public async Task<CarBrand> FindCarBrand(int? brandId)
        {
            if (brandId == null)
            {
                throw new ArgumentNullException("brandId ==null");
            }
            var brand = await _db.CarBrands.FindAsync(brandId);
            if (brand == null)
            {
                throw new NullReferenceException($"CarBrand c brandId = {brandId} отсутствует в бд");
            }
            return brand;
        }

        public async Task<List<ServiceView>> GetRelevantDiscountsForHomePage()
        {
            var servicesWithDiscount = await _db.Services.Include(s => s.Discount).ToListAsync();
            var result = new List<ServiceView>();
            foreach (var item in servicesWithDiscount)
            {
                if (item.Discount != null)
                {
                    if (item.Discount.isRelevant())
                    {
                        result.Add(_servicesLogic.MapServiceToServiceView(item));
                    }
                }
            }
            return result;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                    _servicesLogic.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
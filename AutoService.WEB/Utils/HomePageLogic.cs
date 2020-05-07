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
        private ServicesLogic _servicesLogic;

        public HomePageLogic(ApplicationDbContext db, ServicesLogic servicesLogic)
        {
            _db = db;
            _servicesLogic = servicesLogic;
        }

        public async Task AddNewMainCarouselItem(AddNewCarouselItemView item)
        {
            var newItem = MapNewCarouselItemToMainHomeCarousleItem(item);
            _db.HomeMainCarouselItems.Add(newItem);
            await _db.SaveChangesAsync();
        }

        public async Task EditMainCarouselItem(EditCarouselItemView editView)
        {
            var editedItem = await _db.HomeMainCarouselItems.FindAsync(editView.Id);
            if (editedItem == null)
            {
                throw new NullReferenceException($"Объект с Id = {editView.Id} отсутствует в базе данных");
            }
            editedItem.ImageHref = editView.ImageHref;
            editedItem.Title = editView.Title;
            editedItem.Description = editView.Description;
            _db.Entry(editedItem).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<HomeMainCarouselItemView> FindCarouselItem(int? itemId)
        {
            if (itemId == null)
            {
                throw new ArgumentNullException($"Параметр itemId = null");
            }
            else
            {
                var carouselItemFromDb = await _db.HomeMainCarouselItems.FindAsync(itemId);
                if (carouselItemFromDb == null)
                {
                    throw new NullReferenceException($"Объект с itemId = {itemId} отсутствует в базе данных");
                }
                else
                {
                    var result = MapCarousleItemToCarousleItemView(carouselItemFromDb);
                    return result;
                }
            }
        }

        public async Task<List<HomeMainCarouselItem>> GetMainCarousel()
        {
            var result = await _db.HomeMainCarouselItems.ToListAsync();
            return result;
        }

        public async Task RemoveMainCarouselItem(int? itemId)
        {
            if (itemId == null)
            {
                throw new ArgumentNullException("itemId = null");
            }
            var item = await _db.HomeMainCarouselItems.FindAsync(itemId);
            if (item == null)
            {
                throw new NullReferenceException($"Объект с указанным itemId = {itemId} отсутствует в базе данных");
            }
            _db.HomeMainCarouselItems.Remove(item);
            await _db.SaveChangesAsync();
        }

        private HomeMainCarouselItem MapNewCarouselItemToMainHomeCarousleItem(AddNewCarouselItemView newItem)
        {
            var result = new HomeMainCarouselItem() 
            {
                Title = newItem.Title,
                Description = newItem.Description,
                ImageHref = newItem.ImageHref
            };
            return result;
        }

        private HomeMainCarouselItemView MapCarousleItemToCarousleItemView(HomeMainCarouselItem item)
        {
            return new HomeMainCarouselItemView() 
            {
               Id= item.Id,
               Title = item.Title,
               Description = item.Description,
               ImageHref = item.ImageHref
            };

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
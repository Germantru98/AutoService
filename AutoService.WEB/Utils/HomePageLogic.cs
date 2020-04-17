using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AutoService.WEB.Utils
{
    public class HomePageLogic : IHomePageLogic
    {
        private ApplicationDbContext _db;

        public HomePageLogic(ApplicationDbContext db)
        {
            _db = db;
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
            editedItem.RouteHref = editView.RouteHref;
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
            if (itemId==null)
            {
                throw new ArgumentNullException("itemId = null");
            }
            var item =await _db.HomeMainCarouselItems.FindAsync(itemId);
            if (item == null)
            {
                throw new NullReferenceException($"Объект с указанным itemId = {itemId} отсутствует в базе данных");
            }
            _db.HomeMainCarouselItems.Remove(item);
            await _db.SaveChangesAsync();
        }
        private HomeMainCarouselItem MapNewCarouselItemToMainHomeCarousleItem(AddNewCarouselItemView newItem)
        {
            return new HomeMainCarouselItem(newItem.Title,newItem.Description,newItem.ImageHref,newItem.RouteHref);
        }
        private HomeMainCarouselItemView MapCarousleItemToCarousleItemView(HomeMainCarouselItem item)
        {
            return new HomeMainCarouselItemView(item.Id, item.Title, item.Description, item.ImageHref, item.RouteHref);
        }
    }
}
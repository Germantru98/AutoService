using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class UserLogic : IUserLogic
    {
        private ApplicationDbContext _db;
        private IServicesLogic _servicesLogic;

        public UserLogic()
        {
        }

        public UserLogic(ApplicationDbContext db, IServicesLogic servicesLogic)
        {
            _db = db;
            _servicesLogic = servicesLogic;
        }

        public async Task AddToBasket(int? serviceId, string userId)
        {
            if (serviceId == null)
            {
                throw new ArgumentNullException("Отсутствует id услуги");
            }
            else
            {
                var service = await _db.Services.FindAsync(serviceId);
                if (service == null)
                {
                    throw new NullReferenceException($"Услуга с serviceId = {serviceId} отсутсвует в базе данных");
                }
                else
                {
                    _db.BasketItems.Add(new BasketItem((int)serviceId, userId));
                    await _db.SaveChangesAsync();
                }
            }
        }

        public int GetTotalPrice(IEnumerable<Service> items)
        {
            int totalPrice = 0;
            foreach (var item in items)
            {
                if (item.Discount == null)
                {
                    totalPrice += item.Price;
                }
                else
                {
                    totalPrice += item.PriceWithDiscount;
                }
            }
            return totalPrice;
        }

        public async Task RemoveFromBasket(int itemId)
        {
            BasketItem item = await _db.BasketItems.FindAsync(itemId);
            _db.BasketItems.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAllItemsFromBasket(string userId)
        {
            var items = _db.BasketItems.Where(item => item.UserId == userId);
            _db.BasketItems.RemoveRange(items);
            await _db.SaveChangesAsync();
        }

        public async Task<Dictionary<int, ServiceView>> GetUserShopCart(string userId)
        {
            var shopCartItems = await _db.BasketItems.Where(s => s.UserId == userId).ToListAsync();
            var result = new Dictionary<int, ServiceView>();
            foreach (var item in shopCartItems)
            {
                var tmpService = await _db.Services.Include(s => s.Discount).FirstAsync(s => s.ServiceId == item.ServiceId);
                result.Add(item.Id, _servicesLogic.MapServiceToServiceView(tmpService));
            }
            return result;
        }

        public async Task<ServiceView> StartRemoveFromUserShopCart(int? itemId, string userId)
        {
            if (itemId == null)
            {
                throw new ArgumentNullException("itemId = null");
            }
            var shopCartItem = await _db.BasketItems.FindAsync(itemId);
            if (shopCartItem == null)
            {
                throw new NullReferenceException($"basketItem с itemId = {itemId} отсутсвует в бд");
            }
            if (shopCartItem.UserId != userId)
            {
                throw new Exception("Элемент корзины имеет другого владельца");
            }
            var service = await _db.Services.Include(s => s.Discount).FirstAsync(s => s.ServiceId == shopCartItem.ServiceId);
            return _servicesLogic.MapServiceToServiceView(service);
        }
    }
}
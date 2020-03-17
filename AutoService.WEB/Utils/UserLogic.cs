using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace AutoService.WEB.Utils
{
    public class UserLogic : IUserLogic
    {
        private ApplicationDbContext _db;

        public UserLogic()
        {
        }

        public UserLogic(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNewCar(AddNewCarViewModel newCar, string userId)
        {
            var car = new Car { Color = newCar.CarColor, Model = newCar.CarModel, Year = newCar.CarYear, ApplicationUserId = userId, };
            _db.Cars.Add(car);
            await _db.SaveChangesAsync();
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

        public  int GetTotalPrice(IEnumerable<Service> items)
        {
            int totalPrice =0;
            foreach (var item in items)
            {
                if (item.Discount==null)
                {
                    totalPrice += item.Price;
                }
                else 
                {
                    totalPrice += GetPriceWithDiscount(item.Price, item.Discount.Value);
                }
            }
            return totalPrice;
        }

        public async Task RemoveCar(int carId)
        {
            Car car = await _db.Cars.FindAsync(carId);
            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFromBasket(int itemId)
        {
            BasketItem item = await _db.BasketItems.FindAsync(itemId);
            _db.BasketItems.Remove(item);
            await _db.SaveChangesAsync();
        }

        private async Task<Service> GetService(int serviceId)
        {
            return await _db.Services.Include(s=>s.Discount).FirstOrDefaultAsync(s=>s.ServiceId==serviceId);
        }
        private int GetPriceWithDiscount(int price, int discountValue)
        {
            return price - price / 100 * (discountValue);
        }

        public async Task RemoveAllItemsFromBasket(string userId)
        {
            var items =  _db.BasketItems.Where(item => item.UserId == userId);
            _db.BasketItems.RemoveRange(items);
            await _db.SaveChangesAsync();
        }
    }
}
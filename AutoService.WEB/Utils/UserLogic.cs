using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Threading.Tasks;

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

        public Task GetTotalPrice()
        {
            throw new NotImplementedException();
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
    }
}
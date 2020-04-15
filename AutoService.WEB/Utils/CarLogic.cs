using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class CarLogic : ICarLogic
    {
        private ApplicationDbContext _db;

        public CarLogic(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<CarView>> GetAllUserCars(string userId)
        {
            var resultFromDb = await _db.Cars.Where(c => c.ApplicationUserId == userId).ToListAsync();
            var result = new List<CarView>();
            foreach (var item in resultFromDb)
            {
                var car = MapCarToCarView(item);
                result.Add(car);
            }
            return result;
        }

        public CarView MapCarToCarView(Car car)
        {
            return new CarView(car.Id, car.Model, car.Color, car.Year, car.CarImageHref);
        }
    }
}
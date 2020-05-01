using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
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

        public async Task AddNewUserCar(string userId, AddNewCarViewModel newCar)
        {
            var user = _db.Users.Find(userId);
            var newCarsStorageItem = new CarsStorageItem()
            {
                Car = new Car()
                {
                    Model = newCar.CarModel,
                    Color = newCar.CarColor,
                    Year = newCar.CarYear,
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    CarImageHref = newCar.CarImgHref
                },
                UserId = user.Id
            };
            _db.UsersCarsStorage.Add(newCarsStorageItem);
            await _db.SaveChangesAsync();
        }

        public async Task EditUserCar(EditCarView editedCar)
        {
            var car = await _db.Cars.FindAsync(editedCar.CarId);
            if (car == null)
            {
                throw new NullReferenceException($"Объект с carId = {editedCar.CarId} отсутствует в бд");
            }
            car.Color = editedCar.Color;
            car.Model = editedCar.Model;
            car.Year = editedCar.Year;
            car.CarImageHref = editedCar.CarImgHref;
            _db.Entry(car).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<List<CarView>> GetAllUserCars(string userId)
        {
            var userCarsFromStorage = await _db.UsersCarsStorage.Where(item => item.UserId == userId).ToListAsync();
            var result = new List<CarView>();
            foreach (var item in userCarsFromStorage)
            {
                var tmpCar = await _db.Cars.FindAsync(item.CarId);
                result.Add(MapCarToCarView(tmpCar));
            }
            return result;
        }

        public async Task<List<CarView>> GetAllUsersCarsFromDb()
        {
            var resultFromDb = await _db.Cars.ToListAsync();
            List<CarView> result = new List<CarView>();
            foreach (var car in resultFromDb)
            {
                result.Add(MapCarToCarView(car));
            }
            return result;
        }

        public async Task<CarView> GetUserCar(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("carId равно null");
            }
            var car = await _db.Cars.FindAsync(id);
            if (car == null)
            {
                throw new NullReferenceException("Объект с указанным carId отсутствует в бд");
            }
            return MapCarToCarView(car);
        }

        public CarView MapCarToCarView(Car car)
        {
            return new CarView(car.Id, car.Model, car.Color, car.Year, car.CarImageHref);
        }

        public async Task RemoveUserCar(int carId, string userId)
        {
            var itemFromUsersCarsStorage = await _db.UsersCarsStorage.FirstAsync(item => item.CarId == carId);
            if (itemFromUsersCarsStorage == null)
            {
                throw new NullReferenceException("Объект с указанным carId отсутствует в бд");
            }
            if (itemFromUsersCarsStorage.UserId != userId)
            {
                throw new Exception("id владельца авто и текущего пользователя не совпадают");
            }
            _db.UsersCarsStorage.Remove(itemFromUsersCarsStorage);
            await _db.SaveChangesAsync();
        }

        public async Task<EditCarView> StartEditUserCarOperation(int? carId, string userId)
        {
            if (carId == null || userId == null)
            {
                throw new ArgumentNullException("carId или userId равно null");
            }
            var car = await _db.Cars.FindAsync(carId);
            if (car == null)
            {
                throw new NullReferenceException($"Авто с carId = {carId} отсутствует в бд");
            }
            if (car.ApplicationUserId != userId)
            {
                throw new Exception("id владельца авто и текущего пользователя не совпадают");
            }
            return new EditCarView(car.Id, car.Model, car.Color, car.Year, car.CarImageHref);
        }

        public async Task<DeleteCarView> StartRemoveUserCarOperation(int? carId, string userId)
        {
            if (carId == null || userId == null)
            {
                throw new ArgumentNullException("carId или userId равно null");
            }
            var car = await _db.Cars.FindAsync(carId);
            if (car == null)
            {
                throw new NullReferenceException($"Авто с carId = {carId} отсутствует в бд");
            }
            if (car.ApplicationUserId != userId)
            {
                throw new Exception("id владельца авто и текущего пользователя не совпадают");
            }
            return new DeleteCarView((int)carId, userId);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
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
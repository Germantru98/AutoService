using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface ICarLogic
    {
        CarView MapCarToCarView(Car car);

        Task<List<CarView>> GetAllUserCars(string userId);
        Task RemoveUserCar(int carId,string userId);
        Task EditUserCar(EditCarView editedCar);
        Task AddNewUserCar(string userId, AddNewCarViewModel newCar);
        Task<List<CarView>> GetAllUsersCarsFromDb();
        Task<EditCarView> StartEditUserCarOperation(int? carId, string userId);
        Task<DeleteCarView> StartRemoveUserCarOperation(int? carId, string userId);
        Task<CarView> GetUserCar(int? id);
    }
}
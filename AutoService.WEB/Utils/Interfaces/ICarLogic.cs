using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface ICarLogic
    {
        CarView MapCarToCarView(Car car);

        Task<List<CarView>> GetAllUserCars(string userId);
    }
}
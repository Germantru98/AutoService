using AutoService.WEB.Models;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface ICarLogic
    {
        CarView MapCarToCarView(Car car);
    }
}
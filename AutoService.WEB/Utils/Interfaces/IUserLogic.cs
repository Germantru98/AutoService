using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IUserLogic
    {
        Task AddToBasket(int? serviceId, string userId);

        Task RemoveFromBasket(int itemId);

        Task RemoveAllItemsFromBasket(string userId);

        Task AddNewCar(AddNewCarViewModel newCar, string userId);

        Task RemoveCar(int id);

        int GetTotalPrice(IEnumerable<Service> items);
    }
}
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

        int GetTotalPrice(IEnumerable<Service> items);

        Task<Dictionary<int, ServiceView>> GetUserShopCart(string userId);

        Task<ServiceView> StartRemoveFromUserShopCart(int? itemId, string userId);
    }
}
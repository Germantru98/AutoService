using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IServicesLogic
    {
        List<ServiceView> GetServices(IEnumerable<Service> services);

        int GetNewPriceWithDiscount(int oldPrice, int discountValue);

        Task AddNewService(Service newService);

        Task EditService(EditServiceView editedService);

        Task<Service> FindService(int? serviceId);

        Task RemoveService(int serviceId);
    }
}
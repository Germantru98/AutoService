using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IServicesLogic : IDisposable
    {
        Task<List<ServiceView>> GetAllServicesWithDiscount();

        IEnumerable<Service> GetServicesFromDb();

        Task AddNewService(Service newService);

        Task EditService(EditServiceView editedService);

        Task<Service> FindService(int? serviceId);

        Task RemoveService(int serviceId);

        Task<Discount> FindDiscount(int? discountId);

        Task<Service> FindServiceWithDiscount(int? serviceId);

        Task AddDiscount(AddNewDiscount newDiscount);

        Task RemoveDiscount(int? discountId);

        Task ExtendDiscount(ExtendDiscount extendDiscountItem);

        Task<List<Service>> GetServicesFromSummary(string serviceList);

        ServiceView MapServiceToServiceView(Service service);

        Task<List<ServiceView>> GetServicesSortedByPrice();

        Task<List<ServiceView>> GetServicesSortedByDiscount();

        Task<List<ServiceView>> GetAllServices();

        Task<List<ServiceView>> SearchServicesByName(string name);

        string ServicesToString(List<Service> services);
    }
}
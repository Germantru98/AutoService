using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System.Collections.Generic;

namespace AutoService.WEB.Utils
{
    public class ServicesLogic : IServicesLogic
    {
        public List<ServiceView> GetServices(IEnumerable<Service> services)
        {
            var result = new List<ServiceView>();
            foreach (var service in services)
            {
                result.Add(new ServiceView(service.ServiceId, service.ServiceName, service.Price, service.ServiceImageHref, service.Discount));
            }
            return result;
        }
    }
}
using AutoService.WEB.Models;
using System.Collections.Generic;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IServicesLogic
    {
        List<ServiceView> GetServices(IEnumerable<Service> services);
    }
}
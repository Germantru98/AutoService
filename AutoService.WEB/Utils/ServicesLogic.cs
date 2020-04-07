using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class ServicesLogic : IServicesLogic
    {
        private ApplicationDbContext _db;

        public ServicesLogic()
        {
        }

        public ServicesLogic(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNewService(Service newService)
        {
            _db.Services.Add(newService);
            await _db.SaveChangesAsync();
        }

        public async Task EditService(EditServiceView editedService)
        {
            var service = await _db.Services.FindAsync(editedService.ServiceId);
            service.ServiceName = editedService.ServiceName;
            service.Price = editedService.Price;
            service.ServiceImageHref = editedService.ServiceImageHref;
            _db.Entry(service).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<Service> FindService(int? serviceId)
        {
            if (serviceId == null)
            {
                throw new ArgumentNullException($"Параметр serviceId = {serviceId}");
            }
            else
            {
                var service = await _db.Services.FindAsync(serviceId);
                if (service == null)
                {
                    throw new NullReferenceException($"Объект с serviceId = {serviceId} отсутствует в базе данных");
                }
                else
                {
                    return service;
                }
            }
        }

        public int GetNewPriceWithDiscount(int oldPrice, int discountValue)
        {
            return oldPrice - oldPrice / 100 * (discountValue);
        }

        public List<ServiceView> GetServices(IEnumerable<Service> services)
        {
            var result = new List<ServiceView>();
            foreach (var service in services)
            {
                result.Add(new ServiceView(service.ServiceId, service.ServiceName, service.Price, service.ServiceImageHref, service.Discount));
            }
            return result;
        }

        public async Task RemoveService(int serviceId)
        {
            Service service = await _db.Services.FindAsync(serviceId);
            _db.Services.Remove(service);
            await _db.SaveChangesAsync();
        }
    }
}
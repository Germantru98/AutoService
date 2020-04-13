﻿using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public async Task AddDiscount(AddNewDiscount newDiscount)
        {
            var service = await _db.Services.FindAsync(newDiscount.ServiceId);
            service.Discount = new Discount()
            {
                Value = newDiscount.DiscountValue,
                FinishDate = newDiscount.FinishDate,
                StartDate = newDiscount.StartDate
            };
            service.PriceWithDiscount = GetNewPriceWithDiscount(service.Price, service.Discount.Value);
            _db.Entry(service).State = EntityState.Modified;
            await _db.SaveChangesAsync();
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

        public async Task<Discount> FindDiscount(int? discountId)
        {
            if (discountId == null)
            {
                throw new ArgumentNullException($"Параметр discountId = {discountId}");
            }
            else
            {
                var discount = await _db.Discounts.FindAsync(discountId);
                if (discount == null)
                {
                    throw new NullReferenceException($"Объект с discountId = {discountId} отсутствует в базе данных");
                }
                else
                {
                    return discount;
                }
            }
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

        public async Task<Service> FindServiceWithDiscount(int? serviceId)
        {
            if (serviceId == null)
            {
                throw new ArgumentNullException($"Параметр serviceId = {serviceId}");
            }
            else
            {
                var service = await _db.Services.Include(s => s.Discount).FirstOrDefaultAsync(s => s.ServiceId == serviceId);
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

        private int GetNewPriceWithDiscount(int oldPrice, int discountValue)
        {
            return oldPrice - oldPrice / 100 * (discountValue);
        }

        public async Task<List<ServiceView>> GetAllServicesWithDiscount()
        {
            var result = new List<ServiceView>();
            var services = await _db.Services.Include(s => s.Discount).Where(s => s.DiscountId != null).ToListAsync();
            foreach (var service in services)
            {
                result.Add(new ServiceView(service.ServiceId, service.ServiceName, service.Price, service.ServiceImageHref, service.Discount));
            }
            return result;
        }

        public IEnumerable<Service> GetServicesFromDb()
        {
            return _db.Services;
        }

        public async Task RemoveDiscount(int discountId)
        {
            var discount = await _db.Discounts.FindAsync(discountId);
            _db.Discounts.Remove(discount);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveService(int serviceId)
        {
            Service service = await _db.Services.FindAsync(serviceId);
            _db.Services.Remove(service);
            await _db.SaveChangesAsync();
        }

        public async Task ExtendDiscount(ExtendDiscount extendDiscountItem)
        {
            var discount = await _db.Discounts.FindAsync(extendDiscountItem.DiscountId);
            discount.SetNewFinishDate(extendDiscountItem.Days);
            _db.Entry(discount).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<List<Service>> GetServicesFromSummary(string serviceList)
        {
            var result = new List<Service>();
            var servicesId = serviceList.Split('|');
            foreach (var id in servicesId)
            {
                var tmpService = await FindServiceWithDiscount(int.Parse(id));
                result.Add(tmpService);
            }
            return result;
        }
    }
}
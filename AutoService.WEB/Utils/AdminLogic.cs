using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class AdminLogic : IAdminLogic
    {
        private ApplicationDbContext _db;
        private IServicesLogic _servicesLogic;

        public AdminLogic()
        {
        }

        public AdminLogic(ApplicationDbContext db, IServicesLogic servicesLogic)
        {
            _db = db;
            _servicesLogic = servicesLogic;
        }

        public async Task<AdminMenuView> GetAdminMenuView(string adminId)
        {
            var adminView = new AdminMenuView()
            {
                Users = new List<UserAdminView>(),
                Discounts = await _servicesLogic.GetAllServicesWithDiscount(),
                ServicesSummaries = await GetAllSummaries()
            };
            var admin = _db.Users.Find(adminId);
            foreach (var user in await _db.Users.ToListAsync())
            {
                if (user.UserName != admin.UserName)
                {
                    adminView.Users.Add(new UserAdminView(user.RealName, user.Email, user.PhoneNumber));
                }
            }
            return adminView;
        }

        public async Task<List<ServicesSummaryAdminView>> GetAllSummaries()
        {
            var summariesFromDb = await _db.ServicesSummaries.ToListAsync();
            List<ServicesSummaryAdminView> result = new List<ServicesSummaryAdminView>();
            foreach (var summary in summariesFromDb)
            {
                var tmpUser = _db.Users.Find(summary.UserId);
                var tmpCarFromSummary = MapCarToCarView(await _db.Cars.FindAsync(summary.UserCarId));
                var tmpSummary = new ServicesSummaryAdminView(tmpUser,summary.ServiceList, summary.TotalPrice, tmpCarFromSummary, summary.DayOfWork);
                result.Add(tmpSummary);
            }
            return result;
        }

        public Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task RemoveUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UserDetails(string userId)
        {
            throw new NotImplementedException();
        }
        private CarView MapCarToCarView(Car car)
        {
            return new CarView(car.Model, car.Color, car.Year, car.CarImageHref);
        }
    }
}
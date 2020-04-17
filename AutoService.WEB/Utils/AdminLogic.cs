using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AutoService.WEB.Utils
{
    public class AdminLogic : IAdminLogic
    {
        private ApplicationDbContext _db;
        private IServicesLogic _servicesLogic;
        private ISummariesLogic _summariesLogic;
        private IHomePageLogic _homePageLogic;
        public AdminLogic()
        {
        }

        public AdminLogic(ApplicationDbContext db, IServicesLogic servicesLogic, ISummariesLogic summariesLogic, IHomePageLogic homePageLogic)
        {
            _db = db;
            _servicesLogic = servicesLogic;
            _summariesLogic = summariesLogic;
            _homePageLogic = homePageLogic;
        }

        public async Task<AdminMenuView> GetAdminMenuView()
        {
            var currentOrders = await _summariesLogic.GetCurrentSummaries();
            var archive = await _summariesLogic.GetCompletedSummaries();
            var discounts = await _servicesLogic.GetAllServicesWithDiscount();
            var mainHomeCarousel = await _homePageLogic.GetMainCarousel();
            var adminView = new AdminMenuView(discounts, currentOrders, archive,mainHomeCarousel);
            return adminView;
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
    }
}
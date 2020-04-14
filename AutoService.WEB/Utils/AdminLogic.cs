using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class AdminLogic : IAdminLogic
    {
        private ApplicationDbContext _db;
        private IServicesLogic _servicesLogic;
        private ISummariesLogic _summariesLogic;

        public AdminLogic()
        {
        }

        public AdminLogic(ApplicationDbContext db, IServicesLogic servicesLogic, ISummariesLogic summariesLogic)
        {
            _db = db;
            _servicesLogic = servicesLogic;
            _summariesLogic = summariesLogic;
        }

        public async Task<AdminMenuView> GetAdminMenuView()
        {
            var currentOrders = await _summariesLogic.GetCurrentSummaries();
            var archive = await _summariesLogic.GetCompletedSummaries();
            var discounts = await _servicesLogic.GetAllServicesWithDiscount();
            var adminView = new AdminMenuView(discounts, currentOrders, archive);
            return adminView;
        }

        public async Task<List<ServicesSummaryAdminView>> GetAllSummaries()
        {
            return await _summariesLogic.GetCompletedSummaries();
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
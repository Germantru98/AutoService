using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface ISummariesLogic : IDisposable
    {
        Task<ServicesSummaryAdminView> FindSummaryById(int? summaryId);

        Task AddNewSummary(ServicesSummary newSummary);

        Task RemoveSummary(int summaryId, string userId, bool isAdmin);

        Task EditSummary(EditSummaryView editedSummary);

        Task<List<ServicesSummaryAdminView>> GetCompletedSummaries();

        Task<List<ServicesSummaryAdminView>> GetCurrentSummaries();

        Task<List<ServicesSummaryAdminView>> GetAllUncompletedSummaries();

        Task<List<ServicesSummaryAdminView>> GetCompletedSummariesByPeriod(DateTime start, DateTime finish);

        Task<List<ServicesSummaryAdminView>> GetCompletedSummariesByDate(DateTime date);

        Task CompleteSummary(int summaryId);

        Task<EditSummaryView> GetEditSummaryView(ServicesSummaryAdminView view);

        Task<EditSummaryView> GetEditSummaryView(UserOrderView view);

        ServicesSummaryView GetServicesSummaryView(IEnumerable<Service> shopCart, int totalPrice);

        Task<List<UserOrderView>> GetAllUserOrders(string userId);

        Task<UserOrderView> GetUserOrder(int? orderId, string userId);
    }
}
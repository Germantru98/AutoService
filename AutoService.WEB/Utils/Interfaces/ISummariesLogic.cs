using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface ISummariesLogic
    {
        Task<ServicesSummaryAdminView> FindSummaryById(int? summaryId);

        Task RemoveSummary(int summaryId);

        Task EditSummary(EditSummaryView editedSummary);

        Task<List<ServicesSummaryAdminView>> GetCompletedSummaries();

        Task<List<ServicesSummaryAdminView>> GetCurrentSummaries();

        Task<List<ServicesSummaryAdminView>> GetAllUncompletedSummaries();

        Task<List<ServicesSummaryAdminView>> GetCompletedSummariesByPeriod(DateTime start, DateTime finish);

        Task<List<ServicesSummaryAdminView>> GetCompletedSummariesByDate(DateTime date);

        Task CompleteSummary(int summaryId);

        Task<EditSummaryView> GetEditSummaryView(ServicesSummaryAdminView view);
    }
}
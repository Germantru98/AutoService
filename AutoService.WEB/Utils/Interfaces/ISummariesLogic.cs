using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.WEB.Models;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface ISummariesLogic
    {
        Task<ServicesSummary> FindSummaryById(int? summaryId);
        Task RemoveSummary(int summaryId);
        Task EditSummary(EditSummaryView editedSummary);
        Task<List<ServicesSummaryAdminView>> GetCompletedSummaries();
        Task<List<ServicesSummaryAdminView>> GetCurrentSummaries();
        Task<List<ServicesSummaryAdminView>> GetSummariesByPeriod(DateTime start, DateTime finish);
        Task<List<ServicesSummaryAdminView>> GetSummariesByDate(DateTime date);
        Task CompleteSummary(int summaryId);
    }
}

using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IAdminLogic
    {
        Task RemoveUser(string userId);

        Task UserDetails(string userId);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();

        Task<AdminMenuView> GetAdminMenuView(string adminId);

        Task<List<ServicesSummaryAdminView>> GetAllSummaries();
    }
}
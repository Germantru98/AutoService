using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IAdminLogic : IDisposable
    {
        Task RemoveUser(string userId);

        Task UserDetails(string userId);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();

        Task<AdminMenuView> GetAdminMenuView();
    }
}
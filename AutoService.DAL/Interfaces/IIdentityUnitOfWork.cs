using AutoService.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace AutoService.DAL.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }

        Task SaveAsync();
    }
}
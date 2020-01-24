using AutoService.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace AutoService.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ContactData> ContactDatas { get; }

        Task SaveAsync();
    }
}
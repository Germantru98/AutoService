using AutoService.DAL.Entities;
using System;

namespace AutoService.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
    }
}
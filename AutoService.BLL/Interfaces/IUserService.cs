using AutoService.BLL.DTO;
using AutoService.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoService.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);

        Task<ClaimsIdentity> Authenticate(UserDTO userDto);

        Task SetInitialData(UserDTO adminDto, List<string> roles);
    }
}
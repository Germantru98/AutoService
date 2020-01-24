using AutoService.BLL.DTO;
using System.Collections.Generic;

namespace AutoService.BLL.Interfaces
{
    public interface IContactData
    {
        ContactDataDTO GetContactData(int? id);

        IEnumerable<ContactDataDTO> GetContactDatas();

        void Dispose();
    }
}
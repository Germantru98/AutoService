using AutoService.BLL.DTO;
using AutoService.DAL.Entities;
using System;
using System.Collections.Generic;

namespace AutoService.BLL.Interfaces
{
    public interface IContactData
    {
        ContactDataDTO GetContactData(int? id);

        IEnumerable<ContactDataDTO> GetContactDatas();

        IEnumerable<ContactDataDTO> GetActiveContactDatas(Func<ContactData, Boolean> predicate);

        IEnumerable<ContactDataDTO> GetActiveMails();

        IEnumerable<ContactDataDTO> GetActivePhones();

        void Dispose();
    }
}
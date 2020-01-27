﻿using AutoMapper;
using AutoService.BLL.DTO;
using AutoService.BLL.Interfaces;
using AutoService.WEB.Models;
using System.Collections.Generic;

namespace AutoService.WEB.Utils
{
    public class ContactDataUnit
    {
        private IContactData contactData;

        public ContactDataUnit(IContactData icd)
        {
            contactData = icd;
        }

        public ContactDataView GetContactData(int? id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ContactDataDTO, ContactDataView>()).CreateMapper();
            return mapper.Map<ContactDataDTO, ContactDataView>(contactData.GetContactData(id));
        }

        public IEnumerable<ContactDataView> GetMails()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ContactDataDTO, ContactDataView>()).CreateMapper();
            return mapper.Map<IEnumerable<ContactDataDTO>, List<ContactDataView>>(contactData.GetActiveMails());
        }

        public IEnumerable<ContactDataView> GetPhones()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ContactDataDTO, ContactDataView>()).CreateMapper();
            return mapper.Map<IEnumerable<ContactDataDTO>, List<ContactDataView>>(contactData.GetActivePhones());
        }
    }
}
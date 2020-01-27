using AutoMapper;
using AutoService.BLL.DTO;
using AutoService.BLL.Interfaces;
using AutoService.DAL.Entities;
using AutoService.DAL.Interfaces;
using CarService.BLL.Infrastructure;
using System;
using System.Collections.Generic;

namespace AutoService.BLL.Services
{
    public class ContactDataService : IContactData
    {
        private IUnitOfWork Database;

        public ContactDataService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public ContactDataDTO GetContactData(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id контактной информации", "");
            var contactData = Database.ContactDatas.Get(id.Value);
            if (contactData == null)
                throw new ValidationException("Контактная информация не найдена", "");

            return new ContactDataDTO { Id = contactData.Id, Name = contactData.Name, Value = contactData.Value, isActive = contactData.isActive };
        }

        public IEnumerable<ContactDataDTO> GetContactDatas()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ContactData, ContactDataDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<ContactData>, List<ContactDataDTO>>(Database.ContactDatas.GetAll());
        }

        public IEnumerable<ContactDataDTO> GetActiveMails()
        {
            return GetActiveContactDatas(x => x.Name == "mail" && x.isActive);
        }

        public IEnumerable<ContactDataDTO> GetActiveContactDatas(Func<ContactData, bool> predicate)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ContactData, ContactDataDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<ContactData>, List<ContactDataDTO>>(Database.ContactDatas.Find(predicate));
        }

        public IEnumerable<ContactDataDTO> GetActivePhones()
        {
            return GetActiveContactDatas(x => x.Name == "phone" && x.isActive);
        }
    }
}
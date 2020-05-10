using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IContactInfoLogic : IDisposable
    {
        Task<ContactView> getContactView();

        Task<List<ContactItem>> getAllContactItems();

        Task AddItem(AddNewContactView item);

        Task RemoveItem(int? id);

        Task SwitchStatus(int? id);

        Task<ContactItem> FindItem(int? itemId);

        string[] GetContactItemsTypes();

        Task<EditContactInformationView> GetEditContactInformationView();
    }
}
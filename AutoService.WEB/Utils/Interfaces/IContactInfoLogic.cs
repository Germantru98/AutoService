using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IContactInfoLogic
    {
        Task<ContactView> getContactView();

        Task<List<ContactItem>> getAllContactItems();

        Task AddItem(ContactItem item);

        Task RemoveItem(int? id);

        Task SwitchStatus(int? id);

        Task<ContactItem> FindItem(int? itemId);

        string[] GetContactItemsTypes();
    }
}
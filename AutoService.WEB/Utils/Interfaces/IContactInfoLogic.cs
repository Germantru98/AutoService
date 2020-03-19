using AutoService.WEB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils.Interfaces
{
    public interface IContactInfoLogic
    {
        Task<ContactView> getContactView();

        Task<List<ContactItem>> getAllContactItems();

        Task EditItem(int? id, string data);

        Task AddItem(string type, string data);

        Task RemoveItem(int? id);

        Task SwitchStatus(int? id);

        string[] GetContactItemsTypes();
    }
}
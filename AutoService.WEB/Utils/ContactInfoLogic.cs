using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.WEB.Utils
{
    public class ContactInfoLogic : IContactInfoLogic
    {
        private ApplicationDbContext _db;

        public ContactInfoLogic(ApplicationDbContext db)
        {
            _db = db;
        }

        public ContactInfoLogic()
        {
        }

        public async Task<ContactView> getContactView()
        {
            var contactItems = await _db.Contacts.Where(c => c.isActive).ToListAsync();
            ContactView contactView = new ContactView()
            {
                ContactEmails = new List<string>(),
                ContactPhones = new List<string>()
            };
            foreach (var item in contactItems)
            {
                switch (item.Name)
                {
                    case "Address":
                        contactView.Address = item.Value;
                        break;

                    case "Email":
                        contactView.ContactEmails.Add(item.Value);
                        break;

                    case "Phone":
                        contactView.ContactPhones.Add(item.Value);
                        break;

                    case "Vk":
                        contactView.VKHref = item.Value;
                        break;

                    case "Facebook":
                        contactView.FacebookHref = item.Value;
                        break;

                    case "Instagram":
                        contactView.InstagramHref = item.Value;
                        break;

                    case "VkImage":
                        contactView.VKImageHref = item.Value;
                        break;

                    case "FacebookImage":
                        contactView.FacebookImageHref = item.Value;
                        break;

                    case "InstagramImage":
                        contactView.InstagramImageHref = item.Value;
                        break;

                    case "Yandexframe":
                        contactView.YandexFrameSrc = item.Value;
                        break;

                    default:
                        break;
                }
            }
            return contactView;
        }

        public async Task<List<ContactItem>> getAllContactItems()
        {
            var contactItems = await _db.Contacts.ToListAsync();
            if (contactItems == null)
            {
                throw new NullReferenceException("Ошибка, объект List<ContactItem> = null");
            }
            else
            {
                return contactItems;
            }
        }

        public async Task AddItem(ContactItem item)
        {
            _db.Contacts.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveItem(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"Ошибка,параметр id = {id}");
            }
            else
            {
                var contactItem = await _db.Contacts.FindAsync(id);
                if (contactItem == null)
                {
                    throw new NullReferenceException($"Объект с id = {id} отсутствует в базе данных");
                }
                else
                {
                    _db.Contacts.Remove(contactItem);
                    await _db.SaveChangesAsync();
                }
            }
        }

        public string[] GetContactItemsTypes()
        {
            string[] result = { "Email", "Phone", "Vk", "Facebook", "Instagram",
                "Yandexframe", "Address", "VkImage", "FacebookImage", "InstagramImage" };
            return result;
        }

        public async Task SwitchStatus(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"Ошибка,параметр id = {id}");
            }
            else
            {
                var contactItem = await _db.Contacts.FindAsync(id);
                if (contactItem == null)
                {
                    throw new NullReferenceException($"Объект с id = {id} отсутствует в базе данных");
                }
                else
                {
                    contactItem.isActive = contactItem.isActive ? false : true;
                    _db.Entry(contactItem).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
            }
        }

        public async Task<ContactItem> FindItem(int? itemId)
        {
            if (itemId == null)
            {
                throw new ArgumentNullException($"Ошибка, itemId ={itemId} ");
            }
            else
            {
                var result = await _db.Contacts.FindAsync(itemId);
                if (result == null)
                {
                    throw new NullReferenceException($"Ошибка, объект с itemId = {itemId} отсутствует в базе данных");
                }
                else
                {
                    return result;
                }
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
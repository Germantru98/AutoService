using AutoService.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class ContactInfoController : Controller
    {
        // GET: ContactInfo
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var contactItems = _dbContext.Contacts.Where(c => c.isActive).ToList();
            ContactView contactView = new ContactView()
            {
                ContactEmails = new List<string>(),
                ContactPhones = new List<string>()
            };
            foreach (var item in contactItems)
            {
                switch (item.Name)
                {
                    case "Address" :
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
            return View(contactView);
        }
    }
}
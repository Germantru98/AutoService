using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class ContactInfoController : Controller
    {
        // GET: ContactInfo
        private ApplicationDbContext _dbContext;

        private IContactInfoLogic _contactInfoLogic;

        public ContactInfoController()
        {
        }

        public ContactInfoController(ApplicationDbContext db, IContactInfoLogic logic)
        {
            _dbContext = db;
            _contactInfoLogic = logic;
        }

        public async Task<ActionResult> Index()
        {
            var contactView = await _contactInfoLogic.getContactView();

            return View(contactView);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddNewInfo()
        {
            SelectList types = new SelectList(_contactInfoLogic.GetContactItemsTypes(), "type");
            ViewBag.Types = types;
            return View(new AddNewContactView());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewInfo(AddNewContactView newContactItem)
        {
            if (ModelState.IsValid)
            {
                var item = new ContactItem(newContactItem.Type, newContactItem.Value, true);
                _dbContext.Contacts.Add(item);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            SelectList types = new SelectList(_contactInfoLogic.GetContactItemsTypes(), "type");
            ViewBag.Types = types;
            return View(new AddNewContactView());
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditContacts()
        {
            EditContactInformationView view = new EditContactInformationView()
            {
                ContactItems = await _dbContext.Contacts.ToListAsync()
            };
            return View(view);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SwitchStatus(int? itemId)
        {
            try
            {
                await _contactInfoLogic.SwitchStatus(itemId);
                return RedirectToAction("EditContacts");
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveItem(int? itemId)
        {
            try
            {
                await _contactInfoLogic.RemoveItem(itemId);
                return RedirectToAction("EditContacts");
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditItem(int? itemId, string data)
        {
            try
            {
                await _contactInfoLogic.EditItem(itemId, data);
                return RedirectToAction("EditContacts");
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }
    }
}
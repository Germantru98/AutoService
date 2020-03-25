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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewInfo(AddNewContactView newContactItem)
        {
            var item = new ContactItem(newContactItem.Type, newContactItem.Value, true);
            if (item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            await _contactInfoLogic.AddItem(item);
            return RedirectToAction("EditContacts");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditContacts()
        {
            EditContactInformationView view = new EditContactInformationView()
            {
                ContactItems = await _dbContext.Contacts.ToListAsync(),
                AddNewContactView = new AddNewContactView()
            };
            ViewBag.Types = new SelectList(_contactInfoLogic.GetContactItemsTypes(), "type");
            ViewBag.EditContactsView = view;
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
                var contactItem = await _contactInfoLogic.FindItem(itemId);
                var contacItemView = new ContactItemView(contactItem.Name, contactItem.Value);
                return PartialView("RemoveItemModalView", contacItemView);
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
        [HttpPost, ActionName("RemoveItem")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveConfirmed(int itemId)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
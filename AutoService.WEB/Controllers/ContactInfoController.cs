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
            if (ModelState.IsValid)
            {
                var item = new ContactItem(newContactItem.Type, newContactItem.Value, true);
                _dbContext.Contacts.Add(item);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
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
        public async Task<ActionResult> EditItem(int? itemId)
        {

            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var contactItem = await _dbContext.Contacts.FindAsync(itemId);
                if (contactItem == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    EditContactItemView view = new EditContactItemView()
                    {
                        ItemId = itemId,
                        Type = contactItem.Name
                    };
                    ViewBag.EditView = view;
                    return View(view);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditItem(EditContactItemView editedContactItem)
        {
            if (editedContactItem==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        await _contactInfoLogic.EditItem(editedContactItem.ItemId, editedContactItem.EditedValue);
                        return RedirectToAction("EditContacts");
                    }
                    else
                    {
                        return View((EditContactItemView)ViewBag.Views);
                    }
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
}
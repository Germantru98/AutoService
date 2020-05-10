using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class ContactInfoController : Controller
    {
        // GET: ContactInfo

        private IContactInfoLogic _contactInfoLogic;

        public ContactInfoController()
        {
        }

        public ContactInfoController(IContactInfoLogic logic)
        {
            _contactInfoLogic = logic;
        }

        public async Task<ActionResult> Index()
        {
            var contactView = await _contactInfoLogic.getContactView();
            return View(contactView);
        }

        public enum Messages
        {
            Error,
            AddNewInfoSucces,
            RemoveInfoSuccess,
            SwitchSuccess
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddNewInfo()
        {
            ViewBag.Types = new SelectList(_contactInfoLogic.GetContactItemsTypes(), "type");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewInfo(AddNewContactView newContactItem)
        {
            if (ModelState.IsValid)
            {
                await _contactInfoLogic.AddItem(newContactItem);
                return RedirectToAction("EditContacts", new { message = Messages.AddNewInfoSucces });
            }
            return RedirectToAction("EditContacts", new { message = Messages.Error });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditContacts(Messages? message)
        {
            ViewBag.StatusMessage = MessageGenerator(message);
            EditContactInformationView view = await _contactInfoLogic.GetEditContactInformationView();
            return View(view);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SwitchStatus(int? itemId)
        {
            try
            {
                await _contactInfoLogic.SwitchStatus(itemId);
                return RedirectToAction("EditContacts", new { message = Messages.SwitchSuccess });
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("EditContacts", new { message = Messages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("EditContacts", new { message = Messages.Error });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveItem(int? itemId)
        {
            try
            {
                var contactItem = await _contactInfoLogic.FindItem(itemId);
                return PartialView("RemoveItemModalView", contactItem);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("EditContacts", new { message = Messages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("EditContacts", new { message = Messages.Error });
            }
        }

        [HttpPost, ActionName("RemoveItem")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveConfirmed(int itemId)
        {
            try
            {
                await _contactInfoLogic.RemoveItem(itemId);
                return RedirectToAction("EditContacts", new { message = Messages.RemoveInfoSuccess });
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("EditContacts", new { message = Messages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("EditContacts", new { message = Messages.Error });
            }
        }

        private string MessageGenerator(Messages? message)
        {
            return message == Messages.AddNewInfoSucces ? "Информация добавлена"
               : message == Messages.RemoveInfoSuccess ? "Информация удалена"
               : message == Messages.Error ? "Ошибка"
               : message == Messages.SwitchSuccess ? "Статус изменен"
               : null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contactInfoLogic.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
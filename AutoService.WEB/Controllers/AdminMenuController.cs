using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMenuController : Controller
    {
        private IServicesLogic _servicesLogic;
        private IAdminLogic _adminLogic;

        public AdminMenuController()
        {
        }

        public AdminMenuController(IServicesLogic logic, IAdminLogic adminLogic)
        {
            _servicesLogic = logic;
            _adminLogic = adminLogic;
        }

        // GET: AdminMenu
        public async Task<ActionResult> Index()
        {
            var adminId = User.Identity.GetUserId();
            var indexView = await _adminLogic.GetAdminMenuView(adminId);
            return View("AdminMenu", indexView);
        }

        public async Task<ActionResult> RemoveDiscount(int? serviceId)
        {
            try
            {
                var service = await _servicesLogic.FindServiceWithDiscount(serviceId);
                return PartialView("ConfirmDiscountDeleteView", new RemoveDiscountFromServiceView(service.ServiceName, service.Discount));
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

        [HttpPost, ActionName("RemoveDiscount")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveDiscountConfirmed(int? serviceId)
        {
            var service = await _servicesLogic.FindServiceWithDiscount(serviceId);
            await _servicesLogic.RemoveDiscount(service.Discount.DiscountId);
            return RedirectToAction("Index");
        }

        public ActionResult AddNewDiscount()
        {
            SelectList services = new SelectList(_servicesLogic.GetServicesFromDb(), "ServiceId", "ServiceName");
            ViewBag.Services = services;
            return View("AddNewDiscountWindow");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewDiscount(AddNewDiscount newDiscount)
        {
            if (ModelState.IsValid)
            {
                await _servicesLogic.AddDiscount(newDiscount);
                return RedirectToAction("Index");
            }
            SelectList services = new SelectList(_servicesLogic.GetServicesFromDb(), "ServiceId", "ServiceName");
            ViewBag.Services = services;
            return View("AddNewDiscountWindow");
        }

        public async Task<ActionResult> ExtendDiscount(int? id)
        {
            try
            {
                var discount = await _servicesLogic.FindDiscount(id);
                ExtendDiscount extendDiscount = new ExtendDiscount(id, 1);
                return PartialView(extendDiscount);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExtendDiscount(ExtendDiscount extendDiscountItem)
        {
            if (ModelState.IsValid)
            {
                await _servicesLogic.ExtendDiscount(extendDiscountItem);
                return RedirectToAction("Index");
            }
            return PartialView(extendDiscountItem);
        }
    }
}
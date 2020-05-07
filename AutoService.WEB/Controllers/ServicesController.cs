using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class ServicesController : Controller
    {
        private ApplicationDbContext _db;
        private IServicesLogic _servicesLogic;

        public ServicesController()
        {
        }

        public ServicesController(ApplicationDbContext db, IServicesLogic servicesLogic)
        {
            _db = db;
            _servicesLogic = servicesLogic;
        }

        // GET: Services
        [AllowAnonymous]
        public async Task<ActionResult> Index(Messages? message)
        {
            var services = await _servicesLogic.GetAllServices();
            ViewBag.StatusMessage = MessageGenerator(message);
            return View(services);
        }

        public enum Messages
        {
            UpdateSucces,
            CreateNewServiceSucces,
            DeleteSuccess,
            Error
        }

        private string MessageGenerator(Messages? message)
        {
            return message == Messages.UpdateSucces ? "Услуга успешно обновлена"
                : message == Messages.UpdateSucces ? "Новая услуга успешно создана"
                : message == Messages.DeleteSuccess ? "Услуга успешно удалена"
                : message == Messages.Error ? "Ошибка"
                : null;
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return PartialView("AddNewService");
        }

        // POST: Services/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ServiceView serviceView)
        {
            if (ModelState.IsValid)
            {
                var service = new Service(serviceView.ServiceName, serviceView.Price, serviceView.ServiceImageHref);
                await _servicesLogic.AddNewService(service);
                return RedirectToAction("Index", new { message = Messages.CreateNewServiceSucces });
            }
            return View(serviceView);
        }

        // GET: Services/Edit/5
        public async Task<ActionResult> Edit(int? serviceId)
        {
            try
            {
                var service = await _servicesLogic.FindService(serviceId);
                var editServiceView = new EditServiceView(service.ServiceId, service.ServiceName, service.Price, service.ServiceImageHref);
                return PartialView(editServiceView);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
        }

        // POST: Services/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditServiceView service)
        {
            if (ModelState.IsValid)
            {
                await _servicesLogic.EditService(service);
                return RedirectToAction("Index", new { message = Messages.UpdateSucces });
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<ActionResult> Delete(int? serviceId)
        {
            try
            {
                var service = await _servicesLogic.FindService(serviceId);
                return PartialView(service);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int serviceId)
        {
            await _servicesLogic.RemoveService(serviceId);
            return RedirectToAction("Index", new { message = Messages.DeleteSuccess });
        }

        public async Task<ActionResult> SearchServicesByName(string serviceName)
        {
            var services = await _servicesLogic.SearchServicesByName(serviceName);
            if (services.Count <= 0)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", new { message = Messages.Error });
        }

        public async Task<ActionResult> GetServicesSortedByDiscount()
        {
            var sortedServices = await _servicesLogic.GetServicesSortedByDiscount();
            return PartialView("SortedServices", sortedServices);
        }

        public async Task<ActionResult> GetServicesSortedByPrice()
        {
            var sortedServices = await _servicesLogic.GetServicesSortedByPrice();
            return PartialView("SortedServices", sortedServices);
        }

        public async Task<ActionResult> GetAllServices()
        {
            var services = await _servicesLogic.GetAllServices();
            return PartialView("SortedServices", services);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
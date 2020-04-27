using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class ServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IUserLogic _userLogic;
        private IServicesLogic _servicesLogic;

        public ServicesController()
        {
        }

        public ServicesController(IUserLogic userLogic, IServicesLogic servicesLogic)
        {
            _userLogic = userLogic;
            _servicesLogic = servicesLogic;
        }
        // GET: Services
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var services = await _servicesLogic.GetAllServices();
            return View(services);
        }

        // GET: Services/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
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
                return RedirectToAction("Index");
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
                return View(editServiceView);
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
                return RedirectToAction("Index");
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int serviceId)
        {
            await _servicesLogic.RemoveService(serviceId);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AddToBasket(int? serviceId)
        {
            try
            {
                await _userLogic.AddToBasket(serviceId, User.Identity.GetUserId());
                return PartialView("SuccessView");
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
            catch (Exception)
            {
                return PartialView("ExistsInUserShopCartModalView");
            }
        }

        public async Task<ActionResult> SearchServicesByName(string serviceName)
        {
            var services = await _servicesLogic.SearchServicesByName(serviceName);
            if (services.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView("ServicesSearch", services);
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
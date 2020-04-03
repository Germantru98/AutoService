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

        public ServicesController()
        {
        }

        public ServicesController(IUserLogic logic)
        {
            _userLogic = logic;
        }

        public enum SortState
        {
            NameAsc,    // по имени по возрастанию
            NameDesc,   // по имени по убыванию
            PriceAsc,
            PriceDesc,
            WithDiscount,
            WithoutDiscount
        }

        // GET: Services
        [AllowAnonymous]
        public async Task<ActionResult> Index(SortState sortOrder = SortState.NameAsc)
        {
            var services = db.Services.Include(s => s.Discount);

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["PriceSort"] = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ViewData["DiscountSort"] = sortOrder == SortState.WithDiscount ? SortState.WithoutDiscount : SortState.WithDiscount;

            if (sortOrder == SortState.PriceAsc)
            {
                services = services.OrderBy(s => s.Price);
            }
            else if (sortOrder == SortState.PriceDesc)
            {
                services = services.OrderByDescending(s => s.Price);
            }
            else if (sortOrder == SortState.NameAsc)
            {
                services = services.OrderBy(s => s.ServiceName);
            }
            else if (sortOrder == SortState.NameDesc)
            {
                services = services.OrderByDescending(s => s.ServiceName);
            }
            else if (sortOrder == SortState.WithDiscount)
            {
                services = services.Where(s => s.Discount != null);
            }
            else
            {
                services = services.Where(s => s.Discount == null);
            }
            return View(await services.AsNoTracking().ToListAsync());
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
        public async Task<ActionResult> Create([Bind(Include = "ServiceId,ServiceName,Price,ServiceImageHref")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(service);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

        // POST: Services/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceId,ServiceName,Price,ServiceImageHref")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Service service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
            await db.SaveChangesAsync();
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
        }

        public ActionResult SearchServicesByName(string serviceName)
        {
            var services = db.Services.Where(s => s.ServiceName.Contains(serviceName)).ToList();
            if (services.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView("ServicesSearch", services);
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
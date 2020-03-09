using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMenuController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private IServicesLogic _servicesLogic;

        public AdminMenuController(IServicesLogic logic)
        {
            _servicesLogic = logic;
        }

        // GET: AdminMenu
        public async Task<ActionResult> Index()
        {
            var adminView = new AdminMenuView()
            {
                Users = new List<UserAdminView>(),
                Discounts = _servicesLogic.GetServices(await _dbContext.Services.Include(s => s.Discount).Where(s => s.DiscountId != null).ToListAsync())
            };
            var admin = _dbContext.Users.Find(User.Identity.GetUserId());
            foreach (var user in await _dbContext.Users.ToListAsync())
            {
                if (user.UserName != admin.UserName)
                {
                    adminView.Users.Add(new UserAdminView(user.RealName, user.Email, user.PhoneNumber));
                }
            }
            return View("AdminMenu", adminView);
        }

        public ActionResult ExtendDiscountByDays()
        {
            return PartialView("ExtendDiscountWindow");
        }
        public async Task<ActionResult> RemoveDiscount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var service = await _dbContext.Services.Include(s => s.Discount).FirstOrDefaultAsync(s => s.ServiceId == id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return PartialView("ConfirmDiscountDeleteView", new ServiceLiteView(service.ServiceId, service.ServiceName, service.Discount));
        }
        [HttpPost, ActionName("RemoveDiscount")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveFromBasketConfirmed(int? id)
        {
            var service = await _dbContext.Services.Include(s => s.Discount).FirstOrDefaultAsync(s => s.ServiceId ==id);
            _dbContext.Discounts.Remove(service.Discount);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public  ActionResult AddNewDiscount()
        {
            SelectList services = new SelectList(_dbContext.Services,"ServiceId","ServiceName");
            ViewBag.Services = services;
            return PartialView("AddNewDiscountWindow");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewDiscount(AddNewDiscount newDiscount)
        {
            var service = await _dbContext.Services.FindAsync(newDiscount.ServiceId);
           
            if (ModelState.IsValid)
            {
                service.Discount = new Discount()
                {
                    Value = newDiscount.DiscountValue,
                    StartDate = newDiscount.StartDate,
                    FinishDate = newDiscount.FinishDate
                };
                _dbContext.Entry(service).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("AddNewDiscountWindow");
        }
        public async Task<ActionResult> ExtendDiscount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var discount = await _dbContext.Discounts.FindAsync(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            ExtendDiscount extendDiscount = new ExtendDiscount(id,0);
            return PartialView(extendDiscount);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExtendDiscount( ExtendDiscount extendDiscountItem)
        {
            if (extendDiscountItem.DiscountId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var discount = await _dbContext.Discounts.FindAsync(extendDiscountItem.DiscountId);
            if (discount == null)
            {
                return HttpNotFound();
            }
            discount.SetNewFinishDate(extendDiscountItem.Days);
            _dbContext.Entry(discount).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
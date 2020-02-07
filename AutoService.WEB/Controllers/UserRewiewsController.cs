using AutoService.WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class UserRewiewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: UserRewiews
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var userRewiews = db.UserRewiews.Include(u => u.ApplicationUser);
            ViewBag.UserId = User.Identity.GetUserId();
            return View(await userRewiews.ToListAsync());
        }

        // GET: UserRewiews/Details/5
        // [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRewiew userRewiew = await db.UserRewiews.FindAsync(id);
            var userId = User.Identity.GetUserId();
            userRewiew.ApplicationUser = await UserManager.FindByIdAsync(userId);
            if (userRewiew == null)
            {
                return HttpNotFound();
            }
            return View(userRewiew);
        }

        // GET: UserRewiews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRewiews/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserRewiewId,RewiewText,ApplicationUserId")] UserRewiew userRewiew)
        {
            userRewiew.ApplicationUserId = User.Identity.GetUserId();
            userRewiew.Date = DateTime.Now;
            if (ModelState.IsValid || !string.IsNullOrEmpty(userRewiew.RewiewText))
            {
                db.UserRewiews.Add(userRewiew);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "UserRewiews");
            }
            return View();
        }

        // GET: UserRewiews/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRewiew userRewiew = await db.UserRewiews.FindAsync(id);
            if (userRewiew == null)
            {
                return HttpNotFound();
            }
            return View(userRewiew);
        }

        // POST: UserRewiews/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserRewiewId,RewiewText,ApplicationUserId")] UserRewiew userRewiew)
        {
            userRewiew.ApplicationUserId = User.Identity.GetUserId();
            userRewiew.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(userRewiew).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userRewiew);
        }

        // GET: UserRewiews/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRewiew userRewiew = await db.UserRewiews.FindAsync(id);
            if (userRewiew == null)
            {
                return HttpNotFound();
            }
            return View(userRewiew);
        }

        // POST: UserRewiews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserRewiew userRewiew = await db.UserRewiews.FindAsync(id);
            db.UserRewiews.Remove(userRewiew);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
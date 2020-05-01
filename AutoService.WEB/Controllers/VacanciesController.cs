using AutoService.WEB.Models;
using PagedList;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class VacanciesController : Controller
    {
        private ApplicationDbContext _db;

        public VacanciesController()
        {
        }

        public VacanciesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Vacancies
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            var jobVacancies = await _db.JobVacancies.ToListAsync();
            return View(jobVacancies.ToPagedList(pageNumber, pageSize));
        }

        // GET: Vacancies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobVacancy jobVacancy = await _db.JobVacancies.FindAsync(id);
            if (jobVacancy == null)
            {
                return HttpNotFound();
            }
            return View(jobVacancy);
        }

        // GET: Vacancies/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vacancies/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VacancyName,VacancyDescription,Salary,Email,ContactPhone")] JobVacancy jobVacancy)
        {
            if (ModelState.IsValid || !string.IsNullOrEmpty(jobVacancy.VacancyDescription))
            {
                _db.JobVacancies.Add(jobVacancy);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jobVacancy);
        }

        // GET: Vacancies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobVacancy jobVacancy = await _db.JobVacancies.FindAsync(id);
            if (jobVacancy == null)
            {
                return HttpNotFound();
            }
            return View(jobVacancy);
        }

        // POST: Vacancies/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,VacancyName,VacancyDescription,Salary,Email,ContactPhone")] JobVacancy jobVacancy)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(jobVacancy).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(jobVacancy);
        }

        // GET: Vacancies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobVacancy jobVacancy = await _db.JobVacancies.FindAsync(id);
            if (jobVacancy == null)
            {
                return HttpNotFound();
            }
            return View(jobVacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JobVacancy jobVacancy = await _db.JobVacancies.FindAsync(id);
            _db.JobVacancies.Remove(jobVacancy);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
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
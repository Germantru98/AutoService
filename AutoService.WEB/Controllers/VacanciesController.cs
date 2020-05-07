using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using PagedList;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class VacanciesController : Controller
    {
        private ApplicationDbContext _db;
        private IVacanciesLogic _vacanciesLogic;

        public VacanciesController()
        {
        }

        public VacanciesController(ApplicationDbContext db, IVacanciesLogic vacanciesLogic)
        {
            _db = db;
            _vacanciesLogic = vacanciesLogic;
        }

        // GET: Vacancies
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? page, Message? message)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var jobVacancies = await _vacanciesLogic.GetAllVacancies();
            ViewBag.StatusMessage = MessageGenerator(message);
            return View(jobVacancies.ToPagedList(pageNumber, pageSize));
        }

        public enum Message
        {
            AddVacancySuccess,
            RemoveVacancySuccess,
            EditVacancySuccess,
            Error
        }

        private string MessageGenerator(Message? message)
        {
            return message == Message.AddVacancySuccess ? ""
                : message == Message.EditVacancySuccess ? ""
                : message == Message.RemoveVacancySuccess ? ""
                : message == Message.Error ? ""
                : null;
        }

        // GET: Vacancies/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Vacancies/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobVacancy jobVacancy)
        {
            if (ModelState.IsValid)
            {
                await _vacanciesLogic.AddNewVacancy(jobVacancy);
                return RedirectToAction("Index", new { message = Message.AddVacancySuccess });
            }
            return RedirectToAction("Index", new { message = Message.Error });
        }

        // GET: Vacancies/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                var vacancy = await _vacanciesLogic.FindVacancy(id);
                return PartialView(vacancy);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Message.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Message.Error });
            }
        }

        // POST: Vacancies/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobVacancy jobVacancy)
        {
            if (ModelState.IsValid)
            {
                await _vacanciesLogic.EditVacancy(jobVacancy);
                return RedirectToAction("Index", new { message = Message.EditVacancySuccess });
            }
            return RedirectToAction("Index", new { message = Message.Error });
        }

        // GET: Vacancies/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                var vacancy = await _vacanciesLogic.FindVacancy(id);
                return PartialView(vacancy);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Message.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Message.Error });
            }
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _vacanciesLogic.RemoveVacancy(id);
            return RedirectToAction("Index", new { message = Message.RemoveVacancySuccess });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
                _vacanciesLogic.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
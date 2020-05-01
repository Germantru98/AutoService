using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class UserReviewsController : Controller
    {
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private IReviewsLogic _reviewsLogic;

        public UserReviewsController()
        {
        }

        public UserReviewsController(ApplicationDbContext db, IReviewsLogic logic)
        {
            _db = db;
            _reviewsLogic = logic;
        }

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
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? page, Messages? message)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var userReviewsList = await _reviewsLogic.GetAllReviews();
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.StatusMessage = MessageGenerator(message);
            return View(userReviewsList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CreateReview()
        {
            return PartialView("Create");
        }

        // POST: UserRewiews/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateReview(CreateReviewView userRewiew)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userId);
                await _reviewsLogic.CreateReview(new UserReview(userRewiew.Text, userId, user.UserName));
                return RedirectToAction("Index", new { message = Messages.AddNewReviewSuccess });
            }
            return RedirectToAction("Index", new { message = Messages.Error });
        }

        public async Task<ActionResult> Edit(int? reviewId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var editReviewView = await _reviewsLogic.StartEditUserReview(reviewId, userId);
                return PartialView(editReviewView);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
        }

        // POST: UserRewiews/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserReviewView review)
        {
            if (ModelState.IsValid)
            {
                await _reviewsLogic.EditUserReview(review);
                return RedirectToAction("Index", new { message = Messages.EditUserReviewSuccess });
            }
            return RedirectToAction("Index", new { message = Messages.Error });
        }

        // GET: UserRewiews/Delete/5
        public async Task<ActionResult> Delete(int? reviewId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                bool isAdmin = User.IsInRole("Admin");
                var review = await _reviewsLogic.StartUserReviewRemoving(reviewId, userId, isAdmin);
                return PartialView("Delete", review);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { message = Messages.Error });
            }
        }

        // POST: UserRewiews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int reviewId)
        {
            await _reviewsLogic.RemoveUserReview(reviewId);
            return RedirectToAction("Index", new { message = Messages.DeleteUserReviewSuccess });
        }

        public enum Messages
        {
            AddNewReviewSuccess,
            EditUserReviewSuccess,
            DeleteUserReviewSuccess,
            Error
        }

        private string MessageGenerator(Messages? message)
        {
            return message == Messages.AddNewReviewSuccess ? "Ваш отзыв добавлен."
               : message == Messages.EditUserReviewSuccess ? "Отзыв успешно изменен."
               : message == Messages.DeleteUserReviewSuccess ? "Отзыв удален."
               : message == Messages.Error ? "Произошла ошибка."
               : null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
                _reviewsLogic.Dispose();
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
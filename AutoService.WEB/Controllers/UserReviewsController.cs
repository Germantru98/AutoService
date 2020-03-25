using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Data.Entity;
using System.Net;
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
        public async Task<ActionResult> Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var userReviewsList = await _db.UserRewiews.ToListAsync();
            ViewBag.UserId = User.Identity.GetUserId();
            return View(userReviewsList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CreateReview()
        {
            return View("Create");
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
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        public async Task<ActionResult> Edit(int? reviewId)
        {
            try
            {
                var editorId = User.Identity.GetUserId();
                var userReview = await _reviewsLogic.FindUserReview(reviewId);
                if (editorId == userReview.OwnerId)
                {
                    var view = new EditUserReviewView()
                    {
                        EditedText = userReview.ReviewText,
                        ReviewId = (int)reviewId
                    };
                    ViewBag.EditView = view;
                    return View(view);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
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
                return RedirectToAction("Index");
            }
            return View((EditUserReviewView)ViewBag.EditView);
        }

        // GET: UserRewiews/Delete/5
        public async Task<ActionResult> Delete(int? reviewId)
        {
            try
            {
                var review = await _reviewsLogic.FindUserReview(reviewId);
                var userId = User.Identity.GetUserId();
                if (userId == review.OwnerId||User.IsInRole("Admin"))
                {
                    return PartialView("Delete", review);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
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

        // POST: UserRewiews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int reviewId)
        {
            await _reviewsLogic.RemoveUserReview(reviewId);
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
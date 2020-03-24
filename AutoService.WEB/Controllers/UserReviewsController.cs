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
        public UserReviewsController(ApplicationDbContext db , IReviewsLogic logic)
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
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var userReviewsList =  _db.UserRewiews.ToPagedList(pageNumber, pageSize);
            ViewBag.UserId = User.Identity.GetUserId();
            return View(userReviewsList);
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
                var newReview = new UserReview(userRewiew.Text, User.Identity.GetUserId());
                _db.UserRewiews.Add(newReview);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        public async Task<ActionResult> Edit(int? reviewId)
        {
            try
            {
                var userReview =await _reviewsLogic.FindUserReview(reviewId);
                var view = new EditUserReviewView()
                {
                    EditedText = userReview.ReviewText,
                    UserId = userReview.OwnerId,
                    ReviewId = (int)reviewId
                };
                ViewBag.EditView = view;
                return View(view);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch(NullReferenceException)
            {
                return HttpNotFound();
            }      
            
        }

        // POST: UserRewiews/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserReviewView rewiew)
        {
            if (ModelState.IsValid)
            {
                var editedReview = new UserReview(rewiew.ReviewId, rewiew.EditedText, rewiew.UserId);
                _db.Entry(editedReview).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View((EditUserReviewView)ViewBag.EditView);
        }

        // GET: UserRewiews/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                var review = await _reviewsLogic.FindUserReview(id);
                if (User.Identity.GetUserId()==review.OwnerId)
                {
                    return PartialView(review);
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var review = await _reviewsLogic.FindUserReview(id);
            _db.UserRewiews.Remove(review);
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
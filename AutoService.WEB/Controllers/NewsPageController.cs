using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class NewsPageController : Controller
    {
        private INewsPageLogic _newsPageLogic;

        public NewsPageController(INewsPageLogic newsPageLogic)
        {
            _newsPageLogic = newsPageLogic;
        }

        public NewsPageController()
        {
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(Message? message)
        {
            ViewBag.StatusMessage = MessageGenerator(message);
            var news = await _newsPageLogic.GetAllNews();
            return View(news);
        }

        public enum Message
        {
            createSuccess,
            removeSuccess,
            editSuccess,
            error
        }

        private string MessageGenerator(Message? message)
        {
            return message == Message.createSuccess ? "Новость успешно создана"
                : message == Message.editSuccess ? "Новость обновлена"
                : message == Message.removeSuccess ? "Новость успешно удалена"
                : message == Message.error ? "Произошла ошибка"
                : null;
        }

        public async Task<ActionResult> GetNews(int? newsId)
        {
            try
            {
                var news = await _newsPageLogic.GetNews(newsId);
                return View("UserNewsView", news);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Message.error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Message.error });
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNews(CreateNews news)
        {
            if (ModelState.IsValid)
            {
                await _newsPageLogic.AddNews(news);
                return RedirectToAction("Index", new { message = Message.createSuccess });
            }
            return RedirectToAction("Index", new { message = Message.error });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveNews(int? newsId)
        {
            try
            {
                var news = await _newsPageLogic.GetNews(newsId);
                return PartialView(news);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Message.error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Message.error });
            }
        }

        [HttpPost, ActionName("RemoveNews")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveNewsConfirmed(int newsId)
        {
            if (ModelState.IsValid)
            {
                _newsPageLogic.RemoveNews(newsId);
                return RedirectToAction("Index", new { message = Message.removeSuccess });
            }
            return RedirectToAction("Index", new { message = Message.error });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditNews(int? newsId)
        {
            try
            {
                var news = await _newsPageLogic.StartEditNews(newsId);
                return View(news);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = Message.error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = Message.error });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditNews(EditNews news)
        {
            if (ModelState.IsValid)
            {
                await _newsPageLogic.EditNews(news);
                return RedirectToAction("Index", new { message = Message.editSuccess });
            }
            return View(news);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
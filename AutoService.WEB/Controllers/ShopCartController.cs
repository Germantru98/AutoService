using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [Authorize]
    public class ShopCartController : Controller
    {
        private IServicesLogic _serviceLogic;
        private ISummariesLogic _summariesLogic;
        private ICarLogic _carLogic;

        public ShopCartController(IServicesLogic serviceLogic, ISummariesLogic summariesLogic, ICarLogic carLogic)
        {
            _serviceLogic = serviceLogic;
            _summariesLogic = summariesLogic;
            _carLogic = carLogic;
        }

        public ActionResult Index(Cart cart, SystemMessages? message)
        {
            var view = new CartIndexViewModel()
            {
                Cart = cart
            };
            ViewBag.StatusMessage = MessageGenerator(message);
            return View(view);
        }

        public enum SystemMessages
        {
            removeFromCartSuccess,
            cleanUpShopCartSuccess,
            orderCreateSuccess,
            Error
        }

        private string MessageGenerator(SystemMessages? message)
        {
            return message == SystemMessages.Error ? "Произошла ошибка"
                : message == SystemMessages.cleanUpShopCartSuccess ? "Корзина очищена"
                : message == SystemMessages.orderCreateSuccess ? "Заказ успешно создан"
                : message == SystemMessages.removeFromCartSuccess ? "Услуга удалена из корзины"
                : null;
        }

        public async Task<ActionResult> AddToCart(Cart cart, int? serviceId)
        {
            try
            {
                var service = await _serviceLogic.FindServiceWithDiscount(serviceId);
                cart.AddItem(service);
                return PartialView("AddToCartModalView");
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = SystemMessages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = SystemMessages.Error });
            }
            catch (Exception)
            {
                return PartialView("ExistsInUserShopCartModalView");
            }
        }

        public async Task<ActionResult> RemoveFromCart(int? serviceId)
        {
            try
            {
                var service = _serviceLogic.MapServiceToServiceView(await _serviceLogic.FindServiceWithDiscount(serviceId));
                return PartialView("DeleteServiceFromShopCartModalView", service);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = SystemMessages.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = SystemMessages.Error });
            }
        }

        [HttpPost, ActionName("RemoveFromCart")]
        public ActionResult RemoveFromCartConfirmed(Cart cart, int? serviceId)
        {
            if (ModelState.IsValid)
            {
                cart.RemoveFromCart(serviceId);
                return RedirectToAction("Index", new { message = SystemMessages.removeFromCartSuccess });
            }
            return RedirectToAction("Index", new { message = SystemMessages.Error });
        }

        public ActionResult ClearShopCart()
        {
            return PartialView();
        }

        [HttpPost, ActionName("ClearShopCart")]
        public ActionResult ClearShopCartConfirmed(Cart cart)
        {
            cart.Clear();
            return RedirectToAction("Index", new { message = SystemMessages.cleanUpShopCartSuccess });
        }

        public async Task<ActionResult> GetServiceSummary(Cart cart)
        {
            var userId = User.Identity.GetUserId();
            var userCars = await _carLogic.GetAllUserCars(userId);
            ViewBag.Cars = new SelectList(userCars, "CarId", "FullName");
            var summary = _summariesLogic.GetServicesSummaryView(cart.Lines, cart.ComputeTotalValue());
            return View("ServicesSummary", summary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetServiceSummary(ServicesSummaryView summary, Cart cart)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                ServicesSummary servicesSummary = new ServicesSummary()
                {
                    DateOfCreating = DateTime.Now,
                    DayOfWork = summary.selectedDateTime,
                    TotalPrice = summary.TotalPrice,
                    UserCarId = summary.CarId,
                    UserId = userId,
                    ServiceList = _serviceLogic.ServicesToString(cart.Lines.ToList())
                };
                await _summariesLogic.AddNewSummary(servicesSummary);
                cart.Clear();
                return RedirectToAction("Index", new { message = SystemMessages.orderCreateSuccess });
            }
            return RedirectToAction("Index", new { message = SystemMessages.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _carLogic.Dispose();
                _serviceLogic.Dispose();
                _summariesLogic.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
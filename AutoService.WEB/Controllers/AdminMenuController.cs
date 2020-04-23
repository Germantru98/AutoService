using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMenuController : Controller
    {
        private IServicesLogic _servicesLogic;
        private IAdminLogic _adminLogic;
        private ISummariesLogic _summariesLogic;
        private ICarLogic _carLogic;
        private IHomePageLogic _homePageLogic;

        public AdminMenuController()
        {
        }

        public AdminMenuController(IServicesLogic servicesLogic, IAdminLogic adminLogic, ISummariesLogic summariesLogic, ICarLogic carLogic, IHomePageLogic homePageLogic)
        {
            _servicesLogic = servicesLogic;
            _adminLogic = adminLogic;
            _summariesLogic = summariesLogic;
            _carLogic = carLogic;
            _homePageLogic = homePageLogic;
        }

        public enum AdminMenuMessages
        {
            RemoveMainCarouselItemSuccess,
            AddNewMainCarouselItemSuccess,
            SuccessMainCarouselItemEdit,
            AddNewCarBrandSuccess,
            RemoveCarBrandSuccess,
            ExtendDiscountSuccess,
            AddDiscountSuccess,
            RemoveDiscountSuccess,
            Error
        }

        // GET: AdminMenu
        public async Task<ActionResult> Index(AdminMenuMessages? message)
        {
            ViewBag.StatusMessage =
                message == AdminMenuMessages.AddNewMainCarouselItemSuccess ? "Операция: \"Добавление нового слайда\" прошла успешно"
                : message == AdminMenuMessages.RemoveMainCarouselItemSuccess ? "Операция: \"Удаление слайда\" прошла успешно"
                : message == AdminMenuMessages.SuccessMainCarouselItemEdit ? "Операция: \"Изменение слайда\" прошла успешно"
                : message == AdminMenuMessages.AddNewCarBrandSuccess ? "Операция: \"Добавление нового автобренда\" прошла успешно"
                : message == AdminMenuMessages.RemoveCarBrandSuccess ? "Операция: \"Удаление автобренда\" прошла успешно"
                : message == AdminMenuMessages.ExtendDiscountSuccess ? "Операция: \"Продление скидки\" прошла успешно"
                : message == AdminMenuMessages.RemoveDiscountSuccess ? "Операция: \"Удаление скидки\" прошла успешно"
                : message == AdminMenuMessages.AddDiscountSuccess ? "Операция: \"Добавление скидки\" прошла успешно"
                : message == AdminMenuMessages.Error ? "Ошибка"
                : null;
            var indexView = await _adminLogic.GetAdminMenuView();
            return View("AdminMenu", indexView);
        }

        public async Task<ActionResult> RemoveDiscount(int? serviceId)
        {
            try
            {
                var service = await _servicesLogic.FindServiceWithDiscount(serviceId);
                return PartialView("ConfirmDiscountDeleteView", new RemoveDiscountFromServiceView(service.ServiceName, service.Discount));
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = AdminMenuMessages.Error });
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost, ActionName("RemoveDiscount")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveDiscountConfirmed(int? serviceId)
        {
            try
            {
                var service = await _servicesLogic.FindServiceWithDiscount(serviceId);
                await _servicesLogic.RemoveDiscount(service.Discount.DiscountId);
                return RedirectToAction("Index", new { message = AdminMenuMessages.RemoveDiscountSuccess });
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = AdminMenuMessages.Error });
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        public ActionResult AddNewDiscount()
        {
            SelectList services = new SelectList(_servicesLogic.GetServicesFromDb(), "ServiceId", "ServiceName");
            ViewBag.Services = services;
            return View("AddNewDiscountWindow");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewDiscount(AddNewDiscount newDiscount)
        {
            if (ModelState.IsValid)
            {
                await _servicesLogic.AddDiscount(newDiscount);
                return RedirectToAction("Index");
            }
            SelectList services = new SelectList(_servicesLogic.GetServicesFromDb(), "ServiceId", "ServiceName");
            ViewBag.Services = services;
            return View("AddNewDiscountWindow");
        }

        public async Task<ActionResult> ExtendDiscount(int? id)
        {
            try
            {
                var discount = await _servicesLogic.FindDiscount(id);
                ExtendDiscount extendDiscount = new ExtendDiscount(id, 1);
                return PartialView(extendDiscount);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = AdminMenuMessages.Error });
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExtendDiscount(ExtendDiscount extendDiscountItem)
        {
            if (ModelState.IsValid)
            {
                await _servicesLogic.ExtendDiscount(extendDiscountItem);
                return RedirectToAction("Index", new { message = AdminMenuMessages.ExtendDiscountSuccess });

            }
            return RedirectToAction("Index", new { message = AdminMenuMessages.Error });
        }

        public async Task<ActionResult> CompleteSummary(int? summaryId)
        {
            try
            {
                var summary = await _summariesLogic.FindSummaryById(summaryId);
                return PartialView("CompleteOrderModalView");
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

        [HttpPost, ActionName("CompleteSummary")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteSummaryConfirmed(int summaryId)
        {
            try
            {
                await _summariesLogic.CompleteSummary(summaryId);
                return RedirectToAction("Index");
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult GetSummariesHistoryForThePeriod()
        {
            var periodView = new GetPeriodView();
            return PartialView("GetSummariesHistoryForThePeriodModalView", periodView);
        }

        [HttpPost]
        public async Task<ActionResult> GetSummariesHistoryForThePeriod(GetPeriodView periodView)
        {
            var summariesForThePeriod = await _summariesLogic.GetCompletedSummariesByPeriod(periodView.StartDate, periodView.FinishDate);
            return PartialView("SummariesBlockView", summariesForThePeriod);
        }

        [HttpPost]
        public async Task<ActionResult> GetAllUncompletedOrders()
        {
            var uncompletedOrders = await _summariesLogic.GetAllUncompletedSummaries();
            return PartialView("SummariesBlockView", uncompletedOrders);
        }

        [HttpPost]
        public async Task<ActionResult> GetCurrentOrders()
        {
            var currentOrders = await _summariesLogic.GetCurrentSummaries();
            return PartialView("SummariesBlockView", currentOrders);
        }

        public ActionResult GetOrdersByPeriod()
        {
            var ordersPeriod = new GetPeriodView();
            return PartialView("SelectPeriodModalView", ordersPeriod);
        }

        [HttpPost]
        public async Task<ActionResult> GetOrdersByPeriod(GetPeriodView period)
        {
            if (ModelState.IsValid)
            {
                var ordersByThePeriod = await _summariesLogic.GetCompletedSummariesByPeriod(period.StartDate, period.FinishDate);
                return PartialView("SummariesBlockView", ordersByThePeriod);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> GetAllCompletedOrders()
        {
            var orders = await _summariesLogic.GetCompletedSummaries();
            return PartialView("SummariesBlockView", orders);
        }

        public ActionResult GetCompletedOrdersByDay()
        {
            var dateView = new SelectDateView();
            return PartialView("SelectDateModalView", dateView);
        }

        [HttpPost]
        public async Task<ActionResult> GetCompletedOrdersByDay(DateTime date)
        {
            if (ModelState.IsValid)
            {
                var ordersByDay = await _summariesLogic.GetCompletedSummariesByDate(date);
                return PartialView("SummariesBlockView", ordersByDay);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public async Task<ActionResult> RemoveSummary(int? summaryId)
        {
            try
            {
                var summary = await _summariesLogic.FindSummaryById(summaryId);
                return PartialView("ConfirmSummaryRemovingModalView", summary);
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

        [HttpPost, ActionName("RemoveSummary")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveSummaryConfirmed(int summaryId)
        {
            await _summariesLogic.RemoveSummary(summaryId);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditSummary(int? summaryId)
        {
            try
            {
                var editedSummary = await _summariesLogic.FindSummaryById(summaryId);
                var userCars = await _carLogic.GetAllUserCars(editedSummary.User.Id);
                SelectList carsSelectList = new SelectList(userCars, "CarId", "FullName");
                var editView = await _summariesLogic.GetEditSummaryView(editedSummary);
                ViewBag.UserCars = carsSelectList;
                ViewBag.EditSummary = editView;
                return View("EditSummaryView", editedSummary);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeSummaryInformation(EditSummaryView summary)
        {
            if (ModelState.IsValid)
            {
                await _summariesLogic.EditSummary(summary);
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        public ActionResult SuccessOperation(string action)
        {
            return PartialView(action);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewHomeCarouselItem(AddNewCarouselItemView newItem)
        {
            if (ModelState.IsValid)
            {
                await _homePageLogic.AddNewMainCarouselItem(newItem);
                return RedirectToAction("Index", new { message = AdminMenuMessages.AddNewMainCarouselItemSuccess });
            }
            return RedirectToAction("Index", new { message = AdminMenuMessages.Error });
        }

        public async Task<ActionResult> RemoveHomeCarouselItem(int? id)
        {
            try
            {
                var carouselItem = await _homePageLogic.FindCarouselItem(id);
                return PartialView("RemoveCarouselItemModalView", carouselItem);
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

        [HttpPost, ActionName("RemoveHomeCarouselItem")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveCarouselItemConfirmed(int id)
        {
            try
            {
                await _homePageLogic.RemoveMainCarouselItem(id);
                return RedirectToAction("Index", new { message = AdminMenuMessages.RemoveMainCarouselItemSuccess });
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

        public async Task<ActionResult> EditHomeCarouselItem(int? id)
        {
            try
            {
                var item = await _homePageLogic.FindCarouselItem(id);
                var editItemView = new EditCarouselItemView()
                {
                    Title = item.Title,
                    Description = item.Description,
                    ImageHref = item.ImageHref,
                    RouteHref = item.RouteHref,
                    Id = item.Id
                };
                return PartialView("EditCarouselItemModalView", editItemView);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditHomeCarouselItem(EditCarouselItemView item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _homePageLogic.EditMainCarouselItem(item);
                    return RedirectToAction("Index", new { message = AdminMenuMessages.SuccessMainCarouselItemEdit });
                }
                return RedirectToAction("Index", new { message = AdminMenuMessages.Error });
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewCarBrand(CarBrand newBrand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _homePageLogic.AddNewCarBrand(newBrand);
                    return RedirectToAction("Index", new { message = AdminMenuMessages.AddNewCarBrandSuccess });
                }
                return RedirectToAction("Index", new { message = AdminMenuMessages.Error });
            }
            catch (NullReferenceException)
            {
                return HttpNotFound();
            }
        }

        public async Task<ActionResult> RemoveCarBrand(int? id)
        {
            try
            {
                var carBrand = await _homePageLogic.FindCarBrand(id);
                return PartialView("RemoveCarBrand", carBrand);
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

        [HttpPost, ActionName("RemoveCarBrand")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveCarBrandConfirmed(int id)
        {
            try
            {
                await _homePageLogic.RemoveCarBrand(id);
                return RedirectToAction("Index", new { message = AdminMenuMessages.RemoveCarBrandSuccess });
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
    }
}
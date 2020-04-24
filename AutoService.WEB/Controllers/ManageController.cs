using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _dbContext;
        private IUserLogic _userLogic;
        private ICarLogic _carLogic;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ManageController(ApplicationDbContext dbContext, IUserLogic userLogic, ICarLogic carLogic)
        {
            _dbContext = dbContext;
            _userLogic = userLogic;
            _carLogic = carLogic;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        //
        // GET: /Manage/Index

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (User.IsInRole("Admin"))
            {
                return Redirect("/AdminMenu");
            }
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона добавлен."
                : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален."
                : message == ManageMessageId.AddNewCar ? "Добавлен новый автомобиль."
                : message == ManageMessageId.EditCar ? "Данные об автомобиле обновлены."
                : message == ManageMessageId.RemoveCar ? "Автомобиль удален."
                : null;
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Email = user.Email,
                RealName = user.RealName,
                Cars =await _carLogic.GetAllUserCars(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                Basket = await GetServicesFromBasketItem(await _dbContext.BasketItems.Where(item => item.UserId == userId).ToListAsync())
            };
            return View(model);
        }

        public async Task<Dictionary<int, Service>> GetServicesFromBasketItem(List<BasketItem> items)
        {
            var result = new Dictionary<int, Service>();
            foreach (var item in items)
            {
                var service = await _dbContext.Services.FindAsync(item.ServiceId);
                service.Discount = await _dbContext.Discounts.FindAsync(service.DiscountId);
                result.Add(item.Id, service);
            }
            return result;
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Создание и отправка маркера
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Ваш код безопасности: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Отправка SMS через поставщик SMS для проверки номера телефона
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Это сообщение означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", "Не удалось проверить телефон");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Это сообщение означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Внешнее имя входа удалено."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Запрос перенаправления к внешнему поставщику входа для связывания имени входа текущего пользователя
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        public ActionResult AddNewCar()
        {
            return PartialView("AddNewCarModalView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewCar(AddNewCarViewModel newCar)
        {
            if (ModelState.IsValid)
            {
                await _userLogic.AddNewCar(newCar, User.Identity.GetUserId());
                return RedirectToAction("Index", "Manage",new { message = ManageMessageId.AddNewCar});
            }
            return RedirectToAction("Index", "Manage", new { message = ManageMessageId.Error });
        }

        public async Task<ActionResult> RemoveCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await _dbContext.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            var carView = _carLogic.MapCarToCarView(car);
            return PartialView("RemoveUserCarModalView", carView);
        }

        [HttpPost, ActionName("RemoveCar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _userLogic.RemoveCar(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await _dbContext.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return PartialView("EditUserCarModalView", car);
        }

        [HttpPost, ActionName("EditCar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCar(Car car)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(car).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index", new { message = ManageMessageId.EditCar });
            }
            return RedirectToAction("Index", "Manage", new { message = ManageMessageId.Error });
        }

        public async Task<ActionResult> RemoveFromBasket(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasketItem item = await _dbContext.BasketItems.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            var service = await _dbContext.Services.FindAsync(item.ServiceId);
            service.Discount = await _dbContext.Discounts.FindAsync(service.DiscountId);
            return PartialView("ModalDeleteConfirm", service);
        }

        [HttpPost, ActionName("RemoveFromBasket")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveFromBasketConfirmed(int id)
        {
            await _userLogic.RemoveFromBasket(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetServiceSummary()
        {
            var userId = User.Identity.GetUserId();
            var services = GetServicesFromBasket(await _dbContext.BasketItems.Where(item => item.UserId == userId).ToListAsync());
            ServicesSummaryView summary = new ServicesSummaryView()
            {
                ServicesList = services,
                TotalPrice = _userLogic.GetTotalPrice(services)
            };
            ViewBag.Cars = new SelectList(_dbContext.Cars.Where(c => c.ApplicationUserId == userId), "Id", "Model");
            return View("ServicesSummary", summary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetServiceSummary(ServicesSummaryView summary)
        {
            var userId = User.Identity.GetUserId();
            var services = GetServicesFromBasket(await _dbContext.BasketItems.Where(item => item.UserId == userId).ToListAsync());
            if (ModelState.IsValid)
            {
                ServicesSummary servicesSummary = new ServicesSummary()
                {
                    DateOfCreating = DateTime.Now,
                    DayOfWork = summary.selectedDateTime,
                    TotalPrice = summary.TotalPrice,
                    UserCarId = summary.CarId,
                    UserId = userId,
                    ServiceList = ListServicesToString(services)
                };
                _dbContext.ServicesSummaries.Add(servicesSummary);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Cars = new SelectList(_dbContext.Cars.Where(c => c.ApplicationUserId == userId), "Id", "Model");
            summary.ServicesList = services;
            summary.TotalPrice = _userLogic.GetTotalPrice(summary.ServicesList);
            return View("ServicesSummary", summary);
        }

        private string ListServicesToString(List<Service> services)
        {
            var result = string.Empty;
            for (int i = 0; i < services.Count; i++)
            {
                if (i < services.Count - 1)
                {
                    result += $"{services[i].ServiceId}|";
                }
                else
                {
                    result += $"{services[i].ServiceId}";
                }
            }
            return result;
        }

        public ActionResult ClearBasket()
        {
            return PartialView();
        }

        [HttpPost, ActionName("ClearBasket")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClearConfirmed()
        {
            var userId = User.Identity.GetUserId();
            await _userLogic.RemoveAllItemsFromBasket(userId);
            return RedirectToAction("Index");
        }

        private List<Service> GetServicesFromBasket(List<BasketItem> items)
        {
            var result = new List<Service>();
            foreach (var item in items)
            {
                result.Add(_dbContext.Services.Include(s => s.Discount).FirstOrDefault(s => s.ServiceId == item.ServiceId));
            }
            return result;
        }

        #region Вспомогательные приложения

        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            AddNewCar,
            EditCar,
            RemoveCar,
            Error
        }

        #endregion Вспомогательные приложения
    }
}
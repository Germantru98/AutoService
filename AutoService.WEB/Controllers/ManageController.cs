﻿using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
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
        private ICarLogic _carLogic;
        private ISummariesLogic _summariesLogic;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ManageController(ICarLogic carLogic, ISummariesLogic summariesLogic)
        {
            _carLogic = carLogic;
            _summariesLogic = summariesLogic;
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

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (User.IsInRole("Admin"))
            {
                return Redirect("/AdminMenu");
            }
            ViewBag.StatusMessage = MessageGenerator(message);
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var model = new IndexViewModel()
            {
                HasPassword = HasPassword(),
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Email = user.Email,
                RealName = user.RealName,
                Cars = await _carLogic.GetAllUserCars(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                Orders = await _summariesLogic.GetAllUserOrders(userId)
            };
            return View(model);
        }

        private string MessageGenerator(ManageMessageId? message)
        {
            return message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности."
                : message == ManageMessageId.Error ? "Произошла ошибка."
                : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона добавлен."
                : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален."
                : message == ManageMessageId.AddNewCar ? "Добавлен новый автомобиль."
                : message == ManageMessageId.EditCar ? "Данные об автомобиле обновлены."
                : message == ManageMessageId.RemoveCar ? "Автомобиль удален."
                : message == ManageMessageId.RemoveCarError ? "Ошибка при удалении автомобиля."
                : message == ManageMessageId.EditCarError ? "Ошибка при обновлении информации об автомобиле."
                : message == ManageMessageId.RemoveFromShopCart ? "Услуга была удалена из корзины"
                : message == ManageMessageId.RemoveFromShopCartError ? "Ошибка при удалении услуги из корзины"
                : message == ManageMessageId.ClearShopCart ? "Корзина очищена"
                : message == ManageMessageId.RemoveOrderSuccess ? "Заказ удален"
                : message == ManageMessageId.AccessDeny ? "Ошибка доступа"
                : null;
        }

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
            return PartialView();
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
            return PartialView("ChangePasswordModalView");
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
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
                if (_carLogic != null)
                {
                    _carLogic.Dispose();
                }
                if (_summariesLogic != null)
                {
                    _summariesLogic.Dispose();
                }
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
                var userId = User.Identity.GetUserId();
                await _carLogic.AddNewUserCar(userId, newCar);
                return RedirectToAction("Index", "Manage", new { message = ManageMessageId.AddNewCar });
            }
            return RedirectToAction("Index", "Manage", new { message = ManageMessageId.Error });
        }

        public async Task<ActionResult> RemoveCar(int? id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var carView = await _carLogic.StartRemoveUserCarOperation(id, userId);
                ViewBag.DeletedCarModel = await _carLogic.GetUserCar(id);
                return PartialView("RemoveUserCarModalView", carView);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.RemoveCarError });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.RemoveCarError });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.RemoveCarError });
            }
        }

        [HttpPost, ActionName("RemoveCar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                await _carLogic.RemoveUserCar(id, userId);
                return RedirectToAction("Index", new { message = ManageMessageId.RemoveCar });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.RemoveCarError });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.Error });
            }
        }

        public async Task<ActionResult> EditCar(int? id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var carView = await _carLogic.StartEditUserCarOperation(id, userId);
                return PartialView("EditUserCarModalView", carView);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.EditCarError });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.EditCarError });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.EditCarError });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCar(EditCarView car)
        {
            if (ModelState.IsValid)
            {
                await _carLogic.EditUserCar(car);
                return RedirectToAction("Index", new { message = ManageMessageId.EditCar });
            }
            return RedirectToAction("Index", "Manage", new { message = ManageMessageId.EditCarError });
        }

        public async Task<ActionResult> RemoveSummary(int? summaryId)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var summary = await _summariesLogic.GetUserOrder(summaryId, userId);
                return PartialView("RemoveOrderModalView", summary);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.Error });
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.AccessDeny });
            }
        }

        [HttpPost, ActionName("RemoveSummary")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveSummaryConfirmed(int summaryId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity.GetUserId();
                    var isAdmin = User.IsInRole("Admin");
                    await _summariesLogic.RemoveSummary(summaryId, userId, isAdmin);
                    return RedirectToAction("Index", new { message = ManageMessageId.RemoveOrderSuccess });
                }
                catch (InvalidOperationException)
                {
                    return RedirectToAction("Index", new { message = ManageMessageId.AccessDeny });
                }
            }
            return RedirectToAction("Index", new { message = ManageMessageId.Error });
        }

        public async Task<ActionResult> EditSummary(int? summaryId)
        {
            try
            {
                if (summaryId == null)
                {
                    return RedirectToAction("Index", new { message = ManageMessageId.Error });
                }
                var userId = User.Identity.GetUserId();
                var editedSummary = await _summariesLogic.GetUserOrder(summaryId, userId);
                var userCars = await _carLogic.GetAllUserCars(userId);
                SelectList carsSelectList = new SelectList(userCars, "CarId", "FullName");
                var editView = await _summariesLogic.GetEditSummaryView(editedSummary);
                ViewBag.UserCars = carsSelectList;
                ViewBag.EditSummary = editView;
                return View("EditSummaryView", editedSummary);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.Error });
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.Error });
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Index", new { message = ManageMessageId.AccessDeny });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeSummaryInformation(EditSummaryView summary)
        {
            if (ModelState.IsValid)
            {
                await _summariesLogic.EditSummary(summary);
                return RedirectToAction("Index", new { message = ManageMessageId.EditOrderSucces });
            }
            return RedirectToAction("Index", new { message = ManageMessageId.Error });
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
            EditCarError,
            RemoveCar,
            RemoveCarError,
            RemoveFromShopCart,
            RemoveFromShopCartError,
            ClearShopCart,
            Error,
            AccessDeny,
            RemoveOrderSuccess,
            EditOrderSucces
        }

        #endregion Вспомогательные приложения
    }
}
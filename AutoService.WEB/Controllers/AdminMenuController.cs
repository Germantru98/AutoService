using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMenuController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private IServicesLogic _servicesLogic;

        public AdminMenuController(IServicesLogic logic)
        {
            _servicesLogic = logic;
        }

        // GET: AdminMenu
        public async Task<ActionResult> Index()
        {
            var adminView = new AdminMenuView()
            {
                Users = new List<UserAdminView>(),
                Discounts = _servicesLogic.GetServices(await _dbContext.Services.Include(s => s.Discount).Where(s => s.DiscountId != null).ToListAsync())
            };
            var admin = _dbContext.Users.Find(User.Identity.GetUserId());
            foreach (var user in await _dbContext.Users.ToListAsync())
            {
                if (user.UserName != admin.UserName)
                {
                    adminView.Users.Add(new UserAdminView(user.RealName, user.Email, user.PhoneNumber));
                }
            }
            return View("AdminMenu", adminView);
        }

        public ActionResult ExtendDiscountByDays()
        {
            return PartialView("ExtendDiscountWindow");
        }
    }
}
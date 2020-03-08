using AutoService.WEB.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            HomeView homeView = new HomeView()
            {
                PopularServices = await db.Services.Include(s => s.Discount).ToListAsync(),
                CarBrands = await db.CarBrands.ToListAsync(),
                Discounts = await db.Services.Where(s => s.Discount.isActive && s.Discount.FinishDate >= DateTime.Now).ToListAsync(),
                HomeMainCarouserlItems = await db.HomeMainCarouserlItems.ToListAsync()
            };
            homeView.PopularServices.Sort();
            if (homeView.Discounts.Count > 0)
            {
                ViewBag.MainCarouselSize = "withDisc";
            }
            else
            {
                ViewBag.MainCarouselSize = "withoutDisc";
            }
            return View(homeView);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}
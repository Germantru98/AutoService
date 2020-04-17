using AutoService.WEB.Models;
using AutoService.WEB.Utils.Interfaces;
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
        private ApplicationDbContext _db;
        private IHomePageLogic _homePageLogic;

        public HomeController(ApplicationDbContext db, IHomePageLogic homePageLogic)
        {
            _db = db;
            _homePageLogic = homePageLogic;
        }

        public async Task<ActionResult> Index()
        {
            DateTime currentTime = DateTime.Now;
            HomeView homeView = new HomeView()
            {
                PopularServices = await _db.Services.Include(s => s.Discount).ToListAsync(),
                CarBrands = await _db.CarBrands.ToListAsync(),
                Discounts = await _db.Services.Where(s => s.Discount.StartDate <= currentTime && s.Discount.FinishDate >= currentTime).ToListAsync(),
                HomeMainCarouselItems = await _homePageLogic.GetMainCarousel()
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
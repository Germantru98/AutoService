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
            HomeView homeView = new HomeView()
            {
                CarBrands = await _homePageLogic.GetCarBrandsCarousel(),
                Discounts = await _homePageLogic.GetRelevantDiscountsForHomePage(),
                HomeMainCarouselItems = await _homePageLogic.GetMainCarousel()
            };
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
using AutoService.BLL.Interfaces;
using AutoService.WEB.Models;
using AutoService.WEB.Utils;
using System.Linq;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            HomeView homeView = new HomeView()
            {
                PopularServices = db.Services.ToList()
            };
            return View(homeView);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}
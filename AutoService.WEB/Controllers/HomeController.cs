using AutoService.WEB.Models;
using System.Data.Entity;
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
                CarBrands = await db.CarBrands.ToListAsync()
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
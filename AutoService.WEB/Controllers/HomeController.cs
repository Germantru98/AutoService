using AutoService.BLL.Interfaces;
using AutoService.WEB.Utils;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {

      

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}
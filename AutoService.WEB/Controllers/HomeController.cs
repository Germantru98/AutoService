using AutoService.BLL.DTO;
using AutoService.BLL.Interfaces;
using AutoService.BLL.Services;
using AutoService.WEB.Models;
using AutoService.WEB.Utils;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AutoService.WEB.Controllers
{
    public class HomeController : Controller
    {
        private ContactDataUnit _contactDataUnit;
        public HomeController(IContactData icd)
        {
            _contactDataUnit = new ContactDataUnit(icd);
        }
        
        public ActionResult Index()
        {
            ViewData["cd"] = _contactDataUnit;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            
            return View(_contactDataUnit);
        }
        
    }
}
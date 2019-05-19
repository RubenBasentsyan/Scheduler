 using System.Linq;
using System.Web.Mvc;
using WebApplication1.Helpers;
 using WebApplication1.Services;

 namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var schedule = EntityFetcher.FetchSchedule();
            return View(schedule);
        }

        public ActionResult Reschedule()
        {
            EntityModifier.Reschedule();
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        
    }
}
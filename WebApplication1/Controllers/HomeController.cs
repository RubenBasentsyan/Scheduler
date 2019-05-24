using System.Linq;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Services;
using WebApplication1.Models.ViewModels;
using System.Web.Security;
using System.Web;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            var schedule = EntityFetcher.FetchSchedule();
            return View(schedule);
        }

        [Authorize]
        public ActionResult Reschedule()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                EntityModifier.Reschedule();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Home");
        }
        

        public ActionResult Login()
        {
            ViewBag.isAdmin = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ParticipantsViewModel person) 
        {
            if (ModelState["Username"] != null && ModelState["Password"] != null)
            {
                try
                {
                    EntityFetcher.FetchLoginUser(person.Username, person.Password);
                    ViewBag.Message = "Hello" + person?.Username;
                    FormsAuthentication.SetAuthCookie(person.Username, false);
                    IsAdmin.Admin = EntityFetcher.FetchUserAdminStatus(person.Username) == true;
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            IsAdmin.Admin=false;
            return RedirectToAction("Login");
        }

    }
}
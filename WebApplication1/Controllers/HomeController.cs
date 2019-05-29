using System;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(int page = 1)
        {
            ViewBag.isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            var schedule = EntityFetcher.FetchSchedule(page);
            ViewBag.currentPage = page;
            ViewBag.maxPage = EntityFetcher.SchedulePagesCount;
            ViewBag.isParamSuccess = TempData["paramSuccess"];
            ViewBag.messageState = TempData["messageState"];
            ViewBag.message = TempData["message"];
            return View(schedule);
        }

        [Authorize]
        public ActionResult Reschedule()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                try
                {
                    EntityModifier.Reschedule();
                }
                catch (Exception e)
                {
                    TempData["messageState"] = 2;
                    TempData["message"] = e.Message;
                }

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
                try
                {
                    EntityFetcher.FetchLoginUser(person.Username, person.Password);
                    ViewBag.Message = "Hello" + person?.Username;
                    FormsAuthentication.SetAuthCookie(person.Username, false);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }

            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
using System;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class ParametersController : Controller
    {
        // GET: Parameters
        public ActionResult Set()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin != true) return RedirectToAction("Login", "Home");
            return View(EntityFetcher.FetchSchedulingParameters);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Set(ParametersViewModel parameters)
        {
            if (!ModelState.IsValid)
                return View(parameters);
            try
            {
                if (EntityModifier.SetSchedulingParameters(parameters))
                {
                    TempData["messageState"] = 2; //0-success 1-warning 2-danger
                    TempData["message"] =
                        @"Due to the parameters becoming stricter, the old schedule is now redundant, 
press the Reschedule button to generate a schedule that satisfies the new parameters";
                }
                else
                {
                    TempData["messageState"] = 1; //0-success 1-warning 2-danger
                    TempData["message"] =
                        @"The parameters did not become stricter, so the current schedule (if it exists) is valid.
Also, a more optimal schedule may be possible, try by pressing Reschedule";
                }

                TempData["paramSuccess"] = true;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(parameters);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class ImportController : Controller
    {
        [Authorize]
        public ActionResult ImportTable()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin != true)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        
        [Authorize,HttpPost]
        public ActionResult ImportTable(EnrollmentImportViewModel enrollments)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin != true)
            {
                return RedirectToAction("Login", "Home");
            }
            if(!ModelState.IsValid)
            {
                return View(enrollments);
            }
            try
            {
                EntityModifier.PopulateTablesFromCSV(enrollments.EnrollmentsCSV.InputStream);
            }
            catch
            {
                ModelState.AddModelError("EnrollmentsCSV" ,"Bad file");
                return View(enrollments);
            }
            TempData["message"] = "Successfuly imported data from the csv file, press Reschedule to populate the table with the new data";
            return RedirectToAction("Index", "Home");
        }
    }
}
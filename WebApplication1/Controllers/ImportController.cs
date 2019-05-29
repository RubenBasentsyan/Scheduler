using System.Web;
using System.Web.Mvc;
using WebApplication1.Helpers;
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
            if (isAdmin != true) return RedirectToAction("Login", "Home");
            return View();
        }

        /// <summary>
        ///     Imports a .csv file in the AUA registrar format to populate the tables in the .
        /// </summary>
        /// <param name="enrollments">The enrollments in.csv format.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ImportTable(HttpPostedFileBase enrollments)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin != true) return RedirectToAction("Login", "Home");
            if (enrollments == null || enrollments.ContentLength == 0 ||
                enrollments.ContentType != "application/vnd.ms-excel")
            {
                ModelState.AddModelError("", "Bad upload, upload a non-empty file with .csv extension");
                return View();
            }

            try
            {
                EntityModifier.PopulateTablesFromCsv(enrollments.InputStream);
            }
            catch
            {
                ModelState.AddModelError("", "Failed to read from file");
                return View();
            }

            TempData["messageState"] = 0; //0-success 1-warning 2-danger
            TempData["message"] =
                "Successfully imported data from the csv file, press Reschedule to populate the table with the new data";
            return RedirectToAction("Index", "Home");
        }
    }
}
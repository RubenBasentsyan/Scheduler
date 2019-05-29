using System.Data;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        [Authorize]
        // GET: Course
        public ActionResult Index(int page = 1)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin != true) return RedirectToAction("Login", "Home");
            ViewBag.currentPage = page;
            ViewBag.maxPage = EntityFetcher.CoursesPageCount;
            return View(EntityFetcher.FetchCourses(page));
        }

        [Authorize]
        public ActionResult Create()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true) return View();
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CourseViewModel course)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        EntityModifier.CreateCourse(course);
                    }
                    catch
                    {
                        ModelState.AddModelError("Name", "Can't create a course.");
                        return View(course);
                    }

                    return RedirectToAction("Index");
                }

                return View(course);
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
                try
                {
                    var course = EntityFetcher.FetchCourseWithId(id);
                    return View(course);
                }
                catch (DataException)
                {
                    return RedirectToAction("Index");
                }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(CourseViewModel course)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        EntityModifier.EditCourse(course);
                    }
                    catch
                    {
                        ModelState.AddModelError("Name", "Can't modify the course.");
                    }

                    return RedirectToAction("Index");
                }

                return View(course);
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
                try
                {
                    CourseViewModel course;
                    course = EntityFetcher.FetchCourseWithId(id);
                    return View(course);
                }
                catch (DataException)
                {
                    return RedirectToAction("Index");
                }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(CourseViewModel course)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                if (ModelState["CourseId"] != null)
                {
                    try
                    {
                        EntityModifier.DeleteCourse(course);
                    }
                    catch
                    {
                        ModelState.AddModelError("Name", "Can't modify the course.");
                    }

                    return RedirectToAction("Index");
                }

                return View(course);
            }

            return RedirectToAction("Login", "Home");
        }
    }
}
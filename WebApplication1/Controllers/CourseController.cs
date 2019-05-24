using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        [Authorize]
        // GET: Course
        public ActionResult Index()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                return View(EntityFetcher.FetchAllCourses());
            }
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Create()
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CourseViewModel course)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
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
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
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
            }
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(CourseViewModel course)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
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
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
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
            }
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete( CourseViewModel course )
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(this.HttpContext));
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
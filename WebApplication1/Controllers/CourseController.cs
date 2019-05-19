using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication1.Models.ViewModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View(EntityFetcher.FetchAllCourses());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseViewModel course)
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

        public ActionResult Edit(int id)
        {
            try
            {
                CourseViewModel course;
                course = EntityFetcher.FetchCourseWithId(id);
                return View(course);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(CourseViewModel course)
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

        public ActionResult Delete(int id)
        {
            try
            {
                CourseViewModel course;
                course = EntityFetcher.FetchCourseWithId(id);
                return View(course);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete( CourseViewModel course )
        {
            if (ModelState["CourseId"]!=null)
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
    }
}
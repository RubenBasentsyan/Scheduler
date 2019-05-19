using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.ViewModels.Enrollments;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class EnrollmentController : Controller
    {
        // GET: Enrollment
        public ActionResult CourseEnrollments(int id)
        {
            var enrollments = EntityFetcher.FetchCourseEnrollments(id);
            ViewBag.CourseName = enrollments.First().CourseName;
            return View(enrollments);
        }

        public ActionResult PersonEnrollments(int id)
        {
            var enrollments = EntityFetcher.FetchPersonEnrollments(id);
            ViewBag.PersonName = enrollments.First().PersonName;
            return View(enrollments);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create (CreateEnrollmentVm enrollment)
        {

            return View();
        }

        public ActionResult Delete( int id )
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete( DisplayEnrollmentVm enrollment)
        {
            return View();
        }


    }
}
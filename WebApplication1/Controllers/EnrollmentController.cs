using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        public ActionResult CourseEnrollments(int courseId)
        {
            //TempData["Info"] = new {from = "course", id = courseId};
            ViewBag.Course = EntityFetcher.FetchCourseWithId(courseId);
            return View(EntityFetcher.FetchCourseEnrollments(courseId));
        }

        public ActionResult PersonEnrollments(int personId)
        {
            //TempData["Info"] = new { from = "person", id = personId };
            ViewBag.Person = EntityFetcher.FetchParticipantWithId(personId);
            return View(EntityFetcher.FetchPersonEnrollments(personId));
        }

        public ActionResult CreateForCourse(int courseId)
        {
            var enrollment = new CreateEnrollmentVm() { CourseId = courseId };
            try
            {
                ViewBag.CourseName = EntityFetcher.FetchCourseWithId(courseId).Name;
                enrollment.ParticipantList = EntityFetcher.FetchParticipantsNotEnrolled(courseId);
            }
            catch(DataException)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(enrollment);
        }

        [HttpPost]
        public ActionResult CreateForCourse (CreateEnrollmentVm enrollment)
        {
            try
            {
                EntityModifier.CreateEnrollment(enrollment);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                ModelState.AddModelError("PersonId", "Something went wrong, this participant likely no longer exists.");
                return View(enrollment);
            }
            return RedirectToAction("CourseEnrollments", new { courseId = enrollment.CourseId });
        }

        public ActionResult CreateForParticipant(int participantId)
        {
            var enrollment = new CreateEnrollmentVm() { ParticipantId = participantId };
            try
            {
                ViewBag.ParticipantName = EntityFetcher.FetchParticipantWithId(participantId).Name;
                enrollment.CoursesList = EntityFetcher.FetchCoursesNotEnrolled(participantId);
            }
            catch (DataException)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(enrollment);
        }

        [HttpPost]
        public ActionResult CreateForParticipant(CreateEnrollmentVm enrollment)
        {
            try
            {
                EntityModifier.CreateEnrollment(enrollment);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                ModelState.AddModelError("CourseId", "Something went wrong, this course likely no longer exists.");
                return View(enrollment);
            }
            return RedirectToAction("PersonEnrollments", new { personId = enrollment.ParticipantId });
        }

        public ActionResult Delete( int id )
        {
            TempData.Keep();
            try
            {
                return View( EntityFetcher.FetchEnrollment(id));
            }
            catch (DataException)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Delete(DisplayEnrollmentVm enrollment)
        {
            EntityModifier.DeleteEnrollment(enrollment.EnrollmentId);
            return RedirectToAction("Index","Home");
        }


    }
}
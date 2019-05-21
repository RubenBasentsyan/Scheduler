using System;
using System.Collections.Generic;
using System.Data;
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
            var enrollments = EntityFetcher.FetchCourseEnrollments(courseId);
            ViewBag.Course = EntityFetcher.FetchCourseWithId(courseId);
            return View(enrollments);
        }

        public ActionResult PersonEnrollments(int personId)
        {
            var enrollments = EntityFetcher.FetchPersonEnrollments(personId);
            ViewBag.PersonName = enrollments.First().PersonName;
            return View(enrollments);
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
                ModelState.AddModelError("PersonId", "The participant no longer exists.");
                return View(enrollment);
            }
            return RedirectToAction("CourseEnrollments", new { courseId = enrollment.EnrollmentId });
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
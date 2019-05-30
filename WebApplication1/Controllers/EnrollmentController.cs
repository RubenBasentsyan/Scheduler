using System.Data;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels.Enrollments;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class EnrollmentController : Controller
    {
        [Authorize]
        // GET: Enrollment
        public ActionResult CourseEnrollments(int courseId)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                //TempData["Info"] = new {from = "course", id = courseId};
                ViewBag.Course = EntityFetcher.FetchCourseWithId(courseId);
                return View(EntityFetcher.FetchCourseEnrollments(courseId));
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult PersonEnrollments(int personId)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                ViewBag.Person = EntityFetcher.FetchParticipantWithId(personId);
                return View(EntityFetcher.FetchPersonEnrollments(personId));
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult CreateForCourse(int courseId)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                var enrollment = new CreateEnrollmentVm {CourseId = courseId};
                try
                {
                    ViewBag.CourseName = EntityFetcher.FetchCourseWithId(courseId).Name;
                    enrollment.ParticipantList = EntityFetcher.FetchParticipantsNotEnrolled(courseId);
                }
                catch (DataException)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(enrollment);
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateForCourse(CreateEnrollmentVm enrollment)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                try
                {
                    EntityModifier.CreateEnrollment(enrollment);
                }
                catch (DbEntityValidationException)
                {
                    ModelState.AddModelError("PersonId",
                        "Something went wrong, this participant likely no longer exists.");
                    return View(enrollment);
                }

                return RedirectToAction("CourseEnrollments", new {courseId = enrollment.CourseId});
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult CreateForParticipant(int participantId)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                var enrollment = new CreateEnrollmentVm {ParticipantId = participantId};
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

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateForParticipant(CreateEnrollmentVm enrollment)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                try
                {
                    EntityModifier.CreateEnrollment(enrollment);
                }
                catch (DbEntityValidationException)
                {
                    ModelState.AddModelError("CourseId", "Something went wrong, this course likely no longer exists.");
                    return View(enrollment);
                }

                return RedirectToAction("PersonEnrollments", new {personId = enrollment.ParticipantId});
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                TempData.Keep();
                try
                {
                    TempData["Url"] = Request?.UrlReferrer?.ToString() ?? "/Home/Index";
                    return View(EntityFetcher.FetchEnrollment(id));
                }
                catch (DataException)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(DisplayEnrollmentVm enrollment)
        {
            var isAdmin = EntityFetcher.FetchUserAdminStatus(Methods.GetUsernameFromCookie(HttpContext));
            ViewBag.isAdmin = isAdmin;
            if (isAdmin == true)
            {
                EntityModifier.DeleteEnrollment(enrollment.EnrollmentId);
                return Redirect(TempData["Url"].ToString());
            }

            return RedirectToAction("Login", "Home");
        }
    }
}
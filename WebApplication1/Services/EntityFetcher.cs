using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Models.ViewModels.Enrollments;

namespace WebApplication1.Services
{ 
    /// <summary>
    /// Encapsulates Data Access Methods 
    /// </summary>
    public static class EntityFetcher
    {
        // Course Fetchers 
        public static IEnumerable<CourseViewModel> FetchAllCourses()
        {
            using (var db = new SchedulerEntities())
                return db.Courses.Select(s => new CourseViewModel() { CourseId = s.Id, Name = s.Name }).ToList();
        }

        public static ParticipantsViewModel FetchLoginUser(string username, string password)
        {
            using (var db = new SchedulerEntities())
            {
                var dbUser =  db.Persons.Where(usr => usr.Username == username && usr.Password == password).SingleOrDefault();
                if(dbUser == null)
                {
                    throw new DataException("The user was not found");
                }
                return new ParticipantsViewModel() { PersonId=dbUser.Id, IsAdmin = dbUser.IsAdmin == true, Password = dbUser.Password , Name=dbUser.Name, Username=dbUser.Username };
            }
        }

        public static bool? FetchUserAdminStatus(string username)
        {
            using (var db = new SchedulerEntities())
            {
                var dbUser = db.Persons.Where(usr => usr.Username == username).SingleOrDefault();
                if (dbUser == null)
                {
                    throw new DataException("The user was not found");
                }
                return dbUser.IsAdmin;
            }
        }

        public static CourseViewModel FetchCourseWithId(int courseId)
        {
            using (var db = new SchedulerEntities())
            {
                var dbCourse = db.Courses.Find(courseId);
                if (dbCourse == null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                return new CourseViewModel(){CourseId = dbCourse.Id,Name = dbCourse.Name} ;
            }
        }
        /// <summary>
        /// Fetches the schedule.
        /// </summary>
        /// <returns>The schedule, ordered by Day and TimeSlot</returns>
        public static IEnumerable<ScheduleViewModel> FetchSchedule()
        {
            using (var db = new SchedulerEntities())
            {
                var schedule =
                    (from co in db.Colors
                    join cr in db.Courses on co.Course_Fk equals cr.Id
                    orderby co.Day,co.TimeSlot
                    select new ScheduleViewModel()
                    {
                        CourseId = co.Course_Fk,
                        Course = cr.Name,
                        Day = ((Day) co.Day).ToString(),
                        TimeSlot = ((TimeSlot) co.TimeSlot).ToString(),
                    }).ToList();

                return schedule;
            }
        }
        // Participant Fetcher 
        public static IEnumerable<ParticipantsViewModel> FetchAllParticipants()
        {
            using (var db = new SchedulerEntities())
            {
                return db.Persons.Where(w=>w.IsAdmin!=true).Select(s => new ParticipantsViewModel() { PersonId = s.Id, Name = s.Name }).ToList();
            }
        }
        public static ParticipantsViewModel FetchParticipantWithId(int participantId)
        {
            using (var db = new SchedulerEntities())
            {
                var dbParticipant= db.Persons.Find(participantId);
                if (dbParticipant == null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                return new ParticipantsViewModel() { PersonId = dbParticipant.Id, Name = dbParticipant.Name };
            }
        }
        // Enrollments Fetcher
        public static IEnumerable<DisplayEnrollmentVm> FetchCourseEnrollments(int CourseId)
        {
            using (var db = new SchedulerEntities())
            {
                var courseName = db.Courses.Find(CourseId)?.Name;
                return (from e in db.Entrollments
                    where e.Course_Fk == CourseId
                    join per in db.Persons on e.Person_Fk equals per.Id
                    select new DisplayEnrollmentVm()
                    {
                        EnrollmentId = e.EnrollmentId,
                        CourseId = e.Course_Fk,
                        CourseName = courseName,
                        PersonId = e.Person_Fk,
                        PersonName = per.Name
                    }).ToList();
            }
        }
        public static IEnumerable<DisplayEnrollmentVm> FetchPersonEnrollments(int PersonId)
        {
            using (var db = new SchedulerEntities())
            {
                var personName = db.Persons.Find(PersonId)?.Name;
                return (from e in db.Entrollments
                    where e.Person_Fk == PersonId
                    join crs in db.Courses on e.Course_Fk equals crs.Id
                    select new DisplayEnrollmentVm()
                    {
                        EnrollmentId = e.EnrollmentId,
                        PersonId = e.Person_Fk,
                        CourseName = crs.Name,
                        CourseId = e.Course_Fk,
                        PersonName = personName
                    }).ToList();
            }
        }

        public static IEnumerable<CourseViewModel> FetchCoursesNotEnrolled (int PersonId)
        {
            using (var db = new SchedulerEntities())
            {
                var enrolledCourseIds = new HashSet<int>(db.Persons.Find(PersonId).Entrollments.Select(w => w.Course_Fk));
                return db.Courses.Where(c => !enrolledCourseIds.Contains(c.Id)).Select(c => new CourseViewModel() { CourseId = c.Id, Name = c.Name }).ToList();

            }
        }

        public static IEnumerable<ParticipantsViewModel> FetchParticipantsNotEnrolled(int CourseId)
        {
            using (var db = new SchedulerEntities())
            {
                var enrolledParticipantIds = new HashSet<int>(db.Courses.Find(CourseId).Entrollments.Select(w => w.Person_Fk));
                return db.Persons.Where(c => !enrolledParticipantIds.Contains(c.Id)).Select(c => new ParticipantsViewModel() { PersonId = c.Id, Name = c.Name }).ToList();

            }
        }

        public static DisplayEnrollmentVm FetchEnrollment(int id)
        {
            using (var db = new SchedulerEntities())
            {
                var dbEnrollment = db.Entrollments.Find(id);
                if(dbEnrollment==null)
                    throw new DataException("No such enrollment exists");
                return new DisplayEnrollmentVm()
                {
                    CourseId = dbEnrollment.Course_Fk,
                    CourseName = db.Courses.Find(dbEnrollment.Course_Fk)
                        ?.Name,
                    EnrollmentId = dbEnrollment.EnrollmentId,
                    PersonId = dbEnrollment.Person_Fk,
                    PersonName = db.Persons.Find(dbEnrollment.Person_Fk)
                        ?.Name
                };
            }
        }   


    }
}
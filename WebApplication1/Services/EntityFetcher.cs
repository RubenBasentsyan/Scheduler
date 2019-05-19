using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                var schedule = (from cr in db.Courses
                                join co in db.Colors on cr.Id equals co.Course_Fk
                                select new ScheduleViewModel()
                                { Course = cr.Name, CourseId = cr.Id, TimeSlot = ((TimeSlot)co.TimeSlot).ToString(), Day = ((Day)co.Day).ToString()})
                    .OrderBy(w => w.Day).ThenBy(w=>w.TimeSlot).ToList();

                return schedule;
            }
        }
        // Participant Fetcher 
        public static IEnumerable<ParticipantsViewModel> FetchAllParticipants()
        {
            using (var db = new SchedulerEntities())
            {
                return db.Persons.Select(s => new ParticipantsViewModel() { PersonId = s.Id, Name = s.Name }).ToList();
            }
        }
        public static ParticipantsViewModel FetchParticipantWithId(int participantId)
        {
            using (var db = new SchedulerEntities())
            {
                var dbParticipant= db.Courses.Find(participantId);
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
    }
}
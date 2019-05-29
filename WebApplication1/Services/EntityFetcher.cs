using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Models.ViewModels.Enrollments;

namespace WebApplication1.Services
{
    /// <summary>
    ///     Encapsulates Data Access Methods
    /// </summary>
    public static class EntityFetcher
    {
        private static readonly int PageSize = 13;

        // Course Fetchers 
        public static IEnumerable<CourseViewModel> FetchAllCourses
        {
            get
            {
                using (var db = new SchedulerEntities())
                {
                    return db.Courses.Select(s => new CourseViewModel {CourseId = s.Id, Name = s.Name}).ToList();
                }
            }
        }

        public static int CoursesPageCount
        {
            get
            {
                using (var db = new SchedulerEntities())
                {
                    var entryCount = db.Courses.Count();
                    return entryCount % PageSize == 0 ? entryCount / PageSize : entryCount / PageSize + 1;
                }
            }
        }

        public static int SchedulePagesCount
        {
            get
            {
                using (var db = new SchedulerEntities())
                {
                    var entryCount = db.Colors.Count();
                    return entryCount % PageSize == 0 ? entryCount / PageSize : entryCount / PageSize + 1;
                }
            }
        }

        // Participant Fetcher 
        public static IEnumerable<ParticipantsViewModel> FetchAllParticipants
        {
            get
            {
                using (var db = new SchedulerEntities())
                {
                    return db.Persons.Where(w => w.IsAdmin != true)
                        .Select(s => new ParticipantsViewModel {PersonId = s.Id, Name = s.Name}).ToList();
                }
            }
        }

        public static int ParticipantsPageCount
        {
            get
            {
                using (var db = new SchedulerEntities())
                {
                    var entryCount = db.Persons.Count();
                    return entryCount % PageSize == 0 ? entryCount / PageSize : entryCount / PageSize + 1;
                }
            }
        }

        public static ParametersViewModel FetchSchedulingParameters => new ParametersViewModel
        {
            ConcurrencyLimit = Color.ConcurrencyLimit, Days = Color.MaxDays,
            TimeSlots = Color.MaxTimeSlots
        };

        public static IEnumerable<CourseViewModel> FetchCourses(int page)
        {
            using (var db = new SchedulerEntities())
            {
                return db.Courses.Select(s => new CourseViewModel
                    {
                        CourseId = s.Id,
                        Name = s.Name
                    })
                    .OrderBy(s => s.CourseId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
        }

        public static ParticipantsViewModel FetchLoginUser(string username, string password)
        {
            using (var db = new SchedulerEntities())
            {
                var dbUser = db.Persons.SingleOrDefault(usr => usr.Username == username && usr.Password == password);
                if (dbUser == null) throw new DataException("The user was not found");

                return new ParticipantsViewModel
                {
                    PersonId = dbUser.Id, IsAdmin = dbUser.IsAdmin == true, Password = dbUser.Password,
                    Name = dbUser.Name, Username = dbUser.Username
                };
            }
        }

        public static bool? FetchUserAdminStatus(string username)
        {
            using (var db = new SchedulerEntities())
            {
                var dbUser = db.Persons.SingleOrDefault(usr => usr.Username == username);
                if (dbUser == null) throw new DataException("The user was not found");

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
                return new CourseViewModel {CourseId = dbCourse.Id, Name = dbCourse.Name};
            }
        }

        /// <summary>
        ///     Fetches the schedule.
        /// </summary>
        /// <returns>The schedule, ordered by Day and TimeSlot</returns>
        public static IEnumerable<ScheduleViewModel> FetchSchedule()
        {
            using (var db = new SchedulerEntities())
            {
                var schedule =
                    (from co in db.Colors
                        join cr in db.Courses on co.Course_Fk equals cr.Id
                        orderby co.Day, co.TimeSlot
                        select new ScheduleViewModel
                        {
                            CourseId = co.Course_Fk,
                            Course = cr.Name,
                            Day = (co.Day + 1).ToString(),
                            TimeSlot = (co.TimeSlot + 1).ToString()
                        }).ToList();

                return schedule;
            }
        }

        public static IEnumerable<ScheduleViewModel> FetchSchedule(int page)
        {
            using (var db = new SchedulerEntities())
            {
                var schedule =
                    (from co in db.Colors
                        join cr in db.Courses on co.Course_Fk equals cr.Id
                        orderby co.Day, co.TimeSlot
                        select new ScheduleViewModel
                        {
                            CourseId = co.Course_Fk,
                            Course = cr.Name,
                            Day = (co.Day + 1).ToString(),
                            TimeSlot = (co.TimeSlot + 1).ToString()
                        }).Skip((page - 1) * PageSize).Take(PageSize).ToList();

                return schedule;
            }
        }

        public static IEnumerable<ParticipantsViewModel> FetchParticipants(int page)
        {
            using (var db = new SchedulerEntities())
            {
                return db.Persons.Where(w => w.IsAdmin != true)
                    .Select(s => new ParticipantsViewModel
                    {
                        PersonId = s.Id,
                        Name = s.Name
                    })
                    .OrderBy(s => s.PersonId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
        }

        public static ParticipantsViewModel FetchParticipantWithId(int participantId)
        {
            using (var db = new SchedulerEntities())
            {
                var dbParticipant = db.Persons.Find(participantId);
                if (dbParticipant == null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                return new ParticipantsViewModel {PersonId = dbParticipant.Id, Name = dbParticipant.Name};
            }
        }

        // Enrollments Fetcher
        public static IEnumerable<DisplayEnrollmentVm> FetchCourseEnrollments(int courseId)
        {
            using (var db = new SchedulerEntities())
            {
                var courseName = db.Courses.Find(courseId)?.Name;
                return (from e in db.Entrollments
                    where e.Course_Fk == courseId
                    join per in db.Persons on e.Person_Fk equals per.Id
                    select new DisplayEnrollmentVm
                    {
                        EnrollmentId = e.EnrollmentId,
                        CourseId = e.Course_Fk,
                        CourseName = courseName,
                        PersonId = e.Person_Fk,
                        PersonName = per.Name
                    }).ToList();
            }
        }

        public static IEnumerable<DisplayEnrollmentVm> FetchPersonEnrollments(int personId)
        {
            using (var db = new SchedulerEntities())
            {
                var personName = db.Persons.Find(personId)?.Name;
                return (from e in db.Entrollments
                    where e.Person_Fk == personId
                    join crs in db.Courses on e.Course_Fk equals crs.Id
                    select new DisplayEnrollmentVm
                    {
                        EnrollmentId = e.EnrollmentId,
                        PersonId = e.Person_Fk,
                        CourseName = crs.Name,
                        CourseId = e.Course_Fk,
                        PersonName = personName
                    }).ToList();
            }
        }

        public static IEnumerable<CourseViewModel> FetchCoursesNotEnrolled(int personId)
        {
            using (var db = new SchedulerEntities())
            {
                var enrolledCourseIds = new HashSet<int>(db.Persons.Find(personId)
                    .Entrollments.Select(w => w.Course_Fk));
                return db.Courses.Where(c => !enrolledCourseIds.Contains(c.Id))
                    .Select(c => new CourseViewModel
                    {
                        CourseId = c.Id,
                        Name = c.Name
                    })
                    .ToList();
            }
        }

        public static IEnumerable<ParticipantsViewModel> FetchParticipantsNotEnrolled(int courseId)
        {
            using (var db = new SchedulerEntities())
            {
                var enrolledParticipantIds =
                    new HashSet<int>(db.Courses.Find(courseId).Entrollments.Select(w => w.Person_Fk));
                return db.Persons.Where(c => !enrolledParticipantIds.Contains(c.Id))
                    .Select(c => new ParticipantsViewModel {PersonId = c.Id, Name = c.Name}).ToList();
            }
        }

        public static DisplayEnrollmentVm FetchEnrollment(int id)
        {
            using (var db = new SchedulerEntities())
            {
                var dbEnrollment = db.Entrollments.Find(id);
                if (dbEnrollment == null)
                    throw new DataException("No such enrollment exists");
                return new DisplayEnrollmentVm
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
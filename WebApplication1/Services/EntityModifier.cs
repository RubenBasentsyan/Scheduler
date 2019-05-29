using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using CsvHelper;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Models.ViewModels.Enrollments;

namespace WebApplication1.Services
{
    public static class EntityModifier
    {
        public static void CreateCourse(CourseViewModel course)
        {
            using (var db = new SchedulerEntities())
            {
                var dbCourse = new Courses {Name = course.Name};
                db.Courses.Add(dbCourse);
                db.SaveChanges();
            }
        }

        /// <summary>
        ///     Sets the scheduling parameters.
        ///     If true, parent controller method must truncate the Colors table t (optional), and notify the user that the current
        ///     schedule is redundant
        ///     If false, parent controller method must  notify the user that a better schedule may be possible, and suggest
        ///     rescheduling
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Whether or not any of the parameters became stricter.</returns>
        public static bool SetSchedulingParameters(ParametersViewModel parameters)
        {
            return (parameters.Days != Color.MaxDays && Color.SetConcurrencyLimit(parameters.ConcurrencyLimit)) |
                   (parameters.TimeSlots != Color.MaxTimeSlots && Color.SetMaxDays(parameters.Days)) |
                   (parameters.ConcurrencyLimit != Color.ConcurrencyLimit &&
                    Color.SetMaxTimeSlots(parameters.TimeSlots));
        }

        public static void EditCourse(CourseViewModel course)
        {
            using (var db = new SchedulerEntities())
            {
                var dbCourse = db.Courses.Find(course.CourseId);
                if (dbCourse == null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                db.Entry(dbCourse).CurrentValues.SetValues(course);
                db.SaveChanges();
            }
        }

        public static void DeleteCourse(CourseViewModel course)
        {
            using (var db = new SchedulerEntities())
            {
                var dbCourse = db.Courses.Find(course.CourseId);
                if (dbCourse == null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                db.Courses.Remove(dbCourse);
                db.SaveChanges();
            }
        }

        public static void Reschedule()
        {
            Graph.PopulateGraph();
            Graph.ColorGraph();
            using (var db = new SchedulerEntities())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Colors]");
                db.Colors.AddRange(Graph.GraphInstance.Vertices.Select(cl => new Colors
                    {Course_Fk = cl.Id, Day = cl.Color.Day, TimeSlot = cl.Color.TimeSlot}).AsEnumerable());
                db.SaveChanges();
            }
        }

        public static void CreateParticipant(ParticipantsViewModel participant)
        {
            using (var db = new SchedulerEntities())
            {
                var dbPerson = new Persons
                {
                    Name = participant.Name, Username = participant.Username, Password = participant.Password,
                    IsAdmin = participant.IsAdmin
                };
                db.Persons.Add(dbPerson);
                db.SaveChanges();
            }
        }

        public static void EditParticipant(ParticipantsViewModel participant)
        {
            using (var db = new SchedulerEntities())
            {
                var dbPerson = db.Persons.Find(participant.PersonId);
                if (dbPerson == null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                db.Entry(dbPerson).CurrentValues.SetValues(participant);
                db.SaveChanges();
            }
        }

        public static void DeleteParticipant(ParticipantsViewModel participant)
        {
            using (var db = new SchedulerEntities())
            {
                var dbPerson = db.Courses.Find(participant.PersonId);
                if (dbPerson == null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                db.Courses.Remove(dbPerson);
                db.SaveChanges();
            }
        }

        public static void CreateEnrollment(CreateEnrollmentVm enrollment)
        {
            using (var db = new SchedulerEntities())
            {
                var dbEnrollment = new Entrollments
                    {Course_Fk = enrollment.CourseId, Person_Fk = enrollment.ParticipantId};
                db.Entrollments.Add(dbEnrollment);
                db.SaveChanges();
            }
        }

        public static void DeleteEnrollment(int id)
        {
            using (var db = new SchedulerEntities())
            {
                var dbEnrollment = new Entrollments {EnrollmentId = id};
                db.Entry(dbEnrollment).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public static void PopulateTablesFromCsv(Stream csvStream)
        {
            var students = new Dictionary<int, string>();
            var courses = new Dictionary<int, string>();
            var enrollments = new HashSet<Entrollments>();
            using (var reader = new CsvReader(new StreamReader(csvStream)))
            {
                reader.Read();
                var courseIndex = 1;
                for (;;)
                    try
                    {
                        courses.Add(courseIndex, reader[courseIndex]);
                        courseIndex++;
                    }
                    catch
                    {
                        break;
                    }

                var lineCount = 1;
                while (reader.Read())
                {
                    students.Add(lineCount, reader[0]);
                    for (var i = 1; i < courseIndex; i++)
                        if (reader[i] == "IP")
                            enrollments.Add(new Entrollments {Person_Fk = lineCount, Course_Fk = i});
                    lineCount++;
                }

                using (var db = new SchedulerEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "TRUNCATE TABLE [Colors]\r\nTRUNCATE TABLE [Entrollments]\r\nDELETE FROM [Courses]\r\nDBCC CHECKIDENT ('Courses',Reseed,0)\r\nDELETE FROM [Persons]\r\nDBCC CHECKIDENT ('Persons',Reseed,0)");
                    db.Courses.AddRange(courses.Select(c => new Courses {Name = c.Value}));
                    db.Persons.AddRange(students.Select(s => new Persons {Name = s.Value}));
                    db.Persons.Add(new Persons
                        {Name = "admin", IsAdmin = true, Password = "admin", Username = "admin"});
                    db.SaveChanges();
                    db.Entrollments.AddRange(enrollments);
                    db.SaveChanges();
                }
            }
        }
    }
}
using System.Data;
using System.Linq;
using WebApplication1.Helpers;
using WebApplication1.Models.ViewModels;
using WebApplication1.Models.ViewModels.Enrollments;

namespace WebApplication1.Services
{
    public static class EntityModifier
    {
        public static void CreateCourse( CourseViewModel course )
        {
            using (var db = new SchedulerEntities())
            {
                var dbCourse = new Courses() { Name = course.Name };
                db.Courses.Add(dbCourse);
                db.SaveChanges();
            }
        }

        public static void EditCourse ( CourseViewModel course )
        {
            using (var db = new SchedulerEntities())
            {
                var dbCourse = db.Courses.Find(course.CourseId);
                if(dbCourse==null)
                    throw new DataException(
                        "The table Course does not contain an entry corresponding to the provided primary key");
                db.Entry(dbCourse).CurrentValues.SetValues(course);
                db.SaveChanges();
            }
        }

        public static void DeleteCourse( CourseViewModel course)
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
                db.Colors.AddRange(Graph.GraphInstance.Vertices.Select(cl => new Colors()
                { Course_Fk = cl.Id, Day = (int)cl.Color.day, TimeSlot = (int)cl.Color.timeSlot }).AsEnumerable());
                db.SaveChanges();
            }
        }
        public static void CreateParticipant(ParticipantsViewModel participant)
        {
            using (var db = new SchedulerEntities())
            {
                var dbPerson = new Persons() { Name = participant.Name };
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

        public static void CreateEnrollment (CreateEnrollmentVm enrollment)
        {
            using (var db  = new SchedulerEntities())
            {
                var dbEnrollment = new Entrollments() { Course_Fk = enrollment.CourseId, Person_Fk = enrollment.ParticipantId };
                db.Entrollments.Add(dbEnrollment);
                db.SaveChanges();
            }
        }
    }
}
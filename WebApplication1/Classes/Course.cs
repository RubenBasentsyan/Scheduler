using System;
using System.Collections.Generic;
using WebApplication1.Helpers;

namespace WebApplication1.Classes
{
    public class Course : IEquatable<Course>
    {
        private Course(
            Courses crs /*int id, string name, HashSet<Person> participants,  HashSet<Course> adjacentCourses*/)
        {
            Id = crs.Id;
            Name = crs.Name;
            //this.participants = participants;
            //this.adjacentCourses = adjacentCourses;
        }

        public int Id { get; set; }
        public string Name { get; /*private set;*/ }
        public Color Color { get; set; }

        /// <summary>
        ///     both teachers and students of the course
        /// </summary>
        public HashSet<int> ParticipantIds { get; set; }


        public bool Equals(Course other)
        {
            return other != null && Id == other.Id;
        }

        public static explicit operator Course(Courses c)
        {
            return new Course(c);
        }

        public bool CompareStudents(Course course)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Course && obj.GetHashCode() == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
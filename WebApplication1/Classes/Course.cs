using System;
using System.Collections.Generic;

namespace WebApplication1.Classes
{
    public class Course : IEquatable<Course>
	{
        public int id { get ; set; }
        public string name { get; private set; }

		/// <summary>
		/// both teachers and students of the course
		/// </summary>
        public HashSet<int> participantIds { get; set; }
		//private HashSet<Course> adjacentCourses { get; set; }
        public Course( Courses crs  /*int id, string name, HashSet<Person> participants,  HashSet<Course> adjacentCourses*/)
        {

            this.id = crs.Id;
            this.name = crs.Name;
            //this.participants = participants;
            //this.adjacentCourses = adjacentCourses;
        }

        public static explicit operator Course (Courses c)
        {
            return new Course(c);
        }

        public bool compareStudents(Course course)
        {
            return true;
        }
		
		public bool Equals(Course other) => other == null ? false : id == other.id;
		public override bool Equals(object obj) => obj == null || obj.GetType() != typeof(Course) ? false : obj.GetHashCode() == id;
		public override int GetHashCode() => id;
	}

	public class CustomTupleComparer : IEqualityComparer<(int,int)>
	{
		public bool Equals((int,int) x, (int,int) y) => (x.Item1 == y.Item1 && x.Item2 == y.Item2) || (x.Item1 == y.Item2 && x.Item2 == y.Item1);

		public int GetHashCode((int, int)obj) => obj.Item1>obj.Item2? obj.GetHashCode() : (obj.Item2,obj.Item1).GetHashCode() ;
		
	}


}
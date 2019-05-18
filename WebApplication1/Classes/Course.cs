using System;
using WebApplication1.Helpers;
using System.Collections.Generic;

namespace WebApplication1.Classes
{
    public class Course : IEquatable<Course>
	{
        public int Id { get ; set; }
        public string Name { get; /*private set;*/ }
        public Color Color { get;  set; }
		/// <summary>
		/// both teachers and students of the course
		/// </summary>
        public HashSet<int> participantIds { get; set; }
		
        private Course( Courses crs  /*int id, string name, HashSet<Person> participants,  HashSet<Course> adjacentCourses*/)
        {

            this.Id = crs.Id;
            this.Name = crs.Name;
            //this.participants = participants;
            //this.adjacentCourses = adjacentCourses;
        }

        public static explicit operator Course (Courses c)
        {
            return new Course(c);
        }

        public bool CompareStudents(Course course)
        {
            return true;
        }
        

		public bool Equals(Course other) => other != null && Id == other.Id;
		public override bool Equals(object obj) => obj != null && obj.GetType() == typeof(Course) && obj.GetHashCode() == Id;
		public override int GetHashCode() => Id.GetHashCode();
        
    }

	public class CustomTupleComparer : IEqualityComparer<(int,int)>
	{
		public bool Equals((int,int) x, (int,int) y) => (x.Item1 == y.Item1 && x.Item2 == y.Item2) || (x.Item1 == y.Item2 && x.Item2 == y.Item1);

		public int GetHashCode((int, int)obj) => obj.Item1>obj.Item2? obj.GetHashCode() : (obj.Item2,obj.Item1).GetHashCode() ;
		
	}


}
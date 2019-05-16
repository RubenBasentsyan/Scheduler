using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuickGraph.Data;
using QuickGraph.Algorithms;
using System.Data;

namespace WebApplication1.Models
{
    public class Course : IEquatable<Course>
	{
        public int id { get ; set; }
        private string name { get; set; }
		/// <summary>
		/// both teachers and students of the course
		/// </summary>
        public HashSet<Person> participants { get; set; }
		private HashSet<Course> adjacentCourses { get; set; }

		public Course(int id, string name, HashSet<Person> participants,  HashSet<Course> adjacentCourses)
        {
            this.id = id;
            this.name = name;
            this.participants = participants;
            this.adjacentCourses = adjacentCourses;
        }

        public bool compareStudents(Course course)
        {
            return true;
        }
		
		public bool Equals(Course other) => other == null ? false : id == other.id;
		public override bool Equals(object obj) => obj == null || obj.GetType() != typeof(Course) ? false : obj.GetHashCode() == id;
		public override int GetHashCode() => id;
	}



}
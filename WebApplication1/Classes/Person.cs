﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Models
{
	/// <summary>
	/// Student or Teacher
	/// </summary>
    public class Person
    {
        public int id { get; set; }
        private string name { get; set; }
		private HashSet<Course> course { get; set; }

		public Person(int id, string name, HashSet<Course> courses)
        {
            this.id = id;
            this.name = name;
			course = course;
        }
		public override bool Equals(object obj) => obj == null || obj.GetType() != typeof(Person) ? false : obj.GetHashCode() == id;
		public override int GetHashCode() => id;
	}
}
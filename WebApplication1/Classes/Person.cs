using System.Collections.Generic;

namespace WebApplication1.Classes
{
    /// <summary>
    ///     Student or Teacher
    /// </summary>
    public class Person
    {
        public Person(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        private HashSet<Course> Course { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Person && obj.GetHashCode() == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
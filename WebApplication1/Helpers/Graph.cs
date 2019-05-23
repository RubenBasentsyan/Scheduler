using System;
using QuickGraph;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Classes;

namespace WebApplication1.Helpers
{
    public static class Graph
    {
        public static UndirectedGraph<Course, Edge> GraphInstance { get; private set; }
        /// <summary>
        /// Populates the graph.
        /// </summary>
        public static void PopulateGraph()
        {
            Color.ResetLimits();
            using (SchedulerEntities db = new SchedulerEntities())
            {
                GraphInstance = new UndirectedGraph<Course, Edge>(false);
                Course temp;
                foreach (Courses course in db.Courses)
                {
                    temp = (Course)course;
                    temp.participantIds = new HashSet<int>(course.Entrollments.Select(e => e.Person_Fk));
                    GraphInstance.AddVertex(temp);
                }
                PopulateEdges();
            }
        }
        /// <summary>
        /// Populates the edges.
        /// </summary>
        private static void PopulateEdges()
        {
            foreach (Course first in GraphInstance.Vertices)
            {
                foreach (Course second in GraphInstance.Vertices)
                {
                    if (GraphInstance.ContainsEdge(first, second))
                        continue;
                    if (!first.Equals(second) && first.participantIds.Overlaps(second.participantIds))
                    {
                        HashSet<int> intersection = new HashSet<int>(first.participantIds);
                        intersection.IntersectWith(second.participantIds);
                        GraphInstance.AddEdge(new Edge(first, second, intersection.Count));
                    }
                }
            }
        }
        /// <summary>
        /// Colors the graph.
        /// </summary>
        public static void ColorGraph()
        {
            foreach (var course in GraphInstance.Vertices.OrderByDescending(crs => GraphInstance.AdjacentDegree(crs))
                .ThenByDescending(crs => GraphInstance.AdjacentEdges(crs)
                    .Sum(e => e.weight)))
            {
                if (course.Color != null)
                    continue;
                ColorAppropriately(course);
                foreach (var ret in GetAdjacentVertices(course))
                    ColorAppropriately(ret);
            }
        }
        /// <summary>
        /// Gets the courses that have at least one common student with crs
        /// </summary>
        /// <param name="crs">The course</param>
        /// <returns>The li</returns>
        /// <exception cref="ArgumentNullException">When course is null</exception>
        private static IEnumerable<Course> GetAdjacentVertices(Course crs)
        {
            if (GraphInstance != null)
                return GraphInstance.AdjacentEdges(crs)
                    .Select(edge => edge.Source.Equals(crs)
                        ? edge.Target
                        : edge.Source)
                    .OrderByDescending(course => GraphInstance.AdjacentDegree(course))
                    .ThenByDescending(course => GraphInstance.AdjacentEdges(course)
                        .Sum(e => e.weight))
                    .ToList();
            throw new ArgumentNullException();
        }
        /// <summary>
        /// Colors a course with the best possible color
        /// </summary>
        /// <param name="crs">The CRS.</param>
        private static void ColorAppropriately(Course crs)
        {
            bool valid = true;
            TimeSlot t;
            foreach (var d in (Day[])Enum.GetValues(typeof(Day)))
            {
                for (t = 0; (int)t < Enum.GetNames(typeof(TimeSlot)).Length; t++)
                {
                    valid = true;
                    if (Color.EnoughRoomExists(d, t) && GetAdjacentVertices(crs).All(adjCourse =>
                            (adjCourse.Color == null || adjCourse.Color.Day != d || adjCourse.Color.TimeSlot != t)) &&
                        ThreeExamConstraint(crs, d, t))
                        break;
                    valid = false;
                }
                if (!valid) continue;
                Color.SetCourseColor(d, t, crs);
                return;
            }
            throw new Exception("Impossible to make a schedule");
        }
        /// <summary>
        /// Checks whether the 3 exam constraint is satisfied.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <param name="day">The day.</param>
        /// <param name="timeSlot">The time slot.</param>
        /// <returns>True if the 3 exam constraint is satisfied, false o.w.</returns>
        private static bool ThreeExamConstraint(Course course, Day day, TimeSlot timeSlot)
        {
            foreach (var student in course.participantIds)
            {
                if (((TimeSlot[])Enum.GetValues(typeof(TimeSlot))).Count(t => GraphInstance.Vertices.Any(crs =>
                       crs.Color != null && crs.Color.Day == day && crs.Color.TimeSlot == t &&
                       crs.participantIds.Contains(student))) > 1)
                    return false;
            }
            return true;
        }
    }
}
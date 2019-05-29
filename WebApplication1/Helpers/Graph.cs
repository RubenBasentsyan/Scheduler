using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using WebApplication1.Classes;

namespace WebApplication1.Helpers
{
    public static class Graph
    {
        public static UndirectedGraph<Course, Edge> GraphInstance { get; private set; }

        /// <summary>
        ///     Populates the graph.
        /// </summary>
        public static void PopulateGraph()
        {
            Color.ResetLimits();
            using (var db = new SchedulerEntities())
            {
                GraphInstance = new UndirectedGraph<Course, Edge>(false);
                foreach (var course in db.Courses)
                {
                    var temp = (Course) course;
                    temp.ParticipantIds = new HashSet<int>(course.Entrollments.Select(e => e.Person_Fk));
                    GraphInstance.AddVertex(temp);
                }

                PopulateEdges();
            }
        }

        /// <summary>
        ///     Populates the edges.
        /// </summary>
        private static void PopulateEdges()
        {
            foreach (var first in GraphInstance.Vertices)
            foreach (var second in GraphInstance.Vertices)
            {
                if (GraphInstance.ContainsEdge(first, second))
                    continue;
                if (!first.Equals(second) && first.ParticipantIds.Overlaps(second.ParticipantIds))
                {
                    var intersection = new HashSet<int>(first.ParticipantIds);
                    intersection.IntersectWith(second.ParticipantIds);
                    GraphInstance.AddEdge(new Edge(first, second, intersection.Count));
                }
            }
        }

        /// <summary>
        ///     Colors the graph.
        /// </summary>
        public static void ColorGraph()
        {
            foreach (var course in GraphInstance.Vertices.OrderByDescending(crs => GraphInstance.AdjacentDegree(crs))
                .ThenByDescending(crs => GraphInstance.AdjacentEdges(crs)
                    .Sum(e => e.Weight)))
            {
                if (course.Color != null)
                    continue;
                ColorAppropriately(course);
                foreach (var ret in GetAdjacentVertices(course))
                    ColorAppropriately(ret);
            }
        }

        /// <summary>
        ///     Gets the courses that have at least one common student with crs
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
                        .Sum(e => e.Weight))
                    .ToList();
            throw new ArgumentNullException();
        }

        /// <summary>
        ///     Colors a course with the best possible color
        /// </summary>
        /// <param name="crs">The CRS.</param>
        private static void ColorAppropriately(Course crs)
        {
            var valid = true;
            int timeSlot;
            for (var d = 0; d < Color.MaxDays; d++)
            {
                for (timeSlot = 0; timeSlot < Color.MaxTimeSlots; timeSlot++)
                {
                    valid = true;
                    if (Color.EnoughRoomExists(d, timeSlot) && GetAdjacentVertices(crs).All(adjCourse =>
                            adjCourse.Color == null || adjCourse.Color.Day != d ||
                            adjCourse.Color.TimeSlot != timeSlot) &&
                        ThreeExamConstraint(crs, d, timeSlot))
                        break;
                    valid = false;
                }

                if (!valid) continue;
                Color.SetCourseColor(d, timeSlot, crs);
                return;
            }

            throw new ImpossibleScheduleException();
        }

        /// <summary>
        ///     Checks whether the 3 exam constraint is satisfied.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <param name="day">The day.</param>
        /// <param name="timeSlot">The time slot.</param>
        /// <returns>True if the 3 exam constraint is satisfied, false o.w.</returns>
        private static bool ThreeExamConstraint(Course course, int day, int timeSlot)
        {
            foreach (var student in course.ParticipantIds)
            {
                var count = 0;
                for (var i = 0; i < Color.MaxTimeSlots; i++)
                {
                    if (count > 1)
                        return false;
                    count += GraphInstance.Vertices.Any(crs =>
                        crs.Color != null && crs.Color.Day == day && crs.Color.TimeSlot == i &&
                        crs.ParticipantIds.Contains(student))
                        ? 1
                        : 0;
                }
            }

            return true;
        }
    }
}
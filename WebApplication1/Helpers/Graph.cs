using System;
using QuickGraph;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Classes;

namespace WebApplication1.Helpers
{
    public class Graph
    {
        public UndirectedGraph<Course, Edge> graph;

        public Graph()
        {
            //degrees_totalWeights = new Dictionary<int, (int, int)>();
            using (SchedulerEntities db = new SchedulerEntities())
            {
                graph = new UndirectedGraph<Course, Edge>(false);
                Course temp;
                foreach (Courses course in db.Courses)
                {
                    temp = (Course)course;
                    temp.participantIds = new HashSet<int>(course.Entrollments.Select(e => e.Person_Fk));
                    graph.AddVertex(temp);
                }
                PopulateEdges();
            }
        }

        private void PopulateEdges()
        {
            foreach (Course first in graph.Vertices)
            {
                foreach (Course second in graph.Vertices)
                {
                    if (graph.ContainsEdge(first, second))
                        continue;
                    if (!first.Equals(second) && first.participantIds.Overlaps(second.participantIds))
                    {
                        HashSet<int> intersection = new HashSet<int>(first.participantIds);
                        intersection.IntersectWith(second.participantIds);
                        graph.AddEdge(new Edge(first, second, intersection.Count));
                    }
                }
            }
        }
        public void ColorGraph()
        {
            foreach (var course in graph.Vertices.OrderByDescending(crs => graph.AdjacentDegree(crs)).ThenByDescending(crs => graph.AdjacentEdges(crs).Sum(e=>e.weight)))
            {
                if (course.Color != null) continue;
                ColorAppropriately(course);
                foreach (var ret in GetAdjacentVertices(course)) ColorAppropriately(ret);
            }
        }

        private IEnumerable<Course> GetAdjacentVertices(Course crs)
        {
            if (graph != null)
                return graph.AdjacentEdges(crs)
                    .Select(edge => edge.Source.Equals(crs) ? 
                        edge.Target : edge.Source)
                    .ToList();
            throw new ArgumentNullException();
        }

        private bool ColorAppropriately(Course crs)
        {
            bool valid = true;
            TimeSlot t;
            foreach(Day d in (Day[])Enum.GetValues(typeof(Day)))
            {
                for (t = 0; (int) t < Enum.GetNames(typeof(TimeSlot)).Length; t++)
                {
                    valid = true;
                    if (!Color.EnoughRoomExists(d, t))
                    {
                        valid = false;
                        continue;
                    }
                    if (!GetAdjacentVertices(crs).All(adjCourse =>
                        (adjCourse.Color == null || adjCourse.Color.day != d || adjCourse.Color.timeSlot != t)))
                    {
                        valid = false;
                        continue;
                    }
                    if (!ThreeExamConstraint(crs, d, t))
                    {
                        valid = false;
                        continue;
                    }
                    break;
                }
                if (!valid) continue;
                Color.SetCourseColor(d, t, crs);
                return true;
            }
            return false;
        }

        private bool ThreeExamConstraint(Course course, Day day, TimeSlot timeSlot)
        {
            foreach (var student in course.participantIds)
            {
                //if (((TimeSlot[])Enum.GetValues(typeof(TimeSlot)))
                //        .Any(t => 
                //            graph.Vertices
                //                .Count(crs => 
                //                    crs.Color != null && crs.Color.day == day && crs.Color.timeSlot == t &&
                //                    crs.participantIds.Contains(student)) 
                //            > 1))
                int counter = 0;
                foreach (TimeSlot t in (TimeSlot[]) Enum.GetValues(typeof(TimeSlot)))
                {
                    if (graph.Vertices.Any(crs =>
                        crs.Color != null && crs.Color.day == day && crs.Color.timeSlot == t &&
                        crs.participantIds.Contains(student)))
                        counter++;
                    if (counter > 1)
                        return false;
                }
            }
            return true;
        }
    }
}
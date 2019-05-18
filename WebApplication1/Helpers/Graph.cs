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
                    if (graph.ContainsEdge(first,second))
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
        private void ColorGraph()
        {
           
        }
    }
}
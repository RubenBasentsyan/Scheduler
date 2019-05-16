using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public class Graph
    {
        private HashSet<Course> nodes;
		private Dictionary<Tuple<int,int>,int> edgeMatrix;
        private CourseDAL courseDAL; 

        public Graph()
        {
			nodes = courseDAL.getAllCourses();
			edgeMatrix = new Dictionary<Tuple<int, int>, int>();
			//edges = new HashSet<Edge>();
            PopulateEdgeMatrix();
        }

        private void PopulateEdgeMatrix()
        {
			foreach (Course first in nodes)
			{
				foreach (Course second in nodes)
				{
					if (first.Equals(second))
					{
						edgeMatrix[first.id, second.id] = 0;
						continue;
					}
					if (!first.participants.Overlaps(second.participants))
					{
						edgeMatrix[first.id, second.id] = 0;
						edgeMatrix[second.id, first.id] = 0;
						continue;
					}
					if (edgeMatrix[first.id, second.id] == 0)
					{
						HashSet<Person> intersection = new HashSet<Person>(first.participants);
						intersection.IntersectWith(second.participants);
						edgeMatrix[first.id, second.id] = intersection.Count;
						edgeMatrix[second.id, first.id] = intersection.Count;
					}
				}
			}
        }
    }
}
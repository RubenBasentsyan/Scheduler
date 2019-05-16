using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public class Edge
    {
        private Course startNode { get; set; }
        private Course endNode { get; set; }
		private int weight { get; set; }

		public Edge(Course startNode, Course endNode, int weight)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.weight = weight;
        }

        public Edge(Course startNode, Course endNode)
        {
            this.startNode = startNode;
            this.endNode = endNode;
        }

        public Edge()
        {
        }

        public int calculateWeight()
        {
            return 0;
        }
    }
}
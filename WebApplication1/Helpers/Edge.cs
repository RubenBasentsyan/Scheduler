using QuickGraph;
using WebApplication1.Classes;

namespace WebApplication1.Helpers
{
    public class Edge : IEdge<Course>
    {
        public Edge(Course startNode, Course endNode, int weight) : this(startNode, endNode)
        {
            Weight = weight;
        }

        private Edge(Course startNode, Course endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
        }

        private Course StartNode { get; }
        private Course EndNode { get; }
        public int Weight { get; }

        public Course Source => StartNode;

        public Course Target => EndNode;
    }
}
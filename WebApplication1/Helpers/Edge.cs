using WebApplication1.Classes;

namespace WebApplication1.Helpers
{
	public class Edge :QuickGraph.IEdge<Course>
    {
        private Course startNode { get;  set; }
        private Course endNode { get; set; }
		public int weight { get; private set; }

        public Course Source => startNode;

        public Course Target => endNode;

        public Edge(Course startNode, Course endNode, int weight) : this(startNode,endNode)
        {
            this.weight = weight;
        }

        private Edge(Course startNode, Course endNode)
        {
            this.startNode = startNode;
            this.endNode = endNode;
        }

        public int calculateWeight()
        {
            return 0;
        }
    }
}
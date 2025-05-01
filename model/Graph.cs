namespace MAP_routing.model
{
    public class Intersection
    {
        public int InterscID;
        public double Xcoordinate, Ycoordinate;

        public Intersection(int ID, double X, double Y)
        {
            InterscID = ID;
            Xcoordinate = X;
            Ycoordinate = Y;
        }

    }
    public class Road
    {
        public int FromInterscID, ToInterscID;
        public double RoadLength;
        public List<double> RoadSpeeds;
        public int SpeedIntrv;

        public Road(int fromInterscID, int toInterscID, double roadLength, List<double> roadSpeeds, int speedIntrv)
        {
            FromInterscID = fromInterscID;
            ToInterscID = toInterscID;
            RoadLength = roadLength;
            RoadSpeeds = roadSpeeds;
            SpeedIntrv = speedIntrv;
        }
        public double GetTravelTime(double startTime = 0)
        {
            // Normal case : speed is constant 

            int minutesinOnehour = 60;
            if (SpeedIntrv == 0) return (RoadLength / RoadSpeeds[0]) * minutesinOnehour;


            // Bonus case : speed is changing each time interval

            int intervalIndex = (int)(startTime / SpeedIntrv) % RoadSpeeds.Count;
            double speed = RoadSpeeds[intervalIndex];
            return (RoadLength / speed) * 60;

        }
    }
    internal class Graph
    {
        public Dictionary<int, Intersection> Nodes = new();
        public Dictionary<int, List<Road>> adjList = new();

        public void AddNode(Intersection node)
        {
            Nodes[node.InterscID] = node;
        }
        public void AddEdge(Road Edge)
        {
            if (!Nodes.ContainsKey(Edge.FromInterscID) || !Nodes.ContainsKey(Edge.ToInterscID))
                throw new ArgumentException("Invalid intersection ID in road");


            if (!adjList.ContainsKey(Edge.FromInterscID))
                adjList[Edge.FromInterscID] = new List<Road>();
            if (!adjList.ContainsKey(Edge.ToInterscID))
                adjList[Edge.ToInterscID] = new List<Road>();

            adjList[Edge.FromInterscID].Add(Edge);
            adjList[Edge.ToInterscID].Add(new Road(Edge.ToInterscID, Edge.FromInterscID, Edge.RoadLength, new List<double>(Edge.RoadSpeeds), Edge.SpeedIntrv));

        }

    }
}

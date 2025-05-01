namespace MAP_routing.model
{
    internal class inputReading
    {
        public Graph ParseMapFile(string filePath)
        {
            var graph = new Graph();
            var lines = File.ReadAllLines(filePath);
            int index = 0;
            int N = int.Parse(lines[index++]);
            for (int i = 0; i < N; i++)
            {
                var parts = lines[index++].Split();
                int id = int.Parse(parts[0]);
                double x = double.Parse(parts[1]), y = double.Parse(parts[2]);
                Intersection Node = new Intersection(id, x, y);
                graph.AddNode(Node);
            }
            var roadInfo = lines[index++].Split();
            int M = int.Parse(roadInfo[0]);
            int speedCount = 1, speedInterval = 0;


            if (roadInfo.Length == 3)
            {
                speedCount = int.Parse(roadInfo[1]);
                speedInterval = int.Parse(roadInfo[2]);
            }

            for (int i = 0; i < M; i++)
            {
                var parts = lines[index++].Split();
                int fromId = int.Parse(parts[0]), toId = int.Parse(parts[1]);
                double length = double.Parse(parts[2]);
                var speeds = new List<double>();


                for (int j = 3; j < parts.Length; j++)
                    speeds.Add(double.Parse(parts[j]));

                Road road = new Road(fromId, toId, length, speeds, speedInterval);
                graph.AddEdge(road);
            }

            return graph;
        }
        public static List<Query> ParseQueryFile(string filePath)
        {
            var queries = new List<Query>();
            var lines = File.ReadAllLines(filePath);
            int Q = int.Parse(lines[0]);

            for (int i = 1; i <= Q; i++)
            {
                var parts = lines[i].Split();
                queries.Add(new Query
                {
                    SourceX = double.Parse(parts[0]),
                    SourceY = double.Parse(parts[1]),
                    DestX = double.Parse(parts[2]),
                    DestY = double.Parse(parts[3]),
                    MaxWalkDistance = int.Parse(parts[4])
                });
            }
            return queries;
        }
        public class Query
        {
            public double SourceX { get; set; }
            public double SourceY { get; set; }
            public double DestX { get; set; }
            public double DestY { get; set; }
            public int MaxWalkDistance { get; set; }
        }
    }
}

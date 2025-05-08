using System.Diagnostics;
using MAP_routing.model;

public class Algorithm
{
    double total_time_including_IO = 0;
    static int total_time_of_min_path_finding = 0;
    static double total_time_of_Astar = 0;
    static double total_t_min = 0;
    static double maxSpeedKmh;
    static List<Query> queries;
    public List<Node> graph;

    public Algorithm(List<Node> _graph, double _maxSpeedKmh,List<Query>_queries)
    {
        if (_graph == null || _graph.Count == 0)
        {
            throw new ArgumentException("Graph cannot be null or empty.");
        }

        graph = _graph;

        maxSpeedKmh = _maxSpeedKmh;

            queries = _queries;
    }

    public List<PathResult> ProcessQueries(string outputFile)
    {
            

        var swTotal = Stopwatch.StartNew();
        var results = new List<PathResult>();
        long totalQueryTimeMs = 0;

        foreach (var query in queries)
        {
            var swQuery = Stopwatch.StartNew();
            var result = Min_Path_Finding(query);
            
            swQuery.Stop();
            //Console.WriteLine(" time =\t " + swQuery.ElapsedMilliseconds);

            totalQueryTimeMs += swQuery.ElapsedMilliseconds;
            results.Add(result);
        }

        swTotal.Stop();
        long totalTimeMs = swTotal.ElapsedMilliseconds;


        // Write output
        using (var writer = new StreamWriter(outputFile))
        {
            foreach (var result in results)
            {
                writer.WriteLine(string.Join(" ", result.Path));
                writer.WriteLine($"{result.TotalTimeMin:F2} mins");
                writer.WriteLine($"{result.TotalDistanceKm:F2} km");
                writer.WriteLine($"{result.WalkingDistanceKm:F2} km");
                writer.WriteLine($"{result.VehicleDistanceKm:F2} km");
                writer.WriteLine();
            }
                
            writer.WriteLine($"{totalTimeMs}ms");
            writer.WriteLine($"");
            writer.WriteLine($"{total_time_including_IO}ms");
        }
        Console.WriteLine(" time =\t " + total_time_including_IO);

        return results;
    }

    private PathResult Min_Path_Finding(Query query)
    {
        Stopwatch s = Stopwatch.StartNew();
        const double walkingSpeedKmh = 5.0;
        double walk_limit_in_km = query.Rmeters / 1000;

        var sourceSuperNode = new Node { Id = -1, X = query.SourceX, Y = query.SourceY };
        var destSuperNode = new Node { Id = -2, X = query.DestX, Y = query.DestY };

        var sourceWalkDists = new Dictionary<int, double>();
        var destWalkDists = new Dictionary<int, double>();

        foreach (var node in graph)
        {
            double distToSource = Euclidean_Distance_Km(node.X, node.Y, query.SourceX, query.SourceY);
            if (distToSource <= walk_limit_in_km)
            {
                double walkTimeMin = (distToSource / walkingSpeedKmh) * 60.0;
                sourceSuperNode.Neighbors.Add(new Edge { To = node, LengthKm = distToSource, SpeedKmh = walkingSpeedKmh });
                sourceWalkDists[node.Id] = distToSource;
            }

            double distToDest = Euclidean_Distance_Km(node.X, node.Y, query.DestX, query.DestY);
            if (distToDest <= walk_limit_in_km)
            {
                double walkTimeMin = (distToDest / walkingSpeedKmh) * 60.0;
                node.Neighbors.Add(new Edge { To = destSuperNode, LengthKm = distToDest, SpeedKmh = walkingSpeedKmh });
                destWalkDists[node.Id] = distToDest;
            }
        }


        PathResult result = AStar(sourceSuperNode, destSuperNode);//AStar(sourceSuperNode, destSuperNode);

        result.source = sourceSuperNode;
        result.dest = destSuperNode;
        //swQuery.Stop();
        //Console.WriteLine(" time =\t " + swQuery.ElapsedMilliseconds);
        // Clean up: Remove temporary edges to destination super node
        foreach (var node in graph)
        {
            node.Neighbors.RemoveAll(edge => edge.To.Id == destSuperNode.Id);
        }

        if (result == null || result.Path.Count == 0)
        {
            // Console.WriteLine("No path found by AStar");
            return new PathResult();
        }

        // Filter out super node IDs (-1, -2) from the path
        var filteredPath = result.Path.Where(id => id >= 0).ToList();

        // Calculate walking distances
        double sourceWalkDist = 0;
        double destWalkDist = 0;

        if (filteredPath.Count > 0)
        {
            int firstNodeId = filteredPath[0];
            int lastNodeId = filteredPath[filteredPath.Count - 1];
            sourceWalkDist = sourceWalkDists.ContainsKey(firstNodeId) ? sourceWalkDists[firstNodeId] : 0;
            destWalkDist = destWalkDists.ContainsKey(lastNodeId) ? destWalkDists[lastNodeId] : 0;
        }

        // Set distances
        result.Path = filteredPath;
        result.WalkingDistanceKm = sourceWalkDist + destWalkDist;
        result.TotalDistanceKm = result.WalkingDistanceKm + result.VehicleDistanceKm;
        s.Stop();
        total_t_min += s.ElapsedMilliseconds;
        if (total_time_of_min_path_finding % 50 == 0)
            Console.WriteLine("time of index num :\t" + (total_time_of_min_path_finding + 1) + "\t equal :\t" + total_t_min);
        total_time_of_min_path_finding++;
        return result;
    }


    private PathResult AStar(Node startNode, Node endNode)
    {
        var Astar_stopWatch = Stopwatch.StartNew();

        if (startNode.Id == endNode.Id)
        {
            return new PathResult { Path = new List<int> { startNode.Id }, TotalTimeMin = 0, VehicleDistanceKm = 0 };
        }

        var queue = new PriorityQueue<(Node node, double cost), double>();

        var visited = new HashSet<int>();

        var data = new Dictionary<int, (int parent, double edgeTime, double edgeDist, double totalCost)>();

        queue.Enqueue((startNode, 0), 0);
        data[startNode.Id] = (-1, 0, 0, 0);

        while (queue.Count > 0)
        {
            var (currentNode, currentCost) = queue.Dequeue();

            if (visited.Contains(currentNode.Id))
            {
                continue;
            }

            visited.Add(currentNode.Id);

            if (currentNode.Id == endNode.Id)
            {
                var result = new PathResult();
                result.TotalTimeMin = currentCost;
                result.Path = new List<int>();
                result.VehicleDistanceKm = 0;

                int nodeId = endNode.Id;
                while (nodeId != -1 && data.ContainsKey(nodeId))
                {
                    result.Path.Add(nodeId);
                    var (parent, edgeTime, edgeDist, _) = data[nodeId];
                    // Only include vehicle edges (both graph have ID >= 0)
                    if (parent >= 0 && nodeId >= 0)
                    {
                        result.VehicleDistanceKm += edgeDist;
                    }
                    nodeId = parent;
                }
                result.Path.Reverse();

                Astar_stopWatch.Stop();
                total_time_of_Astar += Astar_stopWatch.ElapsedMilliseconds;

                return result;
            }

            foreach (var edge in currentNode.Neighbors)
            {
                if (visited.Contains(edge.To.Id))
                {
                    continue;
                }

                double newCost = currentCost + edge.TimeMin;
                double heuristic = Calculate_Node_Weight_For_AStar(edge.To, endNode);
                double priority = newCost + heuristic;

                if (!data.ContainsKey(edge.To.Id) || newCost < data[edge.To.Id].totalCost)
                {
                    data[edge.To.Id] = (currentNode.Id, edge.TimeMin, edge.LengthKm, newCost);
                    queue.Enqueue((edge.To, newCost), priority);
                }
            }
        }
        Astar_stopWatch.Stop();
        total_time_of_Astar += Astar_stopWatch.ElapsedMilliseconds;

        return null;
    }
    public double Euclidean_Distance_Km(double x1, double y1, double x2, double y2)
    {
        double x = x1 - x2;
        double y = y1 - y2;
        x *= x;
        y *= y;

        return Math.Sqrt(x + y);
    }

    public double Calculate_Node_Weight_For_AStar(Node Source, Node Dest)
    {
        double Distance = Euclidean_Distance_Km(Source.X, Source.Y, Dest.X, Dest.Y);
        return (Distance / maxSpeedKmh) * 60;
    }
    public static double ret_of_Astar_time()
    {
        return total_time_of_Astar;
    }

    public static double ret_of_min_path_time()
    {
        return total_time_of_min_path_finding;
    }
}
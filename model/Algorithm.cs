using System;
using System.Collections.Generic;
using System.Diagnostics;
using MAP_routing.model;

public class Algorithm
{
    public static double TotalTimeWithIO = 0;

    static double maxSpeedKmh;
    static List<Query> Queries;
    public List<Node> graph;

    public Algorithm(List<Node> _graph, double _maxSpeedKmh, List<Query> _queries)
    {
        if (_graph == null || _graph.Count == 0)
        {
            throw new ArgumentException("Graph cannot be null or empty.");
        }

        graph = _graph;
        maxSpeedKmh = _maxSpeedKmh;
        Queries = _queries;
    }

    public List<PathResult> ProcessQueries()
    {
        var results = new List<PathResult>();

        var swTotal = Stopwatch.StartNew();

        foreach (var query in Queries)
        {
            var result = Min_Path_Finding(query);
            results.Add(result);
        }

        swTotal.Stop();
        TotalTimeWithIO += swTotal.ElapsedMilliseconds;

        return results;
    }

    private PathResult Min_Path_Finding(Query query)
    {
        const double walkingSpeedKmh = 5.0;
        double WalkLimitKM = query.Rmeters / 1000;

        var sourceSuperNode = new Node { Id = -1, X = query.SourceX, Y = query.SourceY };
        var destSuperNode = new Node { Id = -2, X = query.DestX, Y = query.DestY };

        var sourceWalkDists = new Dictionary<int, double>();
        var sourceWalkEdges = new Dictionary<int, Edge>();
        var destWalkDists = new Dictionary<int, double>();
        var destWalkEdges = new Dictionary<int, Edge>();

        List<(int, int)> CandidateIndex = new List<(int, int)>();

        // Define bounding boxes for source and destination
        double sourceXMin = query.SourceX - WalkLimitKM;
        double sourceXMax = query.SourceX + WalkLimitKM;
        double sourceYMin = query.SourceY - WalkLimitKM;
        double sourceYMax = query.SourceY + WalkLimitKM;

        double destXMin = query.DestX - WalkLimitKM;
        double destXMax = query.DestX + WalkLimitKM;
        double destYMin = query.DestY - WalkLimitKM;
        double destYMax = query.DestY + WalkLimitKM;

        foreach (var node in graph)
        {
            // Check if node is within source bounding box
            if (node.X >= sourceXMin && node.X <= sourceXMax &&
                node.Y >= sourceYMin && node.Y <= sourceYMax)
            {
                double distToSource = Euclidean_Distance_Km(node.X, node.Y, query.SourceX, query.SourceY);
                if (distToSource <= WalkLimitKM)
                {
                    double walkTimeMin = (distToSource / walkingSpeedKmh) * 60.0;
                    var edge = new Edge { To = node, LengthKm = distToSource, SpeedKmh = walkingSpeedKmh, Color = Color.Green };
                    sourceSuperNode.Neighbors.Add(edge);
                    sourceWalkDists[node.Id] = distToSource;
                    sourceWalkEdges[node.Id] = edge;

                    CandidateIndex.Add((node.Id, 1));
                }
            }

            // Check if node is within destination bounding box
            if (node.X >= destXMin && node.X <= destXMax &&
                node.Y >= destYMin && node.Y <= destYMax)
            {
                double distToDest = Euclidean_Distance_Km(node.X, node.Y, query.DestX, query.DestY);
                if (distToDest <= WalkLimitKM)
                {
                    double walkTimeMin = (distToDest / walkingSpeedKmh) * 60.0;
                    var edge = new Edge { To = destSuperNode, LengthKm = distToDest, SpeedKmh = walkingSpeedKmh, Color = Color.Green };
                    node.Neighbors.Add(edge);
                    destWalkDists[node.Id] = distToDest;
                    destWalkEdges[node.Id] = edge;
                    CandidateIndex.Add((node.Id, 0));
                }
            }
        }

        PathResult result = AStar(sourceSuperNode, destSuperNode);//AStar(sourceSuperNode, destSuperNode);

        result.source = sourceSuperNode;
        result.dest = destSuperNode;

        //swQuery.Stop();
        //Console.WriteLine(" time =\t " + swQuery.ElapsedMilliseconds);
        // Clean up: Remove temporary edges to destination super node
        foreach ((int id, int type) in CandidateIndex)
        {
            if (type == 1)
            {
                graph[id].Neighbors.Remove(sourceWalkEdges[id]);
            }
            if (type == 0)
            {
                graph[id].Neighbors.Remove(destWalkEdges[id]);
            }
        }

        if (result == null || result.Path.Count == 0)
        {
            // Console.WriteLine("No path found by AStar");
            return new PathResult();
        }

        //foreach (var edge in result.Edges)
        //{
        //    edge.IsPath = true;
        //    edge.Color = Color.Red;
        //}

        // Filter out super node IDs (-1, -2) from the path
        //var filteredPath = result.Path.Where(id => id >= 0).ToList();

        result.Path.RemoveAt(result.Path.Count - 1);

        // Calculate walking distances
        double sourceWalkDist = 0;
        double destWalkDist = 0;

        if (result.Path.Count > 0)
        {
            int firstNodeId = result.Path[0];
            int lastNodeId = result.Path[result.Path.Count - 1];
            sourceWalkDist = sourceWalkDists[firstNodeId];
            destWalkDist = destWalkDists[lastNodeId];
        }

        result.WalkingDistanceKm = sourceWalkDist + destWalkDist;
        result.TotalDistanceKm = result.WalkingDistanceKm + result.VehicleDistanceKm;
        return result;
    }

    private PathResult AStar(Node startNode, Node endNode)
    {
        if (startNode.Id == endNode.Id)
        {
            return new PathResult { Path = new List<int> { startNode.Id }, TotalTimeMin = 0, VehicleDistanceKm = 0 };
        }

        var queue = new PriorityQueue<(Node node, double cost), double>();
        var visited = new HashSet<int>();
        var data = new Dictionary<int, (int parent, Edge usedEdge, double totalCost, double vehicleDistance)>();

        queue.Enqueue((startNode, 0), 0);
        data[startNode.Id] = (-1, null, 0, 0);

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
                result.Edges = new List<Edge>();
                result.VehicleDistanceKm = data[currentNode.Id].vehicleDistance;

                int nodeId = endNode.Id;
                while (nodeId != -1 && data.ContainsKey(nodeId))
                {
                    result.Path.Add(nodeId);
                    var (parent, usedEdge, _, _) = data[nodeId];

                    if (usedEdge != null)
                    {
                        result.Edges.Add(usedEdge);
                    }

                    nodeId = parent;
                }
                result.Path.Reverse();
                result.Edges.Reverse();

                return result;
            }

            foreach (var edge in currentNode.Neighbors)
            {
                if (visited.Contains(edge.To.Id))
                {
                    continue;
                }

                double newCost = currentCost + edge.TimeMin;
                double newVehicleDistance = data[currentNode.Id].vehicleDistance;
                if (currentNode.Id >= 0 && edge.To.Id >= 0)
                {
                    newVehicleDistance += edge.LengthKm;
                }

                double heuristic = Calculate_Node_Weight_For_AStar(edge.To, endNode);
                double priority = newCost + heuristic;

                if (!data.ContainsKey(edge.To.Id) || newCost < data[edge.To.Id].totalCost)
                {
                    data[edge.To.Id] = (currentNode.Id, edge, newCost, newVehicleDistance);
                    queue.Enqueue((edge.To, newCost), priority);
                }
            }
        }

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
}
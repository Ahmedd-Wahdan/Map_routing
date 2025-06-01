using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using MAP_routing.model;

public class Algorithm
{
    public static double TotalTimeWithout_IO = 0;
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

    public List<PathResult> ProcessQueriesSequntial()
    {
        var results = new List<PathResult>();
        var swTotal = Stopwatch.StartNew();

        foreach (var query in Queries)
        {
            var result = Min_Path_Finding(query);
            results.Add(result);
        }

        swTotal.Stop();
        TotalTimeWithout_IO = swTotal.ElapsedMilliseconds;
        return results;
    }

    public List<PathResult> ProcessQueries()
    {
        if (Queries.Count <= 10)
        {
            return ProcessQueriesSequntial();
        }

        var results = new PathResult[Queries.Count];
        int degreeOfParallelism = Math.Min(Environment.ProcessorCount, Queries.Count);
        var options = new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism };
        var swTotal = Stopwatch.StartNew();

        Parallel.For(0, Queries.Count, options, index =>
        {
            var result = Min_Path_Finding(Queries[index]);
            results[index] = result;
        });

        swTotal.Stop();
        TotalTimeWithout_IO = swTotal.ElapsedMilliseconds;
        MapRouting.ResetMode();

        return results.ToList();
    }

    private PathResult Multible_speed_Algorithm(Node __start_super_node__, Node __end_super_node__, Dictionary<int, Edge> __tempo_edges__, Query query)
    {
        if (__start_super_node__.Id == __end_super_node__.Id)
        {
            return new PathResult
            {
                Path = new List<int> { __start_super_node__.Id },
                TotalTimeMin = 0,
                VehicleDistanceKm = 0,
                WalkingDistanceKm = 0,
                TotalDistanceKm = 0,
                Edges = new List<Edge>(),
                source = __start_super_node__,
                dest = __end_super_node__
            };
        }

        var nodeLookup = new Dictionary<int, Node>();
        foreach (var node in graph)
        {
            nodeLookup[node.Id] = node;
        }
        nodeLookup[__start_super_node__.Id] = __start_super_node__;
        nodeLookup[__end_super_node__.Id] = __end_super_node__;

        var priorityQueue = new Priority_Queue<(int nodeId, double timeAtNode), double>();
        var bestTimes = new Dictionary<int, double>();
        var bestDistances = new Dictionary<int, double>();
        var bestDestDistances = new Dictionary<int, double>(); 
        var parentInfo = new Dictionary<int, (int parentId, Edge edge, double timeAtParent)>();

        double startDestDist = Math.Sqrt(Math.Pow(__start_super_node__.X - query.DestX, 2) + Math.Pow(__start_super_node__.Y - query.DestY, 2));
        priorityQueue.Enqueue((__start_super_node__.Id, 0), 0);
        bestTimes[__start_super_node__.Id] = 0;
        bestDistances[__start_super_node__.Id] = 0;
        bestDestDistances[__start_super_node__.Id] = startDestDist;
        parentInfo[__start_super_node__.Id] = (-1, null, 0);

        Stopwatch timer = Stopwatch.StartNew();

        double bestTimeToDestination = double.MaxValue;
        int? bestDestinationNode = null;

        while (priorityQueue.Elements.Count > 0 )
        {
            var (currentNodeId, currentTime) = priorityQueue.Dequeue();

            if (bestTimes.TryGetValue(currentNodeId, out double storedBestTime) &&
                currentTime > storedBestTime )
                continue;

            if (currentNodeId == __end_super_node__.Id)
            {
                if (currentTime < bestTimeToDestination)
                {
                    bestTimeToDestination = currentTime;
                    bestDestinationNode = currentNodeId;
                }
                continue;
            }

            Node currentNode = nodeLookup[currentNodeId];

            foreach (var edge in currentNode.Neighbors)
            {
               
                double edgeTime = edge.GetTimeMin(currentTime);

                double newTotalTime = currentTime + edgeTime;
                double newDistance = bestDistances[currentNodeId] + edge.LengthKm;
                double destDist = Math.Sqrt(Math.Pow(edge.To.X - query.DestX, 2) + Math.Pow(edge.To.Y - query.DestY, 2));

                bool updatePath = false;
                if (!bestTimes.TryGetValue(edge.To.Id, out double existingBestTime))
                {
                    updatePath = true;
                }
                else if (newTotalTime < existingBestTime )
                {
                    updatePath = true;
                }

                else if (Math.Abs(newTotalTime - existingBestTime) ==0)
                {
                    double existingDistance = bestDistances.GetValueOrDefault(edge.To.Id, double.MaxValue);
                    double existingDestDist = bestDestDistances.GetValueOrDefault(edge.To.Id, double.MaxValue);
                    if (newDistance < existingDistance ||
                        (Math.Abs(newDistance - existingDistance) ==0 && destDist < existingDestDist ))
                    {
                        updatePath = true;
                    }
                }

                if (updatePath)
                {
                    bestTimes[edge.To.Id] = newTotalTime;
                    bestDistances[edge.To.Id] = newDistance;
                    bestDestDistances[edge.To.Id] = destDist;
                    parentInfo[edge.To.Id] = (currentNodeId, edge, currentTime);
                    priorityQueue.Enqueue((edge.To.Id, newTotalTime), newTotalTime);
                }

                
            }

            if (__tempo_edges__.TryGetValue(currentNodeId, out Edge tempEdge))
            {
                if (tempEdge.LengthKm <= 0 || (tempEdge.SpeedKmhList != null && tempEdge.SpeedKmhList.Any(s => s <= 0) && tempEdge.SpeedKmh <= 0))
                {
                    continue;
                }

                double edgeTime = tempEdge.GetTimeMin(currentTime);
                if (double.IsNaN(edgeTime) || double.IsInfinity(edgeTime) || edgeTime <= 0)
                {
                    continue;
                }

                double newTotalTime = currentTime + edgeTime;
                double newDistance = bestDistances[currentNodeId] + tempEdge.LengthKm;
                double destDist = Math.Sqrt(Math.Pow(tempEdge.To.X - query.DestX, 2) + Math.Pow(tempEdge.To.Y - query.DestY, 2));

                bool updatePath = false;
                if (!bestTimes.TryGetValue(tempEdge.To.Id, out double existingBestTime))
                {
                    updatePath = true;
                }
                else if (newTotalTime < existingBestTime)
                {
                    updatePath = true;
                }
                else if (newTotalTime ==existingBestTime)
                {
                    double existingDistance = bestDistances.GetValueOrDefault(tempEdge.To.Id, double.MaxValue);
                    double existingDestDist = bestDestDistances.GetValueOrDefault(tempEdge.To.Id, double.MaxValue);
                    if (newDistance < existingDistance ||
                        (newDistance == existingDistance && destDist < existingDestDist))
                    {
                        updatePath = true;
                    }
                }

                if (updatePath)
                {
                    bestTimes[tempEdge.To.Id] = newTotalTime;
                    bestDistances[tempEdge.To.Id] = newDistance;
                    bestDestDistances[tempEdge.To.Id] = destDist;
                    parentInfo[tempEdge.To.Id] = (currentNodeId, tempEdge, currentTime);
                    priorityQueue.Enqueue((tempEdge.To.Id, newTotalTime), newTotalTime);
                }
            }
        }

        if (bestDestinationNode == null)
        {
            return null;
        }


        var result = new PathResult
        {
            Path = new List<int>(),
            Edges = new List<Edge>(),
            TotalTimeMin = 0,
            VehicleDistanceKm = 0,
            WalkingDistanceKm = 0,
            source = __start_super_node__,
            dest = __end_super_node__
        };

        var edgeLog = new List<string>();
        int nodeId = bestDestinationNode.Value;
        double accumulatedTime = 0;
        double accumulatedVehicleDistance = 0;

        while (nodeId != __start_super_node__.Id)
        {
            if (!parentInfo.TryGetValue(nodeId, out var parentData))
            {
                return null;
            }

            var (parentId, edge, parentTime) = parentData;

            if (nodeId >= 0)
                result.Path.Insert(0, nodeId);

            if (edge != null)
            {
                double edgeTime = edge.GetTimeMin(parentTime);
                accumulatedTime += edgeTime;
                result.Edges.Insert(0, edge);

                if (edge.Color == Color.Green)
                    result.WalkingDistanceKm += edge.LengthKm;
                else if (parentId >= 0 && nodeId >= 0)
                {
                    result.VehicleDistanceKm += edge.LengthKm;
                    accumulatedVehicleDistance += edge.LengthKm;
                }

                int intervalIndex = (int)Math.Floor(parentTime / MapRouting.SpeedIntervalMinutes) % edge.SpeedKmhList.Count;
                double speed = intervalIndex >= 0 ? edge.SpeedKmhList[intervalIndex] : edge.SpeedKmh;
                double destDist = Math.Sqrt(Math.Pow(edge.To.X - query.DestX, 2) + Math.Pow(edge.To.Y - query.DestY, 2));
            }
            nodeId = parentId;
        }

        const double walkingSpeedKmh = 5.0;
        double WalkLimitKM = query.Rmeters / 1000;
        double WalkLimitKMSquared = WalkLimitKM * WalkLimitKM;

        var sourceWalkDists = new Dictionary<int, double>();
        var destWalkDists = new Dictionary<int, double>();

        foreach (var node in graph)
        {
            double sx = node.X - query.SourceX;
            double sy = node.Y - query.SourceY;
            double distSquared = sx * sx + sy * sy;
            if (distSquared <= WalkLimitKMSquared)
            {
                double distToSource = Math.Sqrt(distSquared);
                sourceWalkDists[node.Id] = distToSource;
            }

            double dx = node.X - query.DestX;
            double dy = node.Y - query.DestY;
            distSquared = dx * dx + dy * dy;
            if (distSquared <= WalkLimitKMSquared)
            {
                double distToDest = Math.Sqrt(distSquared);
                destWalkDists[node.Id] = distToDest;
            }
        }

        if (result.Path.Count > 0)
        {
            int firstNodeId = result.Path[0];
            int lastNodeId = result.Path[result.Path.Count - 1];

            if (sourceWalkDists.ContainsKey(firstNodeId))
            {
                double dist = sourceWalkDists[firstNodeId];
                bool alreadyCounted = result.Edges.Any(e => e.From.Id == __start_super_node__.Id && e.To.Id == firstNodeId);
                if (!alreadyCounted)
                {
                    result.WalkingDistanceKm += dist;
                    double walkingTime = (dist / walkingSpeedKmh) * 60;
                    accumulatedTime += walkingTime;
                    var edge = new Edge
                    {
                        From = __start_super_node__,
                        To = nodeLookup[firstNodeId],
                        LengthKm = dist,
                        SpeedKmh = walkingSpeedKmh,
                        SpeedKmhList = new List<double> { walkingSpeedKmh },
                        Color = Color.Green
                    };
                    result.Edges.Insert(0, edge);
                    edgeLog.Insert(0, $"Walking Edge {__start_super_node__.Id} -> {firstNodeId}: Time={walkingTime:F6} min, Length={dist:F6} km, Color=Green, Speed={walkingSpeedKmh:F6} km/h");
                }
            }

            if (destWalkDists.ContainsKey(lastNodeId))
            {
                double dist = destWalkDists[lastNodeId];
                bool alreadyCounted = result.Edges.Any(e => e.From.Id == lastNodeId && e.To.Id == __end_super_node__.Id);
                if (!alreadyCounted)
                {
                    result.WalkingDistanceKm += dist;
                    double walkingTime = (dist / walkingSpeedKmh) * 60;
                    accumulatedTime += walkingTime;
                    var edge = new Edge
                    {
                        From = nodeLookup[lastNodeId],
                        To = __end_super_node__,
                        LengthKm = dist,
                        SpeedKmh = walkingSpeedKmh,
                        SpeedKmhList = new List<double> { walkingSpeedKmh },
                        Color = Color.Green
                    };
                    result.Edges.Add(edge);
                    edgeLog.Add($"Walking Edge {lastNodeId} -> {__end_super_node__.Id}: Time={walkingTime:F6} min, Length={dist:F6} km, Color=Green, Speed={walkingSpeedKmh:F6} km/h");
                }
            }
        }

        result.TotalTimeMin = accumulatedTime;
        result.TotalDistanceKm = result.WalkingDistanceKm + result.VehicleDistanceKm;

        

        return result;
    }
   
    private PathResult Min_Path_Finding(Query query)
    {
        const double walkingSpeedKmh = 5.0;
        double WalkLimitKM = query.Rmeters / 1000;
        double WalkLimitKMSquared = WalkLimitKM * WalkLimitKM;

        var sourceSuperNode = new Node { Id = -1, X = query.SourceX, Y = query.SourceY, Neighbors = new List<Edge>() };
        var destSuperNode = new Node { Id = -2, X = query.DestX, Y = query.DestY, Neighbors = new List<Edge>() };

        var sourceWalkDists = new Dictionary<int, double>();
        var destWalkDists = new Dictionary<int, double>();
        var tempEdges = new Dictionary<int, Edge>();

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
            if (node.X >= sourceXMin && node.X <= sourceXMax &&
                node.Y >= sourceYMin && node.Y <= sourceYMax)
            {
                double sx = node.X - query.SourceX;
                double sy = node.Y - query.SourceY;
                double distSquared = sx * sx + sy * sy;

                if (distSquared <= WalkLimitKMSquared)
                {
                    double distToSource = Math.Sqrt(distSquared);
                    var edge = new Edge
                    {
                        From = sourceSuperNode,
                        To = node,
                        LengthKm = distToSource,
                        SpeedKmh = walkingSpeedKmh,
                        SpeedKmhList = new List<double> { walkingSpeedKmh },
                        Color = Color.Green
                    };
                    sourceSuperNode.Neighbors.Add(edge);
                    sourceWalkDists[node.Id] = distToSource;
                }
            }

            if (node.X >= destXMin && node.X <= destXMax &&
                node.Y >= destYMin && node.Y <= destYMax)
            {
                double dx = node.X - query.DestX;
                double dy = node.Y - query.DestY;
                double distSquared = dx * dx + dy * dy;

                if (distSquared <= WalkLimitKMSquared)
                {
                    double distToDest = Math.Sqrt(distSquared);
                    var edge = new Edge
                    {
                        From = node,
                        To = destSuperNode,
                        LengthKm = distToDest,
                        SpeedKmh = walkingSpeedKmh,
                        SpeedKmhList = new List<double> { walkingSpeedKmh },
                        Color = Color.Green
                    };
                    tempEdges[node.Id] = edge;
                    destWalkDists[node.Id] = distToDest;
                }
            }
        }

        PathResult result = MapRouting.IsVariableSpeedMode
            ? Multible_speed_Algorithm(sourceSuperNode, destSuperNode, tempEdges, query)
            : __AStar_Algorithm__(sourceSuperNode, destSuperNode, tempEdges);

        if (result != null)
        {
            result.source = sourceSuperNode;
            result.dest = destSuperNode;
        }

        if (result == null || result.Path == null || result.Path.Count == 0)
        {
            return new PathResult();
        }

        if (result.Path.Count > 0 && result.Path[result.Path.Count - 1] == destSuperNode.Id)
        {
            result.Path.RemoveAt(result.Path.Count - 1);
        }

        double sourceWalkDist = 0;
        double destWalkDist = 0;

        if (result.Path.Count > 0)
        {
            int firstNodeId = result.Path[0];
            int lastNodeId = result.Path[result.Path.Count - 1];

            if (sourceWalkDists.ContainsKey(firstNodeId))
                sourceWalkDist = sourceWalkDists[firstNodeId];

            if (destWalkDists.ContainsKey(lastNodeId))
                destWalkDist = destWalkDists[lastNodeId];
        }

        result.WalkingDistanceKm = sourceWalkDist + destWalkDist;
        result.TotalDistanceKm = result.WalkingDistanceKm + result.VehicleDistanceKm;
        return result;
    }

    private PathResult __AStar_Algorithm__(Node startNode, Node endNode, Dictionary<int, Edge> tempEdges)
    {
        if (startNode.Id == endNode.Id)
        {
            return new PathResult
            {
                Path = new List<int> { startNode.Id },
                TotalTimeMin = 0,
                VehicleDistanceKm = 0,
                WalkingDistanceKm = 0,
                TotalDistanceKm = 0,
                Edges = new List<Edge>()
            };
        }

        var queue = new Priority_Queue<(Node node, double cost), double>();
        var visited = new HashSet<int>();
        var data = new Dictionary<int, (int parent, Edge usedEdge, double totalCost, double vehicleDistance)>();

        queue.Enqueue((startNode, 0), 0);
        data[startNode.Id] = (-1, null, 0, 0);

        while (queue.Elements.Count > 0)
        {
            var (currentNode, currentCost) = queue.Dequeue();

            if (visited.Contains(currentNode.Id))
                continue;

            visited.Add(currentNode.Id);

            if (currentNode.Id == endNode.Id)
            {
                var result = new PathResult
                {
                    TotalTimeMin = currentCost,
                    Path = new List<int>(),
                    Edges = new List<Edge>(),
                    VehicleDistanceKm = data[currentNode.Id].vehicleDistance,
                    WalkingDistanceKm = 0,
                    TotalDistanceKm = data[currentNode.Id].vehicleDistance
                };

                int nodeId = endNode.Id;
                while (nodeId != -1 && data.ContainsKey(nodeId))
                {
                    result.Path.Add(nodeId);
                    var (parent, usedEdge, _, _) = data[nodeId];
                    if (usedEdge != null)
                        result.Edges.Add(usedEdge);
                    nodeId = parent;
                }

                result.Path.Reverse();
                result.Edges.Reverse();
                return result;
            }

            foreach (var edge in currentNode.Neighbors)
            {
                if (visited.Contains(edge.To.Id))
                    continue;

                double newCost = currentCost + edge.TimeMin;
                double newVehicleDistance = data[currentNode.Id].vehicleDistance;

                if (currentNode.Id >= 0 && edge.To.Id >= 0)
                    newVehicleDistance += edge.LengthKm;

                double heuristic = Calculate_Node_Weight_For_AStar(edge.To, endNode);
                double priority = newCost + heuristic;

                if (!data.ContainsKey(edge.To.Id) || newCost < data[edge.To.Id].totalCost)
                {
                    data[edge.To.Id] = (currentNode.Id, edge, newCost, newVehicleDistance);
                    queue.Enqueue((edge.To, newCost), priority);
                }
            }

            if (tempEdges.ContainsKey(currentNode.Id))
            {
                var edge = tempEdges[currentNode.Id];
                if (!visited.Contains(edge.To.Id))
                {
                    double newCost = currentCost + edge.TimeMin;
                    double newVehicleDistance = data[currentNode.Id].vehicleDistance;

                    double heuristic = Calculate_Node_Weight_For_AStar(edge.To, endNode);
                    double priority = newCost + heuristic;

                    if (!data.ContainsKey(edge.To.Id) || newCost < data[edge.To.Id].totalCost)
                    {
                        data[edge.To.Id] = (currentNode.Id, edge, newCost, newVehicleDistance);
                        queue.Enqueue((edge.To, newCost), priority);
                    }
                }
            }
        }

        return null;
    }

    private double Euclidean_Distance_Km(double x1, double y1, double x2, double y2)
    {
        double x = x1 - x2;
        double y = y1 - y2;
        x *= x;
        y *= y;
        return Math.Sqrt(x + y);
    }

    private double Calculate_Node_Weight_For_AStar(Node Source, Node Dest)
    {
        double Distance = Euclidean_Distance_Km(Source.X, Source.Y, Dest.X, Dest.Y);
        return (Distance / maxSpeedKmh) * 60;
    }
}
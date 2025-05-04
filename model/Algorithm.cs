using System;
using System.Collections.Generic;
using System.Linq;

namespace MAP_routing.model
{
    public class Algorithm
    {
        public class PathResult
        {
            public List<int> Path = new List<int>();
            public double TotalTimeMin;
            public double TotalDistanceKm;
            public double WalkingDistanceKm;
            public double VehicleDistanceKm;
        }

        public Graph graph;
        private double maxSpeedKmh;
        const double speed_of_walk = 5.0;

        public Algorithm(Graph graph)
        {
            this.graph = graph;
            maxSpeedKmh = graph.Edges.Any() ? graph.Edges.Max(e => e.Speed) : 100.0;
        }

        public PathResult Min_Path_Finding(Query query)
        {
            var source_feasible_nodes = new List<(Node node, double walkDistKm, double walkTimeMin)>();
            var dest_feasible_nodes = new List<(Node node, double walkDistKm, double walkTimeMin)>();
            PathResult optimal_route_result = new PathResult();
            double walk_limit_in_km = query.Rmeteres / 1000.0;
            const double walkingSpeedKmh = 5.0;

            foreach (var temp_node in graph.Nodes.Values)
            {
                double dist_between_candidate_node_and_source_KM = Euclidean_Distance_Km(temp_node.X, temp_node.Y, query.StartX, query.StartY);
                double dist_between_candidate_node_and_dest_KM = Euclidean_Distance_Km(temp_node.X, temp_node.Y, query.EndX, query.EndY);
                double source_walk_T = (dist_between_candidate_node_and_source_KM / walkingSpeedKmh) * 60.0;
                double dest_walk_T = (dist_between_candidate_node_and_dest_KM / walkingSpeedKmh) * 60.0;

                if (dist_between_candidate_node_and_source_KM <= walk_limit_in_km)
                {
                    source_feasible_nodes.Add((temp_node, dist_between_candidate_node_and_source_KM, source_walk_T));
                }

                if (dist_between_candidate_node_and_dest_KM <= walk_limit_in_km)
                {
                    dest_feasible_nodes.Add((temp_node, dist_between_candidate_node_and_dest_KM, dest_walk_T));
                }
            }

            double min_time = double.PositiveInfinity;
            for (int i = 0; i < source_feasible_nodes.Count; i++)
            {
                var source_temp_node = source_feasible_nodes[i];
                for (int j = 0; j < dest_feasible_nodes.Count; j++)
                {
                    var dest_temp_node = dest_feasible_nodes[j];
                    PathResult result_between_src_and_des = Two_way_AStar(source_temp_node.node, dest_temp_node.node);
                    if (result_between_src_and_des == null)
                    {
                        continue;
                    }

                    result_between_src_and_des.TotalTimeMin += source_temp_node.walkTimeMin + dest_temp_node.walkTimeMin;
                    result_between_src_and_des.WalkingDistanceKm = (source_temp_node.walkDistKm + dest_temp_node.walkDistKm);
                    result_between_src_and_des.TotalDistanceKm = (result_between_src_and_des.WalkingDistanceKm + result_between_src_and_des.VehicleDistanceKm);

                    if (min_time > result_between_src_and_des.TotalTimeMin)
                    {
                        min_time = result_between_src_and_des.TotalTimeMin;
                        optimal_route_result = new PathResult
                        {
                            Path = result_between_src_and_des.Path,
                            TotalTimeMin = result_between_src_and_des.TotalTimeMin,
                            WalkingDistanceKm = result_between_src_and_des.WalkingDistanceKm,
                            VehicleDistanceKm = result_between_src_and_des.VehicleDistanceKm,
                            TotalDistanceKm = result_between_src_and_des.TotalDistanceKm
                        };
                    }
                }
            }
            return optimal_route_result;
        }

        public PathResult Two_way_AStar(Node Starting_Node, Node Ending_Node)
        {
            if (Starting_Node.Id == Ending_Node.Id)
            {
                return new PathResult { Path = new List<int> { Starting_Node.Id }, TotalTimeMin = 0, VehicleDistanceKm = 0 };
            }

            PathResult ret = new PathResult();

            var ForwardQueue = new Priority_Queue<(Node Current_Point, double Edge_Weight), double>();
            var BackwardQueue = new Priority_Queue<(Node Current_Point, double Edge_Weight), double>();

            var ForwardVisited = new HashSet<int>();
            var BackwardVisited = new HashSet<int>();

            var ForwardNodesData = new Dictionary<int, (int Parent, double EdgeTime, double EdgeDist, double EdgeWeight)>();
            var BackwardNodesData = new Dictionary<int, (int parent, double edgeTime, double edgeDist, double EdgeWeight)>();

            var OptimalTime = double.MaxValue;
            var MeetingNodeID = -1;

            var ForwardWeight = Calculate_Node_Weight_For_AStar(Starting_Node, Ending_Node);
            var BackwardWeight = Calculate_Node_Weight_For_AStar(Ending_Node, Starting_Node);

            ForwardQueue.Enqueue((Starting_Node, 0), ForwardWeight);
            BackwardQueue.Enqueue((Ending_Node, 0), BackwardWeight);

            ForwardNodesData[Starting_Node.Id] = (-1, double.MaxValue, double.MaxValue, 0);
            BackwardNodesData[Ending_Node.Id] = (-1, double.MaxValue, double.MaxValue, 0);

            while (ForwardQueue.Yaya_Elements.Count > 0 && BackwardQueue.Yaya_Elements.Count > 0)
            {
                var (ForwardCurrentNode, ForwardEdgeWeight) = ForwardQueue.Dequeue();

                if (!ForwardVisited.Contains(ForwardCurrentNode.Id))
                {
                    ForwardVisited.Add(ForwardCurrentNode.Id);
                    if (BackwardVisited.Contains(ForwardCurrentNode.Id))
                    {
                        double totalTime = ForwardEdgeWeight + BackwardNodesData[ForwardCurrentNode.Id].EdgeWeight;
                        if (totalTime < OptimalTime)
                        {
                            OptimalTime = totalTime;
                            MeetingNodeID = ForwardCurrentNode.Id;
                        }
                    }

                    // Find outgoing edges
                    var outgoingEdges = graph.Edges.Where(e => e.FromId == ForwardCurrentNode.Id);
                    foreach (var edge in outgoingEdges)
                    {
                        int neighborId = edge.ToId;
                        if (ForwardVisited.Contains(neighborId)) { continue; }

                        var UpdatedEdgeWeight = ForwardEdgeWeight + edge.TimeMin;

                        if (!ForwardNodesData.ContainsKey(neighborId) || UpdatedEdgeWeight < ForwardNodesData[neighborId].EdgeWeight)
                        {
                            ForwardNodesData[neighborId] = (ForwardCurrentNode.Id, edge.TimeMin, edge.Length, UpdatedEdgeWeight);
                            var time = Calculate_Node_Weight_For_AStar(graph.Nodes[neighborId], Ending_Node);
                            ForwardQueue.Enqueue((graph.Nodes[neighborId], UpdatedEdgeWeight), time + UpdatedEdgeWeight);
                        }
                    }
                }

                var (BackwardCurrentNode, BackwardEdgeWeight) = BackwardQueue.Dequeue();
                if (!BackwardVisited.Contains(BackwardCurrentNode.Id))
                {
                    BackwardVisited.Add(BackwardCurrentNode.Id);

                    if (ForwardVisited.Contains(BackwardCurrentNode.Id))
                    {
                        double totalTime = BackwardEdgeWeight + ForwardNodesData[BackwardCurrentNode.Id].EdgeWeight;
                        if (totalTime < OptimalTime)
                        {
                            OptimalTime = totalTime;
                            MeetingNodeID = BackwardCurrentNode.Id;
                        }
                    }

                    // Find incoming edges (reverse direction)
                    var incomingEdges = graph.Edges.Where(e => e.ToId == BackwardCurrentNode.Id);
                    foreach (var edge in incomingEdges)
                    {
                        int neighborId = edge.FromId;
                        if (BackwardVisited.Contains(neighborId)) { continue; }

                        var UpdatedEdgeWeight = BackwardEdgeWeight + edge.TimeMin;

                        if (!BackwardNodesData.ContainsKey(neighborId) || UpdatedEdgeWeight < BackwardNodesData[neighborId].EdgeWeight)
                        {
                            BackwardNodesData[neighborId] = (BackwardCurrentNode.Id, edge.TimeMin, edge.Length, UpdatedEdgeWeight);
                            var time = Calculate_Node_Weight_For_AStar(graph.Nodes[neighborId], Starting_Node);
                            BackwardQueue.Enqueue((graph.Nodes[neighborId], UpdatedEdgeWeight), time + UpdatedEdgeWeight);
                        }
                    }
                }

                double ForwardMinNode, BackwardMinNode;
                if (ForwardQueue.Yaya_Elements.Count > 0)
                {
                    ForwardMinNode = ForwardQueue.Yaya_Elements[0].Item2; // Access priority (weight)
                }
                else
                {
                    ForwardMinNode = double.MaxValue;
                }

                if (BackwardQueue.Yaya_Elements.Count > 0)
                {
                    BackwardMinNode = BackwardQueue.Yaya_Elements[0].Item2; // Access priority (weight)
                }
                else
                {
                    BackwardMinNode = double.MaxValue;
                }

                if (ForwardMinNode >= OptimalTime && BackwardMinNode >= OptimalTime && MeetingNodeID != -1)
                    break;
            }

            if (MeetingNodeID == -1) { return null; }

            ret.Path = new List<int>();
            ret.VehicleDistanceKm = 0;
            ret.TotalTimeMin = 0;

            var CurrentNode = MeetingNodeID;
            var ForwardPath = new List<int>();
            while (CurrentNode != Starting_Node.Id)
            {
                ForwardPath.Add(CurrentNode);
                if (!ForwardNodesData.ContainsKey(CurrentNode)) break;
                var (ParentNode, EdgeTime, EdgeDistance, EdgeWeight) = ForwardNodesData[CurrentNode];
                ret.VehicleDistanceKm += EdgeDistance;
                ret.TotalTimeMin += EdgeTime;
                CurrentNode = ParentNode;
            }
            ForwardPath.Add(Starting_Node.Id);
            ForwardPath.Reverse();

            var BackwardPath = new List<int>();
            CurrentNode = MeetingNodeID;
            while (CurrentNode != Ending_Node.Id)
            {
                BackwardPath.Add(CurrentNode);
                if (BackwardNodesData.ContainsKey(CurrentNode))
                {
                    var (ParentNode, EdgeTime, EdgeDistance, EdgeWeight) = BackwardNodesData[CurrentNode];
                    ret.VehicleDistanceKm += EdgeDistance;
                    ret.TotalTimeMin += EdgeTime;
                    CurrentNode = ParentNode;
                }
                else { break; }
            }
            if (CurrentNode == Ending_Node.Id)
            {
                BackwardPath.Add(Ending_Node.Id);
            }

            ret.Path.AddRange(ForwardPath);
            if (BackwardPath.Count >= 1)
            {
                ret.Path.AddRange(BackwardPath.Skip(1));
            }
            else if (BackwardPath.Count == 1 && BackwardPath[0] == Ending_Node.Id)
            {
                ret.Path.Add(Ending_Node.Id);
            }

            return ret;
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
}
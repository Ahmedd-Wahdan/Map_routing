using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP_routing.model
{
    public class Algorithm
    {
        public class Node
        {
            public int Id { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public List<Edge> Neighbors { get; set; } = new List<Edge>();
        }
        public class Edge
        {
            public Node To { get; set; }
            public double LengthKm { get; set; }
            public double SpeedKmh { get; set; }
            public double TimeMin => (LengthKm / SpeedKmh) * 60.0;
        }

        public class output_result
        {
            public List<int> Path = new List<int>();
            public double total_t_in_minute;
            public double total_dis_in_km;
            public double Walk_dis_km;
            public double vehicle_dis_in_km;
        }

        public List<Node> Example_of_graph;

        private double maxSpeedKmh;

        const double speed_of_walk = 5.0;
        public double Euclidean_Distance_Km(double SrcX, double SrcY, double DestX, double DestY)
        {
            double x = DestX - SrcX;
            double y = DestY - SrcY;
            x *= x;
            y *= y;

            return Math.Sqrt(x + y) / 1000;
        }

        public double Calculate_Node_Weight_For_AStar(double SrcX, double SrcY, double DestX, double DestY)
        {
            double Distance = Euclidean_Distance_Km(SrcX, SrcY, DestX, DestY);
            return (Distance / maxSpeedKmh) * 60;
        }

        private output_result Min_Path_Finding(double SourceX, double SourceY, double DestX, double DestY, double Rmeters)
        {
            var source_feasible_nodes = new List<(Node node, double walkDistKm, double walkTimeMin)>();
            var dest_feasible_nodes = new List<(Node node, double walkDistKm, double walkTimeMin)>();


            output_result optimal_route_result = new output_result();

            double walk_limit_in_km = Rmeters / 1000;

            for (int i = 0; i < Example_of_graph.Count; i++)
            {
                var temp_node = Example_of_graph[i];

                double dist_between_candidate_node_and_source_KM = Euclidean_Distance_Km(temp_node.X, temp_node.Y, SourceX, SourceY);
                double dist_between_candidate_node_and_dest_KM = Euclidean_Distance_Km(temp_node.X, temp_node.Y, DestX, DestY);


                double factor = 60 / speed_of_walk;
                double source_walk_T = dist_between_candidate_node_and_source_KM * factor;
                double dest_walk_T = dist_between_candidate_node_and_dest_KM * factor;

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

                    output_result result_between_src_and_des = Two_way_AStar();
                    if (result_between_src_and_des == null)
                    {
                        continue;
                    }

                    result_between_src_and_des.total_t_in_minute += source_temp_node.walkTimeMin + dest_temp_node.walkTimeMin;
                    result_between_src_and_des.Walk_dis_km = Math.Round((source_temp_node.walkDistKm + dest_temp_node.walkDistKm), 2);
                    result_between_src_and_des.total_dis_in_km = Math.Round((result_between_src_and_des.Walk_dis_km
                        + result_between_src_and_des.vehicle_dis_in_km), 2);


                    if (min_time > result_between_src_and_des.total_t_in_minute)
                    {
                        optimal_route_result.total_t_in_minute = min_time = result_between_src_and_des.total_t_in_minute;
                        optimal_route_result.Walk_dis_km = result_between_src_and_des.Walk_dis_km;
                        optimal_route_result.vehicle_dis_in_km = result_between_src_and_des.vehicle_dis_in_km;
                        optimal_route_result.total_dis_in_km = result_between_src_and_des.total_dis_in_km;
                    }
                }
            }
            return optimal_route_result;
        }

        private output_result Two_way_AStar()
        {
            output_result ret = new output_result();
            return ret;
        }
    }
}
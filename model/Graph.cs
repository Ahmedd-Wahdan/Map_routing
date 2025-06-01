using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MAP_routing.model
{
    public class Node
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsPath { get; set; } = false;
        public Color Color { get; set; } = Color.Black;
        public List<Edge> Neighbors { get; set; } = new List<Edge>();
    }

    public class Edge
    {
        public Node From { get; set; }
        public Node To { get; set; }
        public double LengthKm { get; set; }
        public double SpeedKmh { get; set; }
        // 
        public List<double> SpeedKmhList { get; set; } = new List<double>();
        public Color Color { get; set; } = Color.Gray;
        public bool IsPath { get; set; } = false;


        public double GetTimeMin(double currentTimeMin)
        {
            if ( !MapRouting.IsVariableSpeedMode)
            {
                if (SpeedKmh <= 0)
                {
                    return LengthKm * 60; 
                }
                return (LengthKm / SpeedKmh) * 60;
            }

            int timeSlot = (int)Math.Floor(currentTimeMin / MapRouting.SpeedIntervalMinutes) % SpeedKmhList.Count;
            double speed = SpeedKmhList[timeSlot];
            if (speed <= 0)
            {
                return (LengthKm / (SpeedKmh > 0 ? SpeedKmh : 1)) * 60;
            }
            return (LengthKm / speed) * 60;
        }


        public double TimeMin => GetTimeMin(0);
    }

    public class Query
    {
        public double SourceX { get; set; }
        public double SourceY { get; set; }
        public double DestX { get; set; }
        public double DestY { get; set; }
        public double Rmeters { get; set; }

        public Query(double sourceX, double sourceY, double destX, double destY, double rmeters)
        {
            SourceX = sourceX;
            SourceY = sourceY;
            DestX = destX;
            DestY = destY;
            Rmeters = rmeters;
        }

        public static List<Query> ReadFromFile(string filePath)
        {
            Stopwatch processquery_stopWatch = Stopwatch.StartNew();
            List<Query> Queries = new List<Query>();

            string[] lines;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch (IOException ex)
            {
                throw new IOException($"Error reading queries file {filePath}: {ex.Message}");
            }

            int lineIndex = 0;

            if (!int.TryParse(lines[lineIndex++].Trim(), out int queryCount))
                throw new FormatException("Invalid number of queries");

            for (int i = 0; i < queryCount; i++)
            {
                if (lineIndex >= lines.Length)
                    throw new FormatException("Unexpected end of file while reading queries");

                string[] parts = lines[lineIndex++].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 5)
                    throw new FormatException($"Invalid query format at line {lineIndex}");

                if (!double.TryParse(parts[0], out double startX) ||
                    !double.TryParse(parts[1], out double startY) ||
                    !double.TryParse(parts[2], out double endX) ||
                    !double.TryParse(parts[3], out double endY) ||
                    !int.TryParse(parts[4], out int Rmeter))
                    throw new FormatException($"Invalid query data at line {lineIndex}");

                Queries.Add(new Query(startX, startY, endX, endY, Rmeter));
            }
            if (Queries.Count == 0)
                throw new FormatException("No queries found in the file");

            processquery_stopWatch.Stop();
            MapRouting.total_time_of_IO += processquery_stopWatch.ElapsedMilliseconds;

            return Queries;
        }
    }

    public class PathResult
    {
        public Node source { get; set; }
        public Node dest { get; set; }
        public List<int> Path { get; set; } = new List<int>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public double TotalTimeMin { get; set; }
        public double TotalDistanceKm { get; set; }
        public double WalkingDistanceKm { get; set; }
        public double VehicleDistanceKm { get; set; }
    }

    public class MapRouting
    {
        public List<Node> graph = new List<Node>();
        public List<Edge> edges = new List<Edge>();
        public double maxSpeedKmh = 0.0;
        public static double total_time_of_IO = 0;
        public static int total_number_of_edges = 0;
        public static int total_number_of_nodes = 0;

        // 
        public static void ResetMode()
        {
            IsVariableSpeedMode = false;
            SpeedCount = 1;
            SpeedIntervalMinutes = 0;
        }

        public static bool IsVariableSpeedMode { get; private set; } = false;
        // 
        public static int SpeedCount { get; private set; } = 1;
        // 
        public static int SpeedIntervalMinutes { get; private set; } = 0;

        public MapRouting(string mapFile)
        {
            ReadMapFile(mapFile);
            if (graph == null || graph.Count == 0)
            {
                throw new ArgumentException("Graph cannot be null or empty. (in graph.cs)");
            }
        }

        private void ReadMapFile(string mapFile)
        {
            Stopwatch IO_stopwatch = Stopwatch.StartNew();

            var lines = File.ReadAllLines(mapFile);
            int lineIndex = 0;

            // Read number of intersections
            int N = int.Parse(lines[lineIndex++]);

            if (graph == null)
            {
                MessageBox.Show("Graph is null");
            }

            // Read intersections
            for (int i = 0; i < N; i++)
            {
                var parts = lines[lineIndex++].Split(' ').Select(double.Parse).ToArray();
                graph.Add(new Node { Id = (int)parts[0], X = parts[1], Y = parts[2] });
            }

            // 
            // Read roads info
            var roadInfoParts = lines[lineIndex++].Split(' ').Select(int.Parse).ToArray();
            int M = roadInfoParts[0];

            // 
            // Check if we're in variable speed mode
            if (roadInfoParts.Length >= 3)
            {
                IsVariableSpeedMode = true;
                SpeedCount = roadInfoParts[1];
                SpeedIntervalMinutes = roadInfoParts[2];
            }

            // 
            // Read roads
            maxSpeedKmh = 0;
            for (int i = 0; i < M; i++)
            {
                var parts = lines[lineIndex++].Split(' ').Select(double.Parse).ToArray();
                int id1 = (int)parts[0];
                int id2 = (int)parts[1];
                double lengthKm = parts[2];

                var node1 = graph[id1];
                var node2 = graph[id2];

                Edge edge1, edge2;

                if (IsVariableSpeedMode)
                {
                    edge1 = new Edge
                    {
                        From = node1,
                        To = node2,
                        LengthKm = lengthKm
                    };

                    edge2 = new Edge
                    {
                        From = node2,
                        To = node1,
                        LengthKm = lengthKm
                    };

                    for (int j = 0; j < SpeedCount; j++)
                    {
                        double speed = parts[3 + j];
                        edge1.SpeedKmhList.Add(speed);
                        edge2.SpeedKmhList.Add(speed);
                        maxSpeedKmh = Math.Max(maxSpeedKmh, speed);
                    }

                    edge1.SpeedKmh = edge1.SpeedKmhList[0];
                    edge2.SpeedKmh = edge2.SpeedKmhList[0];
                }
                else
                {
                    double speedKmh = parts[3];
                    edge1 = new Edge { From = node1, To = node2, LengthKm = lengthKm, SpeedKmh = speedKmh };
                    edge2 = new Edge { From = node2, To = node1, LengthKm = lengthKm, SpeedKmh = speedKmh };
                    maxSpeedKmh = Math.Max(maxSpeedKmh, speedKmh);
                }

                // Add bidirectional edges
                node1.Neighbors.Add(edge1);
                node2.Neighbors.Add(edge2);
                edges.Add(edge1);
                edges.Add(edge2);
            }

            IO_stopwatch.Stop();
            total_time_of_IO += IO_stopwatch.ElapsedMilliseconds;
            total_number_of_nodes = N;
            total_number_of_edges = M;
        }
    }
}
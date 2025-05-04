using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP_routing.model
{
    public class Query
    {
        public float StartX { get; set; }
        public float StartY { get; set; }
        public float EndX { get; set; }
        public float EndY { get; set; }
        public int QueryId { get; set; }

        public Query(float startX, float startY, float endX, float endY, int queryId)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            QueryId = queryId;
        }
    }

    public class Queries
    {
        public List<Query> QueryList { get; private set; } = new List<Query>();

        public void Clear()
        {
            QueryList.Clear();
        }

        public void ReadFromFile(string filePath)
        {
            Clear();

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

                if (!float.TryParse(parts[0], out float startX) ||
                    !float.TryParse(parts[1], out float startY) ||
                    !float.TryParse(parts[2], out float endX) ||
                    !float.TryParse(parts[3], out float endY) ||
                    !int.TryParse(parts[4], out int queryId))
                    throw new FormatException($"Invalid query data at line {lineIndex}");

                QueryList.Add(new Query(startX, startY, endX, endY, queryId));
            }
        }

        public Query GetQuery(int index)
        {
            if (index < 0 || index >= QueryList.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Query index is out of range");

            return QueryList[index];
        }

        public int GetQueryCount()
        {
            return QueryList.Count;
        }

        public static int FindNearestNode(Dictionary<int, Node> nodes, float x, float y)
        {
            if (nodes == null || nodes.Count == 0)
                throw new ArgumentException("Nodes dictionary is empty or null");

            int nearestNodeId = -1;
            float minDistance = float.MaxValue;

            foreach (var node in nodes.Values)
            {
                float distance = CalculateEuclideanDistance(x, y, node.X, node.Y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNodeId = node.Id;
                }
            }

            return nearestNodeId;
        }

        private static float CalculateEuclideanDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
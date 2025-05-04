namespace MAP_routing.model
{

    public class Node
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Color Color { get; set; } = Color.Blue;
        public bool IsPath { get; set; } = false;

        public Node(int id, float x, float y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }

    public class Edge
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public float Length { get; set; }
        public int Speed { get; set; }
        public Color Color { get; set; } = Color.Gray;
        public bool IsPath { get; set; } = false;
        public float Distance => Length;

        public Edge(int fromId, int toId, float length, int speed)
        {
            FromId = fromId;
            ToId = toId;
            Length = length;
            Speed = speed;
        }
    }

    public class Graph
    {
        public Dictionary<int, Node> Nodes { get; private set; } = new Dictionary<int, Node>();
        public List<Edge> Edges { get; private set; } = new List<Edge>();

        public void AddNode(int id, float x, float y, Color? color = null)
        {
            var node = new Node(id, x, y);
            if (color.HasValue) node.Color = color.Value;
            Nodes[id] = node;
        }

        public void AddEdge(int fromId, int toId, float length, int speed, Color? color = null)
        {
            var edge = new Edge(fromId, toId, length, speed);
            if (color.HasValue) edge.Color = color.Value;
            Edges.Add(edge);
        }

        public void Clear()
        {
            Nodes.Clear();
            Edges.Clear();
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
                throw new IOException($"Error reading file {filePath}: {ex.Message}");
            }

            int lineIndex = 0;

            if (!int.TryParse(lines[lineIndex++].Trim(), out int n))
                throw new FormatException("Invalid number of intersections");

            for (int i = 0; i < n; i++)
            {
                if (lineIndex >= lines.Length)
                    throw new FormatException("Unexpected end of file while reading intersections");

                string[] parts = lines[lineIndex++].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                    throw new FormatException($"Invalid intersection format at line {lineIndex}");

                if (!int.TryParse(parts[0], out int id) ||
                    !float.TryParse(parts[1], out float x) ||
                    !float.TryParse(parts[2], out float y))
                    throw new FormatException($"Invalid intersection data at line {lineIndex}");

                AddNode(id, x, y);
            }

            if (lineIndex >= lines.Length)
                throw new FormatException("Unexpected end of file while reading number of roads");

            if (!int.TryParse(lines[lineIndex++].Trim(), out int m))
                throw new FormatException("Invalid number of roads");

            for (int i = 0; i < m; i++)
            {
                if (lineIndex >= lines.Length)
                    throw new FormatException("Unexpected end of file while reading roads");

                string[] parts = lines[lineIndex++].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 4)
                    throw new FormatException($"Invalid road format at line {lineIndex}");

                if (!int.TryParse(parts[0], out int fromId) ||
                    !int.TryParse(parts[1], out int toId) ||
                    !float.TryParse(parts[2], out float length) ||
                    !int.TryParse(parts[3], out int speed))
                    throw new FormatException($"Invalid road data at line {lineIndex}");

                AddEdge(fromId, toId, length, speed);
                AddEdge(toId, fromId, length, speed);
            }
        }

        public Dictionary<int, Node> GetNodes()
        {
            return Nodes;
        }

        public Edge GetEdge(int fromId, int toId)
        {
            return Edges.FirstOrDefault(e => e.FromId == fromId && e.ToId == toId);
        }
    }
 }
}
 



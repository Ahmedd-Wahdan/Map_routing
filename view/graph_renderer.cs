using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms.VisualStyles;
using MAP_routing.model;
namespace MAP_routing.view
{
    internal class graph_renderer
    {
        private readonly List<Node> _graph;
        private readonly Panel _panel;

        private double _scale = 1.0f;
        private PointF _offset = new PointF(0, 0);
        private bool _isPanning = false;
        private Point _lastMousePosition;

        private PointF _viewCenter = new PointF(0, 0);

        private Dictionary<int, Color> _highlightedNodes = new Dictionary<int, Color>();
        private List<Edge> _edges;
        private List<Edge> _highlightedPath = new List<Edge>();

        public graph_renderer(List<Node> graph, List<Edge> edges, Panel panel)
        {
            _graph = graph;
            _panel = panel;
            _edges = edges;
            CenterGraph();
            HookEvents();
        }

        #region Public API

        public void Redraw() => _panel.Invalidate();

        public void CenterGraph()
        {
            if (_graph.Count == 0) return;

            double minX = _graph.Min(n => n.X);
            double maxX = _graph.Max(n => n.X);
            double minY = _graph.Min(n => n.Y);
            double maxY = _graph.Max(n => n.Y);

            double centerX = (minX + maxX) / 2;
            double centerY = (minY + maxY) / 2;

            double graphWidth = maxX - minX;
            double graphHeight = maxY - minY;

            if (graphWidth > 0 && graphHeight > 0)
            {
                double scaleX = (_panel.Width * 0.9f) / graphWidth;
                double scaleY = (_panel.Height * 0.9f) / graphHeight;
                _scale = Math.Min(scaleX, scaleY);
                _scale = Math.Max(0.01f, Math.Min(10f, _scale));
            }

            _viewCenter = new PointF((float)centerX, (float)centerY);
            UpdateOffsetFromViewCenter();
        }

        public void HighlightNode(Node node, Color color)
        {
            if (node != null)
            {
                _highlightedNodes[node.Id] = color;
            }
        }

        public void ClearHighlightedNodes()
        {
            _highlightedNodes.Clear();
        }

        public void HighlightPath(PathResult path, Color color)
        {
            if (path == null || path.Path.Count < 2)
                return;

            _highlightedPath.Clear();

            for (int i = 0; i < path.Path.Count - 1; i++)
            {
                int fromId = path.Path[i];
                int toId = path.Path[i + 1];

                // Find the edge in the graph
                Node fromNode = _graph.FirstOrDefault(n => n.Id == fromId);
                if (fromNode != null)
                {
                    Edge edge = fromNode.Neighbors.FirstOrDefault(e => e.To.Id == toId);
                    if (edge != null)
                    {
                        // Create a copy of the edge for highlighting
                        Edge highlightedEdge = new Edge
                        {
                            To = edge.To,
                            LengthKm = edge.LengthKm,
                            SpeedKmh = edge.SpeedKmh
                        };
                        _highlightedPath.Add(highlightedEdge);
                    }
                }
            }
        }
        public void ClearHighlightedPath()
        {
            _highlightedPath.Clear();
        }

        #endregion

        private void UpdateOffsetFromViewCenter()
        {
            _offset.X = (float)(_panel.Width / 2 - _viewCenter.X * _scale);
            _offset.Y = (float)(_panel.Height / 2 + _viewCenter.Y * _scale);
        }

        #region Event Hooks

        private void HookEvents()
        {
            _panel.Paint += OnPaint;
            _panel.MouseWheel += OnMouseWheel;
            _panel.MouseDown += OnMouseDown;
            _panel.MouseMove += OnMouseMove;
            _panel.MouseUp += OnMouseUp;
            _panel.Resize += (s, e) => _panel.Invalidate();
        }

        #endregion

        #region Paint Handler

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            g.TranslateTransform(_offset.X, _offset.Y);
            g.ScaleTransform((float)_scale, (float)-_scale);

            DrawBoundingBox(g);
            DrawGraph(g);
            DrawHighlightedPath(g);
            DrawHighlightedNodes(g);
        }

        #endregion

        #region Drawing Methods

        private void DrawGraph(Graphics g)
        {
            RectangleF visibleBounds = GetVisibleWorldBounds();

            foreach (var node in _graph)
            {
                foreach (var edge in node.Neighbors)
                {
                    if (visibleBounds.Contains((float)node.X, (float)node.Y) ||
                        visibleBounds.Contains((float)edge.To.X, (float)edge.To.Y))
                    {
                        DrawEdge(g, node, edge);
                    }
                }
            }

            foreach (Node node in _graph)
            {
                if (visibleBounds.Contains((float)node.X, (float)node.Y))
                {
                    DrawNode(g, node);
                }
            }
        }

        private void DrawHighlightedPath(Graphics g)
        {
            foreach (var edge in _highlightedPath)
            {
                Node from = _graph.FirstOrDefault(n => n.Neighbors.Contains(edge));
                if (from != null)
                {
                    using var pen = new Pen(Color.Blue, 3f / (float)_scale);
                    g.DrawLine(pen, (float)from.X, (float)from.Y, (float)edge.To.X, (float)edge.To.Y);
                }
            }
        }

        private void DrawHighlightedNodes(Graphics g)
        {
            foreach (var kvp in _highlightedNodes)
            {
                int nodeId = kvp.Key;
                Color highlightColor = kvp.Value;

                Node node = _graph.FirstOrDefault(n => n.Id == nodeId);
                if (node != null)
                {
                    float radius = 100f / (float)_scale;

                    var rect = new RectangleF(
                        (float)node.X - radius, (float)node.Y - radius,
                        radius * 2, radius * 2
                    );

                    using var brush = new SolidBrush(highlightColor);
                    using var pen = new Pen(Color.Black, 1.5f / (float)_scale);

                    g.FillEllipse(brush, rect);
                    g.DrawEllipse(pen, rect);
                }
            }
        }

        public void DrawNode(Graphics g, Node node)
        {
            if (_highlightedNodes.ContainsKey(node.Id))
                return;

            float radius = 3f;

            var rect = new RectangleF(
                (float)node.X - radius, (float)node.Y - radius,
                radius * 2, radius * 2
            );

            using var brush = new SolidBrush(node.IsPath ? Color.Red : node.Color);
            using var pen = new Pen(Color.Black, 1);

            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
        }

        public void DrawEdge(Graphics g, Node from, Edge edge)
        {
            if (_highlightedPath.Any(e => e.To.Id == edge.To.Id && _graph.FirstOrDefault(n => n.Neighbors.Contains(e))?.Id == from.Id))
                return;

            using var pen = new Pen(Color.Gray, 1f / (float)_scale);
            g.DrawLine(pen, (float)from.X, (float)from.Y, (float)edge.To.X, (float)edge.To.Y);
        }

        private void DrawBoundingBox(Graphics g)
        {
            if (_graph.Count == 0) return;

            double minX = _graph.Min(n => n.X);
            double maxX = _graph.Max(n => n.X);
            double minY = _graph.Min(n => n.Y);
            double maxY = _graph.Max(n => n.Y);

            var rect = new RectangleF((float)minX, (float)minY, (float)(maxX - minX), (float)(maxY - minY));
            using var pen = new Pen(Color.LightGray, (float)(5f / _scale)) { DashStyle = DashStyle.Dash };
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        #endregion

        private RectangleF GetVisibleWorldBounds()
        {
            double left = (0 - _offset.X) / _scale;
            double top = -((0 - _offset.Y) / _scale);
            double right = (_panel.Width - _offset.X) / _scale;
            double bottom = -((_panel.Height - _offset.Y) / _scale);

            return new RectangleF(
                (float)Math.Min(left, right),
                (float)Math.Min(top, bottom),
                (float)Math.Abs(right - left),
                (float)Math.Abs(bottom - top)
            );
        }

        #region Coordinate Conversions

        private PointF ScreenToWorld(Point screenPoint)
        {
            return new PointF(
                (float)((screenPoint.X - _offset.X) / _scale),
                (float)(-((screenPoint.Y - _offset.Y) / _scale))
            );
        }

        #endregion

        #region Mouse Interaction

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            PointF worldPoint = ScreenToWorld(e.Location);

            double oldScale = _scale;
            double zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            _scale *= zoomFactor;
            _scale = Math.Max(0.01f, Math.Min(10f, _scale));

            double scaleRatio = _scale / oldScale;

            _viewCenter.X = (float)(worldPoint.X + (_viewCenter.X - worldPoint.X) / scaleRatio);
            _viewCenter.Y = (float)(worldPoint.Y + (_viewCenter.Y - worldPoint.Y) / scaleRatio);

            UpdateOffsetFromViewCenter();

            Redraw();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPanning = true;
                _lastMousePosition = e.Location;
                _panel.Cursor = Cursors.Hand;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                float dx = e.X - _lastMousePosition.X;
                float dy = e.Y - _lastMousePosition.Y;

                double worldDx = dx / _scale;
                double worldDy = -dy / _scale;

                _viewCenter.X -= (float)worldDx;
                _viewCenter.Y -= (float)worldDy;

                UpdateOffsetFromViewCenter();

                _lastMousePosition = e.Location;
                Redraw();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                _panel.Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}
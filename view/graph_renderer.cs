using System.Drawing.Drawing2D;
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
        private List<Edge> _highlightedPath = new List<Edge>();
        private Dictionary<Edge, Tuple<Color, string>> _highlightedEdges = new Dictionary<Edge, Tuple<Color, string>>();

        public graph_renderer(List<Node> graph, Panel panel)
        {
            _graph = graph;
            _panel = panel;
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

        public void ClearHighlightedNodes()
        {
            _highlightedNodes.Clear();
        }

        public void HighlightPath(PathResult path, Color color)
        {
            if (path == null || path.Edges == null || path.Edges.Count == 0)
                return;

            _highlightedPath.Clear();

            foreach (var edge in path.Edges)
            {
                edge.IsPath = true;
                edge.Color = color;
                _highlightedPath.Add(edge);
            }
        }

        public void ClearHighlightedPath()
        {
            _highlightedPath.Clear();
            _highlightedEdges.Clear();
        }

        public void HighlightEdge(Edge edge, Color color, string label)
        {
            if (edge == null) return;

            _highlightedEdges.Clear();
            _highlightedEdges[edge] = new Tuple<Color, string>(color, label);
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
        }

        #endregion

        #region Drawing Methods

        private void DrawGraph(Graphics g)
        {
            RectangleF visibleBounds = GetVisibleWorldBounds();

            using var pen = new Pen(Color.Gray, 1f / (float)_scale);
            foreach (var node in _graph)
            {
                foreach (var edge in node.Neighbors)
                {
                    if (node.Id >= edge.To.Id) continue;

                    if (!(visibleBounds.Contains((float)node.X, (float)node.Y)
                        || visibleBounds.Contains((float)edge.To.X, (float)edge.To.Y)))
                        continue;

                    g.DrawLine(pen,
                               (float)node.X, (float)node.Y,
                               (float)edge.To.X, (float)edge.To.Y);
                }
            }

            PathResult currentPath = GetCurrentPathResult();
            if (currentPath != null)
            {
                DrawHighlightedPath(g, currentPath);
                DrawWalkingPaths(g, currentPath);
                DrawSingleHighlightedEdge(g);
            }

            foreach (Node node in _graph)
            {
                if (visibleBounds.Contains((float)node.X, (float)node.Y))
                    DrawNode(g, node);
            }

            DrawQueryPoints(g);
        }


        private void DrawHighlightedPath(Graphics g, PathResult currentPath)
        {
            if (_highlightedPath.Count == 0) return;

            using var pen = new Pen(Color.Blue, 3f / (float)_scale);
            foreach (var edge in _highlightedPath)
            {
                if(edge.From.Id == -1 || edge.To.Id == -2) continue;
                if (!_highlightedEdges.ContainsKey(edge))
                {
                    g.DrawLine(pen, (float)edge.From.X, (float)edge.From.Y, (float)edge.To.X, (float)edge.To.Y);
                }
            }
        }

        private void DrawWalkingPaths(Graphics g, PathResult currentPath)
        {
            if (currentPath == null || _highlightedPath.Count == 0) return;

            using var greenPen = new Pen(Color.Green, 3f / (float)_scale) { DashStyle = DashStyle.Dash };
            using var redPen = new Pen(Color.Red, (float)(3f / _scale)) { DashStyle = DashStyle.Dash };

            if (currentPath.source != null && currentPath.source.Id == -1 && _highlightedPath.Any())
            {
                Node firstNode = _highlightedPath.First().To;

                g.DrawLine(greenPen,
                    (float)currentPath.source.X, (float)currentPath.source.Y,
                    (float)firstNode.X, (float)firstNode.Y);
            }

            if (currentPath.dest != null && currentPath.dest.Id == -2 && _highlightedPath.Any())
            {
                var lastEdge = _highlightedPath[_highlightedPath.Count - 2];

                g.DrawLine(redPen,
                    (float)lastEdge.To.X, (float)lastEdge.To.Y,
                    (float)currentPath.dest.X, (float)currentPath.dest.Y);
            }
        }

        private void DrawSingleHighlightedEdge(Graphics g)
        {
            if (_highlightedEdges.Count == 0) return;

            var entry = _highlightedEdges.First();
            Edge edge = entry.Key;
            Color color = entry.Value.Item1;
            string label = entry.Value.Item2;

            using var pen = new Pen(color, 5f / (float)_scale);
            g.DrawLine(pen,
                (float)edge.From.X, (float)edge.From.Y,
                (float)edge.To.X, (float)edge.To.Y);

            if (!string.IsNullOrEmpty(label))
            {
                PointF midpoint = new PointF(
                    (float)((edge.From.X + edge.To.X) / 2),
                    (float)((edge.From.Y + edge.To.Y) / 2)
                );

                float dx = (float)(edge.To.X - edge.From.X);
                float dy = (float)(edge.To.Y - edge.From.Y);

                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                if (length > 0)
                {
                    dx /= length;
                    dy /= length;

                    float perpX = -dy;
                    float perpY = dx;

                    float pointerLength = 160f / (float)_scale;
                    PointF boxPosition = new PointF(
                        midpoint.X + perpX * pointerLength,
                        midpoint.Y + perpY * pointerLength
                    );

                    Matrix originalTransform = g.Transform;
                    g.ResetTransform();

                    PointF screenMidpoint = new PointF(
                        _offset.X + midpoint.X * (float)_scale,
                        _offset.Y - midpoint.Y * (float)_scale
                    );
                    PointF screenBoxPos = new PointF(
                        _offset.X + boxPosition.X * (float)_scale,
                        _offset.Y - boxPosition.Y * (float)_scale
                    );

                    using var font = new Font("Arial", 10f);
                    using var brush = new SolidBrush(Color.White);

                    // Define the two lines of text
                    string line1 = label;

                    // Measure text sizes
                    SizeF textSize1 = g.MeasureString(line1, font);

                    // Calculate box dimensions
                    float maxWidth =(textSize1.Width);
                    float totalHeight = textSize1.Height ;

                    // Position the box so the bottom aligns with the pointer
                    float boxTop = screenBoxPos.Y - totalHeight;

                    using var bgBrush = new SolidBrush(Color.Black);
                    g.FillRectangle(bgBrush,
                        screenBoxPos.X - maxWidth / 2,
                        boxTop,
                        maxWidth,
                        totalHeight);
                    g.DrawRectangle(new Pen(Color.Black, 1),
                        screenBoxPos.X - maxWidth / 2,
                        boxTop,
                        maxWidth,
                        totalHeight);

                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    // Draw first line
                    g.DrawString(line1, font, brush,
                        new PointF(screenBoxPos.X, boxTop + textSize1.Height / 2), format);

                    // Draw second line
                   

                    // Draw pointer to the bottom center of the box
                    PointF screenBoxBottom = new PointF(screenBoxPos.X, boxTop + totalHeight);
                    using var pointerPen = new Pen(Color.Black, 1);
                    g.DrawLine(pointerPen, screenMidpoint, screenBoxBottom);

                    g.Transform = originalTransform;
                }
            }
        }

        private void DrawQueryPoints(Graphics g)
        {
            PathResult path = GetCurrentPathResult();
            if (path == null || path.source == null || path.dest == null)
                return;

            if (path.source.Id == -1)
            {
                float sourceRadius = Math.Max(10f / (float)_scale, 3f);
                var sourceRect = new RectangleF(
                    (float)path.source.X - sourceRadius,
                    (float)path.source.Y - sourceRadius,
                    sourceRadius * 2,
                    sourceRadius * 2
                );

                using var sourceBrush = new SolidBrush(Color.Green);
                using var sourcePen = new Pen(Color.Black, 1.5f / (float)_scale);
                g.FillEllipse(sourceBrush, sourceRect);
                g.DrawEllipse(sourcePen, sourceRect);
            }

            if (path.dest.Id == -2)
            {
                float destRadius = Math.Max(10f / (float)_scale, 3f);
                var destRect = new RectangleF(
                    (float)path.dest.X - destRadius,
                    (float)path.dest.Y - destRadius,
                    destRadius * 2,
                    destRadius * 2
                );

                using var destBrush = new SolidBrush(Color.Red);
                using var destPen = new Pen(Color.Black, 1.5f / (float)_scale);
                g.FillEllipse(destBrush, destRect);
                g.DrawEllipse(destPen, destRect);
            }
        }

        public void DrawNode(Graphics g, Node node)
        {
            if (_highlightedNodes.ContainsKey(node.Id))
                return;

            float baseRadius = 3f;

            float dynamicRadius = _scale > 1.0
                ? baseRadius / (float)Math.Sqrt(_scale)
                : baseRadius;

            float minRadius = 1.5f;
            dynamicRadius = Math.Max(dynamicRadius, minRadius);

            var rect = new RectangleF(
                (float)node.X - dynamicRadius, (float)node.Y - dynamicRadius,
                dynamicRadius * 2, dynamicRadius * 2
            );

            using var brush = new SolidBrush(node.IsPath ? Color.Red : node.Color);
            using var pen = new Pen(Color.Black, 1 / (float)Math.Max(1.0, _scale));

            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
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

        private PathResult GetCurrentPathResult()
        {
            Control parent = _panel;
            while (parent != null && !(parent is map_vis))
            {
                parent = parent.Parent;
            }

            return (parent as map_vis)?.GetCurrentPathResult();
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
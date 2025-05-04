using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MAP_routing.model;

namespace MAP_routing.view
{
    internal class graph_renderer
    {
        private readonly Graph _graph;
        private readonly Panel _panel;

        private float _scale = 1.0f;
        private PointF _offset = new PointF(0, 0);
        private bool _isPanning = false;
        private Point _lastMousePosition;

        private PointF _viewCenter = new PointF(0, 0);

        private Dictionary<int, Color> _highlightedNodes = new Dictionary<int, Color>();
        private List<Edge> _highlightedPath = new List<Edge>();

        public graph_renderer(Graph graph, Panel panel)
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
            if (_graph.Nodes.Count == 0) return;

            float minX = _graph.Nodes.Values.Min(n => n.X);
            float maxX = _graph.Nodes.Values.Max(n => n.X);
            float minY = _graph.Nodes.Values.Min(n => n.Y);
            float maxY = _graph.Nodes.Values.Max(n => n.Y);

            float centerX = (minX + maxX) / 2;
            float centerY = (minY + maxY) / 2;

            float graphWidth = maxX - minX;
            float graphHeight = maxY - minY;

            if (graphWidth > 0 && graphHeight > 0)
            {
                float scaleX = (_panel.Width * 0.9f) / graphWidth;
                float scaleY = (_panel.Height * 0.9f) / graphHeight;
                _scale = Math.Min(scaleX, scaleY);
                _scale = Math.Max(0.01f, Math.Min(10f, _scale));
            }

            _viewCenter = new PointF(centerX, centerY);
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

        public void ClearHighlightedPath()
        {
            _highlightedPath.Clear();
        }

        #endregion

        private void UpdateOffsetFromViewCenter()
        {
            _offset.X = _panel.Width / 2f - _viewCenter.X * _scale;
            _offset.Y = _panel.Height / 2f + _viewCenter.Y * _scale;
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
            g.ScaleTransform(_scale, -_scale);

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

            foreach (var edge in _graph.Edges)
            {
                if (_graph.Nodes.TryGetValue(edge.FromId, out var from) &&
                    _graph.Nodes.TryGetValue(edge.ToId, out var to))
                {
                    if (visibleBounds.Contains(from.X, from.Y) || visibleBounds.Contains(to.X, to.Y))
                    {
                        DrawEdge(g, edge);
                    }
                }
            }

            foreach (var node in _graph.Nodes.Values)
            {
                if (visibleBounds.Contains(node.X, node.Y))
                {
                    DrawNode(g, node);
                }
            }
        }

        private void DrawHighlightedPath(Graphics g)
        {
            foreach (var edge in _highlightedPath)
            {
                if (_graph.Nodes.TryGetValue(edge.FromId, out var from) &&
                    _graph.Nodes.TryGetValue(edge.ToId, out var to))
                {
                    using var pen = new Pen(edge.Color, 3f);
                    g.DrawLine(pen, from.X, from.Y, to.X, to.Y);
                }
            }
        }

        private void DrawHighlightedNodes(Graphics g)
        {
            foreach (var kvp in _highlightedNodes)
            {
                int nodeId = kvp.Key;
                Color highlightColor = kvp.Value;

                if (_graph.Nodes.TryGetValue(nodeId, out var node))
                {
                    float radius = 5f;

                    var rect = new RectangleF(
                        node.X - radius, node.Y - radius,
                        radius * 2, radius * 2
                    );

                    using var brush = new SolidBrush(highlightColor);
                    using var pen = new Pen(Color.Black, 1.5f);

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
                node.X - radius, node.Y - radius,
                radius * 2, radius * 2
            );

            using var brush = new SolidBrush(node.IsPath ? Color.Red : node.Color);
            using var pen = new Pen(Color.Black, 1);

            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
        }

        public void DrawEdge(Graphics g, Edge edge)
        {
            if (_highlightedPath.Any(e => e.FromId == edge.FromId && e.ToId == edge.ToId))
                return;

            if (!_graph.Nodes.TryGetValue(edge.FromId, out var from) ||
                !_graph.Nodes.TryGetValue(edge.ToId, out var to))
                return;

            using var pen = new Pen(edge.IsPath ? Color.Red : edge.Color, edge.IsPath ? 3f : 1f);
            g.DrawLine(pen, from.X, from.Y, to.X, to.Y);
        }

        private void DrawBoundingBox(Graphics g)
        {
            if (_graph.Nodes.Count == 0) return;

            float minX = _graph.Nodes.Values.Min(n => n.X);
            float maxX = _graph.Nodes.Values.Max(n => n.X);
            float minY = _graph.Nodes.Values.Min(n => n.Y);
            float maxY = _graph.Nodes.Values.Max(n => n.Y);

            var rect = new RectangleF(minX, minY, maxX - minX, maxY - minY);
            using var pen = new Pen(Color.LightGray, 5f / _scale) { DashStyle = DashStyle.Dash };
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        #endregion

        private RectangleF GetVisibleWorldBounds()
        {
            float left = (0 - _offset.X) / _scale;
            float top = -((0 - _offset.Y) / _scale);
            float right = (_panel.Width - _offset.X) / _scale;
            float bottom = -((_panel.Height - _offset.Y) / _scale);

            return new RectangleF(
                Math.Min(left, right),
                Math.Min(top, bottom),
                Math.Abs(right - left),
                Math.Abs(bottom - top)
            );
        }

        #region Coordinate Conversions

        private PointF ScreenToWorld(Point screenPoint)
        {
            return new PointF(
                (screenPoint.X - _offset.X) / _scale,
                -((screenPoint.Y - _offset.Y) / _scale)
            );
        }

        #endregion

        #region Mouse Interaction

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            PointF worldPoint = ScreenToWorld(e.Location);

            float oldScale = _scale;
            float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            _scale *= zoomFactor;
            _scale = Math.Max(0.01f, Math.Min(10f, _scale));

            float scaleRatio = _scale / oldScale;

            _viewCenter.X = worldPoint.X + (_viewCenter.X - worldPoint.X) / scaleRatio;
            _viewCenter.Y = worldPoint.Y + (_viewCenter.Y - worldPoint.Y) / scaleRatio;

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

                float worldDx = dx / _scale;
                float worldDy = -dy / _scale;

                _viewCenter.X -= worldDx;
                _viewCenter.Y -= worldDy;

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
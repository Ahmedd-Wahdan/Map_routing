using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MAP_routing.model;

namespace MAP_routing.view
{
    internal class graph_renderrer
    {
        private readonly Graph _graph;
        private readonly Panel _panel;

        private float _scale = 1.0f;
        private PointF _offset = new PointF(0, 0);
        private bool _isPanning = false;
        private Point _lastMousePosition;

        // World coordinates of the viewport center
        private PointF _viewCenter = new PointF(0, 0);

        public graph_renderrer(Graph graph, Panel panel)
        {
            _graph = graph;
            _panel = panel;

            // Initialize by centering the graph
            CenterGraph();
            HookEvents();
        }

        #region Public API

        public void Redraw() => _panel.Invalidate();

        public void CenterGraph()
        {
            if (_graph.Nodes.Count == 0) return;

            // Calculate the center of the graph
            float minX = _graph.Nodes.Values.Min(n => n.X);
            float maxX = _graph.Nodes.Values.Max(n => n.X);
            float minY = _graph.Nodes.Values.Min(n => n.Y);
            float maxY = _graph.Nodes.Values.Max(n => n.Y);

            float centerX = (minX + maxX) / 2;
            float centerY = (minY + maxY) / 2;

            // Calculate the necessary scale to fit the graph
            float graphWidth = maxX - minX;
            float graphHeight = maxY - minY;

            if (graphWidth > 0 && graphHeight > 0)
            {
                float scaleX = (_panel.Width * 0.9f) / graphWidth;
                float scaleY = (_panel.Height * 0.9f) / graphHeight;
                _scale = Math.Min(scaleX, scaleY);
                _scale = Math.Max(0.01f, Math.Min(10f, _scale)); // Clamp scale
            }

            // Update view center and recalculate offset
            _viewCenter = new PointF(centerX, centerY);
            UpdateOffsetFromViewCenter();
        }

        #endregion

        private void UpdateOffsetFromViewCenter()
        {
            // Calculate offset from view center and scale
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

            // Apply transformations
            g.TranslateTransform(_offset.X, _offset.Y);
            g.ScaleTransform(_scale, -_scale);

            DrawBoundingBox(g);
            DrawGraph(g);
        }

        #endregion

        #region Drawing Methods

        private void DrawGraph(Graphics g)
        {
            foreach (var edge in _graph.Edges)
                DrawEdge(g, edge);

            foreach (var node in _graph.Nodes.Values)
                DrawNode(g, node);
        }

        public void DrawNode(Graphics g, Node node)
        {
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

        #region Coordinate Conversions

        // Convert screen coordinates to world coordinates
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
            // Convert mouse position to world coordinates before scaling
            PointF worldPoint = ScreenToWorld(e.Location);

            // Calculate new scale
            float oldScale = _scale;
            float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            _scale *= zoomFactor;
            _scale = Math.Max(0.01f, Math.Min(10f, _scale));

            // Calculate how much the world point moved due to scaling
            float scaleRatio = _scale / oldScale;

            // Update view center to keep the mouse point fixed during zoom
            _viewCenter.X = worldPoint.X + (_viewCenter.X - worldPoint.X) / scaleRatio;
            _viewCenter.Y = worldPoint.Y + (_viewCenter.Y - worldPoint.Y) / scaleRatio;

            // Update offset based on new view center and scale
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
                // Calculate the delta in screen space
                float dx = e.X - _lastMousePosition.X;
                float dy = e.Y - _lastMousePosition.Y;

                // Convert screen delta to world delta
                float worldDx = dx / _scale;
                float worldDy = -dy / _scale;

                // Update the view center
                _viewCenter.X -= worldDx;
                _viewCenter.Y -= worldDy;

                // Update the offset based on the new view center
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
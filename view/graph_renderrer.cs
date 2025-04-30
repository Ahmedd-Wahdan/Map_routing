using System;
using System.Collections.Generic;
using System.Drawing;
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

        public graph_renderrer(Graph graph, Panel panel)
        {
            _graph = graph;
            _panel = panel;

            HookEvents();
        }

        #region Public API

        public void Redraw() => _panel.Invalidate();

        #endregion

        private void ClampOffset()
        {
            // Example: limit pan to within -1000 to +1000 range
            _offset.X = Math.Max(-1000, Math.Min(1000, _offset.X));
            _offset.Y = Math.Max(-1000, Math.Min(1000, _offset.Y));
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            // Set transform: bottom-left origin and scale
            g.TranslateTransform(_offset.X, _panel.Height - _offset.Y);
            g.ScaleTransform(_scale, -_scale);

            DrawBoundingBox(g);
            DrawGraph(g);
        }

        #endregion

        #region Drawing Methods

        private void DrawGraph(Graphics g)
        {

            foreach (var node in _graph.Nodes.Values)
                DrawNode(g, node);

            foreach (var edge in _graph.Edges)
                DrawEdge(g, edge);


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
            using var font = new Font("Arial", 8);

            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
            //g.DrawString(node.Id.ToString(), font, Brushes.Black, node.X + radius, node.Y + radius);
        }

        public void DrawEdge(Graphics g, Edge edge)
        {
            if (!_graph.Nodes.TryGetValue(edge.FromId, out var from) ||
                !_graph.Nodes.TryGetValue(edge.ToId, out var to))
                return;

            using var pen = new Pen(edge.IsPath ? Color.Red : edge.Color, edge.IsPath ? 3f : 1f);
            g.DrawLine(pen, from.X, from.Y, to.X, to.Y);
        }

        #endregion



        private void DrawBoundingBox(Graphics g)
        {
            if (_graph.Nodes.Count == 0) return;

            float minX = _graph.Nodes.Values.Min(n => n.X);
            float maxX = _graph.Nodes.Values.Max(n => n.X);
            float minY = _graph.Nodes.Values.Min(n => n.Y);
            float maxY = _graph.Nodes.Values.Max(n => n.Y);

            var rect = new RectangleF(minX, minY, maxX - minX, maxY - minY);
            using var pen = new Pen(Color.LightGray, 5f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }


        #region Mouse Interaction

    private void OnMouseWheel(object sender, MouseEventArgs e)
    {
        float oldScale = _scale;

        // Define the zoom factor for scroll up/down
        float zoomFactor = (e.Delta > 0) ? 1.1f : 0.9f;
    
        // Apply the zoom factor
        _scale *= zoomFactor;

        // Allow more zoom-out by lowering the minimum scale (e.g., 0.01 instead of 0.1)
        _scale = Math.Max(0.01f, Math.Min(10f, _scale));  // You can adjust 0.01f as needed

        // Convert screen (panel) coordinates to world coordinates before zoom
        float mouseX = (e.X - _offset.X) / oldScale;
        float mouseY = (_panel.Height - e.Y - _offset.Y) / oldScale;

        // Update the offset to keep the point under the mouse fixed in world coordinates
        _offset.X = e.X - mouseX * _scale;
        _offset.Y = _panel.Height - e.Y - mouseY * _scale;

        // Ensure the offset is clamped within the allowed bounds
        ClampOffset();

        // Redraw the panel to reflect the new zoom and offset
        Redraw();
    }



        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPanning = true;
                _lastMousePosition = e.Location;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                float dx = e.X - _lastMousePosition.X;
                float dy = e.Y - _lastMousePosition.Y;

                _offset.X += dx;
                _offset.Y -= dy; // Y is flipped

                ClampOffset();
                _lastMousePosition = e.Location;
                Redraw();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _isPanning = false;
        }

        #endregion
    }
}

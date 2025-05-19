using MAP_routing.view;
using MAP_routing.model;

namespace MAP_routing
{
    public partial class map_vis : Form
    {
        private graph_renderer _graphRenderer;
        private List<Node> graph;
        private List<PathResult> path_results;

        private int _currentQueryIndex = -1;
        private int _currentEdgeIndex = -1;

        public map_vis(List<Node> _graph, List<PathResult> _path_res)
        {
            InitializeComponent();
            graph = _graph;
            path_results = _path_res;

            EnableDoubleBuffering(panel1);

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MAP VISUALIZATION";
            this.Icon = SystemIcons.Application;

            _graphRenderer = new graph_renderer(graph, panel1);

            InitializeQueryControls();
            InitializeEdgeControls();
        }

        public PathResult GetCurrentPathResult()
        {
            if (_currentQueryIndex >= 0 && _currentQueryIndex < path_results.Count)
            {
                return path_results[_currentQueryIndex];
            }
            return null;
        }

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void InitializeQueryControls()
        {
            btnPrevQuery.Enabled = false;
            btnSaveAllResults.Enabled = path_results != null && path_results.Count > 0;

            if (path_results != null && path_results.Count > 0)
            {
                lblQueryCount.Text = $"Queries found: {path_results.Count}";
                _currentQueryIndex = 0;
                btnNextQuery.Enabled = path_results.Count > 1;
                DisplayCurrentQuery();
            }
            else
            {
                lblQueryCount.Text = "Queries found: 0";
                btnNextQuery.Enabled = false;
            }
        }

        private void InitializeEdgeControls()
        {
            btnPrevEdge.Enabled = false;
            lblCurrentEdgeTitle.Text = "Current Edge: -";

            if (path_results != null && path_results.Count > 0)
            {
                btnNextEdge.Enabled = true;
            }

        }

        private void DisplayCurrentQuery()
        {
            if (_currentQueryIndex >= 0 && _currentQueryIndex < path_results.Count)
            {
                PathResult path = path_results[_currentQueryIndex];
                lblCurrentQuery.Text = $"Current Query: {_currentQueryIndex + 1}";

                lblQueryInfo.Text = $"Start: ({path.source.X:F2}, {path.source.Y:F2})\n" +
                                    $"End: ({path.dest.X:F2}, {path.dest.Y:F2})";

                btnPrevQuery.Enabled = _currentQueryIndex > 0;
                btnNextQuery.Enabled = _currentQueryIndex < path_results.Count - 1;

                if (path != null)
                {
                    lblDistance.Text = $"Minimum distance: {path.TotalDistanceKm:F2} km";
                    lblPathMetrics.Text = $"Total time: {path.TotalTimeMin:F2} mins\n" +
                                         $"Total distance: {path.TotalDistanceKm:F2} km\n" +
                                         $"Walking: {path.WalkingDistanceKm:F2} km\n" +
                                         $"Vehicle: {path.VehicleDistanceKm:F2} km\n";

                    if (path.Path != null && path.Path.Count > 0)
                    {
                        HighlightQueryPath(path);

                        _currentEdgeIndex = -1;
                        UpdateEdgeNavigation();
                    }
                    else
                    {
                        HighlightQueryPoints(path);
                    }
                }
                else
                {
                    lblDistance.Text = "Minimum distance: N/A";
                    lblPathMetrics.Text = "";
                    HighlightQueryPoints(path);
                }
            }
            else
            {
                lblCurrentQuery.Text = "Current Query: -";
                lblQueryInfo.Text = "No query selected";
                lblDistance.Text = "Minimum distance: N/A";
                lblPathMetrics.Text = "";
                btnPrevQuery.Enabled = false;
                btnNextQuery.Enabled = false;
            }

            _graphRenderer.Redraw();
        }

        private void HighlightQueryPoints(PathResult query)
        {
            if (query == null) return;

            _graphRenderer.ClearHighlightedNodes();
            _graphRenderer.ClearHighlightedPath();

            _graphRenderer.Redraw();
        }

        private void HighlightQueryPath(PathResult path)
        {
            if (path == null) return;

            _graphRenderer.ClearHighlightedNodes();
            _graphRenderer.ClearHighlightedPath();

            if (path != null && path.Edges.Count > 0)
            {
                _graphRenderer.HighlightPath(path, Color.Blue);

                btnNextEdge.Enabled = path.Edges.Count > 0;
            }

            _graphRenderer.Redraw();
        }

        private void UpdateEdgeNavigation()
        {
            PathResult currentPath = GetCurrentPathResult();

            if (currentPath == null || currentPath.Edges == null || currentPath.Edges.Count == 0)
            {
                btnPrevEdge.Enabled = false;
                btnNextEdge.Enabled = false;
                lblCurrentEdgeTitle.Text = "Current Edge: -";
                return;
            }

            int effectiveEdgeCount = currentPath.Edges.Count;
            bool hasWalkingEdge = currentPath.source != null && currentPath.source.Id == -1 && currentPath.Edges.Count > 0;
            if (hasWalkingEdge)
            {
                effectiveEdgeCount--;
            }

            if (_currentEdgeIndex == -1)
            {
                btnPrevEdge.Enabled = false;
                btnNextEdge.Enabled = effectiveEdgeCount > 0;
                lblCurrentEdgeTitle.Text = $"Current Edge: -/{effectiveEdgeCount}";
                return;
            }

            btnPrevEdge.Enabled = true;
            btnNextEdge.Enabled = true;

            int actualEdgeIndex = hasWalkingEdge ? _currentEdgeIndex + 1 : _currentEdgeIndex;
            int displayIndex = _currentEdgeIndex + 1;

            if (actualEdgeIndex >= 0 && actualEdgeIndex < currentPath.Edges.Count)
            {
                lblCurrentEdgeTitle.Text = $"Current Edge: {displayIndex}/{effectiveEdgeCount}";

                Edge currentEdge = currentPath.Edges[actualEdgeIndex];
                _graphRenderer.HighlightEdge(currentEdge, Color.Green, currentEdge.LengthKm.ToString("F2") + " km");
                _graphRenderer.Redraw();
            }
        }

        private void btnPrevQuery_Click(object sender, EventArgs e)
        {
            if (_currentQueryIndex > 0)
            {
                _currentQueryIndex--;
                DisplayCurrentQuery();
            }
        }

        private void btnNextQuery_Click(object sender, EventArgs e)
        {
            if (_currentQueryIndex < path_results.Count - 1)
            {
                _currentQueryIndex++;
                DisplayCurrentQuery();
            }
        }

        private void btnPrevEdge_Click(object sender, EventArgs e)
        {
            PathResult currentPath = GetCurrentPathResult();
            if (currentPath != null && currentPath.Edges != null && _currentEdgeIndex > -1)
            {
                _currentEdgeIndex--;
                UpdateEdgeNavigation();

                if (_currentEdgeIndex == -1)
                {
                    lblCurrentEdgeTitle.Text = $"Current Edge: -/{currentPath.Edges.Count}";

                    HighlightQueryPath(currentPath);
                    _graphRenderer.Redraw();
                }
            }
        }

        private void btnNextEdge_Click(object sender, EventArgs e)
        {
            PathResult currentPath = GetCurrentPathResult();
            if (currentPath != null && currentPath.Edges != null && currentPath.Edges.Count > 0)
            {
                int effectiveEdgeCount = currentPath.Edges.Count;
                bool hasWalkingEdge = currentPath.source != null && currentPath.source.Id == -1 && currentPath.Edges.Count > 0;
                if (hasWalkingEdge)
                {
                    effectiveEdgeCount--;
                }

                if (_currentEdgeIndex == -1)
                {
                    _currentEdgeIndex = 0;
                }
                else if (_currentEdgeIndex >= effectiveEdgeCount - 1)
                {
                    _currentEdgeIndex = 0;
                }
                else
                {
                    _currentEdgeIndex++;
                }

                UpdateEdgeNavigation();
            }
        }

        private void btnSaveAllResults_Click(object sender, EventArgs e)
        {
            SaveAllResults();
        }

        private void SaveAllResults()
        {
            if (path_results == null || path_results.Count == 0)
            {
                MessageBox.Show("No results to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (.txt)|.txt|All Files (.)|.";
                saveFileDialog.Title = "Save Query Results";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            foreach (var result in path_results)
                            {
                                if (result != null && result.Path.Count > 0)
                                {
                                    for (int i = 0; i < result.Path.Count; i++)
                                    {
                                        writer.Write(result.Path[i]);
                                        if (i < result.Path.Count - 1)
                                            writer.Write(" ");
                                    }
                                    writer.WriteLine();
                                }
                                else
                                {
                                    writer.WriteLine("No path found");
                                }

                                writer.WriteLine($"{result.TotalTimeMin:F2} mins");
                                writer.WriteLine($"{result.TotalDistanceKm:F2} km");
                                writer.WriteLine($"{result.WalkingDistanceKm:F2} km");
                                writer.WriteLine($"{result.VehicleDistanceKm:F2} km");

                                writer.WriteLine();
                            }
                            double total_time = Algorithm.TotalTimeWithIO + MapRouting.total_time_of_IO;

                            writer.WriteLine($"{(int)Algorithm.TotalTimeWithIO} ms");
                            writer.WriteLine();
                            writer.WriteLine($"{(int)total_time} ms");
                        }
                        Algorithm.TotalTimeWithIO = MapRouting.total_time_of_IO = 0;
                        MessageBox.Show("Results saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving results: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


    }
}
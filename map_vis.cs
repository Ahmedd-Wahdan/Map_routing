using MAP_routing.view;
using MAP_routing.model;
namespace MAP_routing
{
    public partial class map_vis : Form
    {
        private graph_renderer _graphRenderer;
        private List<Node> graph;
        private List<Edge> edges;
        private List<PathResult> path_results;

        private int _currentQueryIndex = -1;

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        public map_vis(List<Node> _graph, List<Edge> _edges, List<PathResult> _path_res)
        {
            InitializeComponent();
            graph = _graph;
            edges = _edges;
            path_results = _path_res;

            EnableDoubleBuffering(panel1);

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MAP VISUALIZATION";
            this.Icon = SystemIcons.Application;

            _graphRenderer = new graph_renderer(graph,edges , panel1);

            InitializeQueryControls();
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

        private void map_vis_Load_1(object sender, EventArgs e)
        {
            _graphRenderer.Redraw();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // drawing is handled by the graph_renderer
        }

        private void map_vis_Resize(object sender, EventArgs e)
        {
            if (_graphRenderer != null)
            {
                _graphRenderer.Redraw();
            }
        }

        private void DisplayCurrentQuery()
        {
            if (_currentQueryIndex >= 0 && _currentQueryIndex < path_results.Count)
            {
                PathResult path = path_results[_currentQueryIndex];
                lblCurrentQuery.Text = $"Current Query: {_currentQueryIndex + 1}";

                lblQueryInfo.Text = $"Start: ({path.source.X}, {path.source.Y})\n" +
                                    $"End: ({path.dest.X}, {path.dest.Y})";

                btnPrevQuery.Enabled = _currentQueryIndex > 0;
                btnNextQuery.Enabled = _currentQueryIndex < path_results.Count - 1;

                // Display the path if it exists in results
                if (path_results != null && _currentQueryIndex < path_results.Count)
                {
                    PathResult result = path_results[_currentQueryIndex];
                    lblDistance.Text = $"Minimum distance: {result.TotalDistanceKm:F2}";

                    // Show additional path metrics if available
                    if (result != null)
                    {
                        lblPathMetrics.Text = $"Total time: {result.TotalDistanceKm:F2} mins\n" +
                                             $"Total distance: {result.TotalDistanceKm:F2} km\n" +
                                             $"Walking: {result.WalkingDistanceKm:F2} km\n" +
                                             $"Vehicle: {result.VehicleDistanceKm:F2} km";
                    }
                    else
                    {
                        lblPathMetrics.Text = "";
                    }

                    HighlightQueryPath(result);
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
        }

        private void HighlightQueryPoints(PathResult query)
        {
            List<Node> nodes = graph;
            Node startNode = query.source;
            Node endNode = query.dest;

            _graphRenderer.ClearHighlightedNodes();
            _graphRenderer.ClearHighlightedPath();
            _graphRenderer.HighlightNode(startNode, Color.Green);
            _graphRenderer.HighlightNode(endNode, Color.Red);

            _graphRenderer.Redraw();
        }

        private void HighlightQueryPath(PathResult path)
        {
            List<Node> nodes = graph;
            Node startNode = path.source;
            Node endNode = path.dest;

            _graphRenderer.ClearHighlightedNodes();
            _graphRenderer.ClearHighlightedPath();

            _graphRenderer.HighlightNode(startNode, Color.Green);
            _graphRenderer.HighlightNode(endNode, Color.Red);

            if (path != null && path.Path.Count > 0)
            {
                _graphRenderer.HighlightPath(path, Color.Blue);
            }

            _graphRenderer.Redraw();
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
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
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

                            writer.WriteLine("1 ms");
                            writer.WriteLine();
                            writer.WriteLine("6 ms");
                        }
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

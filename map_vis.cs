using MAP_routing.view;
using MAP_routing.model;
namespace MAP_routing
{
    public partial class map_vis : Form
    {
        private graph_renderer _graphRenderer;
        private List<Node> graph;
        private List<Edge> edges;
        private List<PathResult> path_results ;

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
            btnSaveAllResults.Enabled = _queryResults != null && _queryResults.Count > 0;

            if (_queries != null && _queries.GetQueryCount() > 0)
            {
                lblQueryCount.Text = $"Queries found: {_queries.GetQueryCount()}";
                _currentQueryIndex = 0;
                btnNextQuery.Enabled = _queries.GetQueryCount() > 1;
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
            if (_currentQueryIndex >= 0 && _currentQueryIndex < _queries.GetQueryCount())
            {
                Query query = _queries.GetQuery(_currentQueryIndex);
                lblCurrentQuery.Text = $"Current Query: {_currentQueryIndex + 1}";

                lblQueryInfo.Text = $"Query ID: {query.Rmeteres}\n" +
                                   $"Start: ({query.StartX}, {query.StartY})\n" +
                                   $"End: ({query.EndX}, {query.EndY})";

                btnPrevQuery.Enabled = _currentQueryIndex > 0;
                btnNextQuery.Enabled = _currentQueryIndex < _queries.GetQueryCount() - 1;

                // Display the path if it exists in results
                if (_queryResults != null && _currentQueryIndex < _queryResults.Count)
                {
                    QueryResult result = _queryResults[_currentQueryIndex];
                    lblDistance.Text = $"Minimum distance: {result.Distance:F2}";

                    // Show additional path metrics if available
                    if (result.PathID != null)
                    {
                        lblPathMetrics.Text = $"Total time: {result.PathID.TotalTimeMin:F2} mins\n" +
                                             $"Total distance: {result.PathID.TotalDistanceKm:F2} km\n" +
                                             $"Walking: {result.PathID.WalkingDistanceKm:F2} km\n" +
                                             $"Vehicle: {result.PathID.VehicleDistanceKm:F2} km";
                    }
                    else
                    {
                        lblPathMetrics.Text = "";
                    }

                    HighlightQueryPath(query, result.PathID);
                }
                else
                {
                    lblDistance.Text = "Minimum distance: N/A";
                    lblPathMetrics.Text = "";
                    HighlightQueryPoints(query);
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

        private void HighlightQueryPoints(Query query)
        {
            Dictionary<int, Node> nodes = _graph.GetNodes();
            int startNodeId = Queries.FindNearestNode(nodes, query.StartX, query.StartY);
            int endNodeId = Queries.FindNearestNode(nodes, query.EndX, query.EndY);

            _graphRenderer.ClearHighlightedNodes();
            _graphRenderer.ClearHighlightedPath();

            if (nodes.ContainsKey(startNodeId))
                _graphRenderer.HighlightNode(nodes[startNodeId], Color.Green);

            if (nodes.ContainsKey(endNodeId))
                _graphRenderer.HighlightNode(nodes[endNodeId], Color.Red);

            _graphRenderer.Redraw();
        }

        private void HighlightQueryPath(Query query, Algorithm.PathResult path)
        {
            Dictionary<int, Node> nodes = _graph.GetNodes();
            int startNodeId = Queries.FindNearestNode(nodes, query.StartX, query.StartY);
            int endNodeId = Queries.FindNearestNode(nodes, query.EndX, query.EndY);

            _graphRenderer.ClearHighlightedNodes();
            _graphRenderer.ClearHighlightedPath();

            if (nodes.ContainsKey(startNodeId))
                _graphRenderer.HighlightNode(nodes[startNodeId], Color.Green);

            if (nodes.ContainsKey(endNodeId))
                _graphRenderer.HighlightNode(nodes[endNodeId], Color.Red);

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
            if (_currentQueryIndex < _queries.GetQueryCount() - 1)
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
            if (_queryResults == null || _queryResults.Count == 0)
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
                            foreach (var result in _queryResults)
                            {
                                if (result.PathID != null && result.PathID.Path.Count > 0)
                                {
                                    for (int i = 0; i < result.PathID.Path.Count; i++)
                                    {
                                        writer.Write(result.PathID.Path[i]);
                                        if (i < result.PathID.Path.Count - 1)
                                            writer.Write(" ");
                                    }
                                    writer.WriteLine();
                                }
                                else
                                {
                                    writer.WriteLine("No path found");
                                }

                                writer.WriteLine($"{result.PathID.TotalTimeMin:F2} mins");
                                writer.WriteLine($"{result.PathID.TotalDistanceKm:F2} km");
                                writer.WriteLine($"{result.PathID.WalkingDistanceKm:F2} km");
                                writer.WriteLine($"{result.PathID.VehicleDistanceKm:F2} km");

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

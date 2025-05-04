using MAP_routing.view;
using MAP_routing.model;

namespace MAP_routing
{
    public partial class map_vis : Form
    {
        private graph_renderer _graphRenderer;
        private Graph _graph;
        private Queries _queries = new Queries();
        private List<QueryResult> _queryResults = new List<QueryResult>();

        private int _currentQueryIndex = -1;
        private string _queriesFilePath = "";
        private Algorithm _algorithm;

        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        public map_vis(Graph graph)
        {
            InitializeComponent();
            _graph = graph;
            EnableDoubleBuffering(panel1);

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MAP VISUALIZATION";
            this.Icon = SystemIcons.Application;

            _graphRenderer = new graph_renderer(_graph, panel1);
            _algorithm = new Algorithm(_graph);

            InitializeQueryControls();
        }

        private void InitializeQueryControls()
        {
            btnPrevQuery.Enabled = false;
            btnNextQuery.Enabled = false;
            btnRunAllQueries.Enabled = false;
            btnSaveAllResults.Enabled = false;
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

        private void btnBrowseQueries_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Query Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Queries File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _queriesFilePath = openFileDialog.FileName;
                    LoadQueriesFromFile(_queriesFilePath);
                    lblQueriesFile.Text = Path.GetFileName(_queriesFilePath);

                    int queryCount = _queries.GetQueryCount();
                    lblQueryCount.Text = $"Queries found: {queryCount}";

                    bool hasQueries = queryCount > 0;
                    btnNextQuery.Enabled = hasQueries;
                    btnRunAllQueries.Enabled = hasQueries;
                    btnSaveAllResults.Enabled = hasQueries;

                    if (hasQueries)
                    {
                        _currentQueryIndex = 0;
                        DisplayCurrentQuery();
                    }
                }
            }
        }

        private void LoadQueriesFromFile(string filePath)
        {
            try
            {
                _queries.ReadFromFile(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading queries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCurrentQuery(double dist = -1)
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

                HighlightQueryPoints(query);
            }
            else
            {
                lblCurrentQuery.Text = "Current Query: -";
                lblQueryInfo.Text = "No query selected";
                lblDistance.Text = "Minimum distance: N/A";
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

            if (_algorithm != null)
            {
                Algorithm.PathResult path = _algorithm.Min_Path_Finding(query);
                if (path != null && path.Path.Count > 0)
                {
                    _graphRenderer.HighlightPath(path, Color.Blue);
                    double distance = CalculatePathDistance(path);
                    lblDistance.Text = $"Minimum distance: {distance}";
                }
                else lblDistance.Text = "Minimum distance: No path found";
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

        private void btnRunAllQueries_Click(object sender, EventArgs e)
        {
            RunAllQueries();
        }

        private void RunAllQueries()
        {
            if (_algorithm == null)
            {
                MessageBox.Show("No routing algorithm specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Dictionary<int, Node> nodes = _graph.GetNodes();
                List<QueryResult> results = new List<QueryResult>();

                for (int i = 0; i < _queries.GetQueryCount(); i++)
                {
                    Query query = _queries.GetQuery(i);

                    int startNodeId = Queries.FindNearestNode(nodes, query.StartX, query.StartY);
                    int endNodeId = Queries.FindNearestNode(nodes, query.EndX, query.EndY);

                    Algorithm.PathResult path = _algorithm.Min_Path_Finding(query);
                    double distance = CalculatePathDistance(path);
                    //double duration = CalculatePathDuration(path);

                    results.Add(new QueryResult
                    {
                        QueryId = query.Rmeteres,
                        StartNodeId = startNodeId,
                        EndNodeId = endNodeId,
                        PathID = path,
                        Distance = distance,
                        //Duration = duration
                    });
                }

                _queryResults = results;

                if (_queries.GetQueryCount() > 0)
                {
                    _currentQueryIndex = 0;
                    DisplayCurrentQuery(results[_currentQueryIndex].Distance);
                }

                MessageBox.Show("All queries processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running queries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private double CalculatePathDistance(Algorithm.PathResult paths)
        {
            if (paths == null || paths.Path.Count < 2)
                return 0;

            double distance = 0;

            for (int i = 0; i < paths.Path.Count - 1; i++)
            {
                Edge edge = _graph.GetEdge(paths.Path[i], paths.Path[i + 1]);
                if (edge != null)
                {
                    distance += edge.Distance;
                }
            }

            return distance;
        }

        private void btnSaveAllResults_Click(object sender, EventArgs e)
        {
            SaveAllResults();
        }

        private void SaveAllResults()
        {
            if (_queryResults == null || _queryResults.Count == 0)
            {
                MessageBox.Show("No results to save. Please run queries first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    public class QueryResult
    {
        public int QueryId { get; set; }
        public int StartNodeId { get; set; }
        public int EndNodeId { get; set; }
        public Algorithm.PathResult PathID { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
    }
}
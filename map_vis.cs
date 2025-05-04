using MAP_routing.view;
using MAP_routing.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MAP_routing
{
    public partial class map_vis : Form
    {
        private graph_renderer _graphRenderer;
        private Graph _graph;
        private Queries _queries = new Queries();
        private int _currentQueryIndex = -1;
        private string _queriesFilePath = "";

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

        private void DisplayCurrentQuery()
        {
            if (_currentQueryIndex >= 0 && _currentQueryIndex < _queries.GetQueryCount())
            {
                Query query = _queries.GetQuery(_currentQueryIndex);
                lblCurrentQuery.Text = $"Current Query: {_currentQueryIndex + 1}";

                lblQueryInfo.Text = $"Query ID: {query.QueryId}\n" +
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

            // add algorithm later
            /*if (_routingAlgorithm != null)
            {
                List<Node> path = _routingAlgorithm.FindPath(startNodeId, endNodeId);
                if (path != null && path.Count > 0)
                {
                    _graphRenderer.HighlightPath(path, Color.Blue);
                }
            }*/

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
            /*if (_routingAlgorithm == null)
            {
                MessageBox.Show("No routing algorithm specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

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

                    // Find path
                    //List<Node> path = _routingAlgorithm.FindPath(startNodeId, endNodeId);

                    // Calculate metrics
                    //double distance = CalculatePathDistance(path);
                    //double duration = CalculatePathDuration(path);

                    // Store result
                    results.Add(new QueryResult
                    {
                        QueryId = query.QueryId,
                        StartNodeId = startNodeId,
                        EndNodeId = endNodeId,
                        //Path = path,
                        //Distance = distance,
                        //Duration = duration
                    });
                }

                _queryResults = results;

                if (_queries.GetQueryCount() > 0)
                {
                    _currentQueryIndex = 0;
                    DisplayCurrentQuery();
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

        private List<QueryResult> _queryResults = new List<QueryResult>();

        private double CalculatePathDistance(List<Node> path)
        {
            if (path == null || path.Count < 2)
                return 0;

            double distance = 0;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Edge edge = _graph.GetEdge(path[i].Id, path[i + 1].Id);
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
                            writer.WriteLine($"Total Queries: {_queryResults.Count}");
                            writer.WriteLine("Format: QueryID StartNodeID EndNodeID Distance Duration PathLength");
                            writer.WriteLine();

                            foreach (var result in _queryResults)
                            {
                                writer.WriteLine($"{result.QueryId} {result.StartNodeId} {result.EndNodeId} {result.Distance:F2} {result.Duration:F2} {(result.Path != null ? result.Path.Count : 0)}");

                                if (result.Path != null && result.Path.Count > 0)
                                {
                                    writer.Write("Path: ");
                                    for (int i = 0; i < result.Path.Count; i++)
                                    {
                                        writer.Write(result.Path[i].Id);
                                        if (i < result.Path.Count - 1)
                                            writer.Write(" -> ");
                                    }
                                    writer.WriteLine();
                                }
                                else
                                {
                                    writer.WriteLine("No path found");
                                }

                                writer.WriteLine();
                            }
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
        public List<Node> Path { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
    }
}
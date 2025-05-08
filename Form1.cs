using MAP_routing.model;
using System.IO;

namespace MAP_routing
{
    public partial class Form1 : Form
    {
        private string selectedMapFilePath = string.Empty;
        private string selectedQueriesFilePath = string.Empty;
        private Graph graph = new Graph();
        private Queries queries = new Queries();
        private List<QueryResult> queryResults = new List<QueryResult>();
        private Algorithm algorithm;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MAP Routing Visualizer";
            this.Icon = SystemIcons.Application;
            this.MinimumSize = new Size(800, 500);
        }

        private void btnBrowseMap_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Map File";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedMapFilePath = openFileDialog.FileName;
                    lblMapFilePath.Text = "Selected map: \"" + selectedMapFilePath + "\"";
                    UpdateVisualizeButtonState();
                }
            }
        }

        private void btnBrowseQueries_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Queries File";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedQueriesFilePath = openFileDialog.FileName;
                    lblQueriesFilePath.Text = "Selected queries: \"" + selectedQueriesFilePath + "\"";
                    UpdateVisualizeButtonState();
                }
            }
        }

        private void UpdateVisualizeButtonState()
        {
            btnVisualize.Enabled = !string.IsNullOrEmpty(selectedMapFilePath) && !string.IsNullOrEmpty(selectedQueriesFilePath);
        }

        private void LogOperation(string operation, TimeSpan duration)
        {
            string logMessage = $"{operation}: {duration.TotalMilliseconds:F2} ms";
            lblStatus.Text = logMessage;
            Application.DoEvents();
        }

        private void btnVisualize_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedMapFilePath) || string.IsNullOrEmpty(selectedQueriesFilePath))
            {
                MessageBox.Show("Please select both map and queries files first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(selectedMapFilePath) || !File.Exists(selectedQueriesFilePath))
            {
                MessageBox.Show("One or more of the selected files does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                lblStatus.Text = "Loading map data...";
                lblStatus.Visible = true;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;

                Application.DoEvents();

                graph = new Graph();
                graph.ReadFromFile(selectedMapFilePath);

                lblStatus.Text = "Loading queries data...";
                Application.DoEvents();

                queries = new Queries();
                queries.ReadFromFile(selectedQueriesFilePath);

                algorithm = new Algorithm(graph);

                lblStatus.Text = "Processing queries...";
                Application.DoEvents();

                ProcessAllQueries();

                lblStatus.Text = "Opening visualization...";
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                Application.DoEvents();

                map_vis mapForm = new map_vis(graph, queries, queryResults);
                mapForm.Show();

                lblStatus.Text = "Map visualized successfully!";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error processing data.";
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ProcessAllQueries()
        {
            Dictionary<int, Node> nodes = graph.GetNodes();
            queryResults.Clear();

            int total = queries.GetQueryCount();
            int processed = 0;

            // Process queries in batches to avoid excessive UI updates
            int batchSize = Math.Max(1, total / 20);

            for (int i = 0; i < total; i++)
            {
                Query query = queries.GetQuery(i);
                Algorithm.PathResult path = algorithm.Min_Path_Finding(query);

                // Find node IDs for visualization purposes only
                int startNodeId = path?.Path.FirstOrDefault() ?? Queries.FindNearestNode(nodes, query.StartX, query.StartY);
                int endNodeId = path?.Path.LastOrDefault() ?? Queries.FindNearestNode(nodes, query.EndX, query.EndY);

                double distance = path?.TotalDistanceKm ?? 0;

                queryResults.Add(new QueryResult
                {
                    QueryId = query.Rmeteres,
                    StartNodeId = startNodeId,
                    EndNodeId = endNodeId,
                    PathID = path ?? new Algorithm.PathResult(),
                    Distance = distance
                });

                processed++;

                // Update progress
                if (processed % batchSize == 0 || processed == total)
                {
                    double percent = (double)processed / total * 100;
                    lblStatus.Text = $"Processing queries: {processed}/{total} ({percent:F1}%)";
                    progressBar.Value = (int)percent;
                    Application.DoEvents();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnVisualize.Enabled = false;
            lblStatus.Visible = false;
            progressBar.Visible = false;

            ResizeControls();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        private void ResizeControls()
        {
            int panelWidth = panel2.Width;

            int centerX = panelWidth / 2;
            int controlWidth = 355;
            int leftMargin = centerX - (controlWidth / 2);

            lblMapFilePath.Location = new Point(leftMargin, lblMapFilePath.Location.Y);
            lblMapFilePath.Width = controlWidth;

            btnBrowseMap.Location = new Point(leftMargin, btnBrowseMap.Location.Y);
            btnBrowseMap.Width = controlWidth;

            lblQueriesFilePath.Location = new Point(leftMargin, lblQueriesFilePath.Location.Y);
            lblQueriesFilePath.Width = controlWidth;

            btnBrowseQueries.Location = new Point(leftMargin, btnBrowseQueries.Location.Y);
            btnBrowseQueries.Width = controlWidth;

            label2.Location = new Point(leftMargin, label2.Location.Y);
            label3.Location = new Point(leftMargin, label3.Location.Y);

            lblStatus.Location = new Point(leftMargin, lblStatus.Location.Y);
            progressBar.Location = new Point(leftMargin, progressBar.Location.Y);
            progressBar.Width = controlWidth;

            int btnVisWidth = 158;
            int btnExitWidth = 132;
            int totalBtnWidth = btnVisWidth + btnExitWidth + 10;
            int btnLeftMargin = centerX - (totalBtnWidth / 2);

            btnVisualize.Location = new Point(btnLeftMargin, btnVisualize.Location.Y);
            btnExit.Location = new Point(btnLeftMargin + btnVisWidth + 10, btnExit.Location.Y);

            lblTitle.Left = (panel1.Width - lblTitle.Width) / 2;
        }
    }
}
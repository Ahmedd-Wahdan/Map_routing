using MAP_routing.model;
using System.IO;

namespace MAP_routing
{
    public partial class Form1 : Form
    {
        private string selectedFilePath = string.Empty;

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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Select Map File";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;
                    txtFilePath.Text = selectedFilePath;
                    btnVisualize.Enabled = true;
                }
            }
        }

        private void btnVisualize_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a map file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(selectedFilePath))
            {
                MessageBox.Show("The selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                lblStatus.Text = "Loading map data...";
                lblStatus.Visible = true;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;

                // Update UI before processing
                Application.DoEvents();

                Graph graph = new Graph();
                graph.ReadFromFile(selectedFilePath);

                map_vis mapForm = new map_vis(graph);
                mapForm.Show();

                lblStatus.Text = "Map visualized successfully!";
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading map: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading map data.";
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize with the button disabled until a file is selected
            btnVisualize.Enabled = false;
            lblStatus.Visible = false;
            progressBar.Visible = false;

            // Call resize function to position controls correctly on load
            ResizeControls();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Adjust controls position on resize
            ResizeControls();
        }

        private void ResizeControls()
        {
            // Get the panel width
            int panelWidth = panel2.Width;

            // Calculate center positions
            int centerX = panelWidth / 2;
            int controlWidth = 355;
            int leftMargin = centerX - (controlWidth / 2);

            // Reposition controls
            txtFilePath.Location = new Point(leftMargin, txtFilePath.Location.Y);
            txtFilePath.Width = controlWidth;

            btnBrowse.Location = new Point(leftMargin, btnBrowse.Location.Y);
            btnBrowse.Width = controlWidth;

            label2.Location = new Point(leftMargin, label2.Location.Y);

            // Position status elements
            lblStatus.Location = new Point(leftMargin, lblStatus.Location.Y);
            progressBar.Location = new Point(leftMargin, progressBar.Location.Y);
            progressBar.Width = controlWidth;

            // Calculate button positions
            int btnVisWidth = 158;
            int btnExitWidth = 132;
            int totalBtnWidth = btnVisWidth + btnExitWidth + 10; // 10px spacing
            int btnLeftMargin = centerX - (totalBtnWidth / 2);

            btnVisualize.Location = new Point(btnLeftMargin, btnVisualize.Location.Y);
            btnExit.Location = new Point(btnLeftMargin + btnVisWidth + 10, btnExit.Location.Y);

            // Center the title in the header panel
            lblTitle.Left = (panel1.Width - lblTitle.Width) / 2;
        }
    }
}
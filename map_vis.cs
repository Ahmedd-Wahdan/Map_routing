using MAP_routing.view;
using MAP_routing.model;
namespace MAP_routing
{
    public partial class map_vis : Form
    {
        private graph_renderrer _graph_renderrer;
        private Graph _graph;

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

            // Allow form to be maximized
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MAP VISUALIZATION";
            this.Icon = SystemIcons.Application;

            _graph_renderrer = new graph_renderrer(_graph, panel1);
        }

        private void map_vis_Load_1(object sender, EventArgs e)
        {
            _graph_renderrer.Redraw();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Intentionally left empty - drawing is handled by the graph_renderrer
        }

        private void map_vis_Resize(object sender, EventArgs e)
        {
            // Adjust panel size when the form is resized
            ResizePanel();

            // Redraw graph with new panel dimensions
            if (_graph_renderrer != null)
            {
                _graph_renderrer.Redraw();
            }
        }

        private void ResizePanel()
        {
            // Calculate margins
            int margin = 12;

            // Set panel size to fill the form with margins
            panel1.Location = new Point(margin, margin);
            panel1.Size = new Size(
                this.ClientSize.Width - (margin * 2),
                this.ClientSize.Height - (margin * 2)
            );
        }
    }
}
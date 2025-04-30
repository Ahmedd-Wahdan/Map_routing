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

            _graph_renderrer = new graph_renderrer(_graph, panel1);
        }

        private void map_vis_Load_1(object sender, EventArgs e)
        {

            _graph_renderrer.Redraw();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}

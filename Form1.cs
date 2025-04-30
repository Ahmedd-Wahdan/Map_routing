using MAP_routing.model;

namespace MAP_routing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graph graph = new Graph();

            graph.ReadFromFile("C:\\Users\\2hmed\\Desktop\\OLMap.txt");

            map_vis mapForm = new map_vis(graph);
            mapForm.Show();
        }

    }
}

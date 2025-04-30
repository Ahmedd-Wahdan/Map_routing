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

            graph.ReadFromFile("C:\\Users\\ahmed\\Downloads\\OLMap.txt");

            // Show the form
            map_vis mapForm = new map_vis(graph);
            mapForm.Show();
        }

    }
}

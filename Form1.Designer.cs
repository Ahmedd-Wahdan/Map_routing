namespace MAP_routing
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            lblTitle = new Label();
            panel2 = new Panel();
            label3 = new Label();
            btnBrowseQueries = new Button();
            lblQueriesFilePath = new Label();
            lblMapFilePath = new Label();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            btnExit = new Button();
            btnVisualize = new Button();
            label2 = new Label();
            btnBrowseMap = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.MidnightBlue;
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(784, 100);
            panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.None;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(215, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(374, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "MAP Routing Visualizer";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(label3);
            panel2.Controls.Add(btnBrowseQueries);
            panel2.Controls.Add(lblQueriesFilePath);
            panel2.Controls.Add(lblMapFilePath);
            panel2.Controls.Add(progressBar);
            panel2.Controls.Add(lblStatus);
            panel2.Controls.Add(btnExit);
            panel2.Controls.Add(btnVisualize);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(btnBrowseMap);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 100);
            panel2.Name = "panel2";
            panel2.Size = new Size(784, 461);
            panel2.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(223, 161);
            label3.Name = "label3";
            label3.Size = new Size(174, 21);
            label3.TabIndex = 11;
            label3.Text = "Select Queries File (.txt):";
            // 
            // btnBrowseQueries
            // 
            btnBrowseQueries.BackColor = Color.LightSteelBlue;
            btnBrowseQueries.FlatAppearance.BorderSize = 0;
            btnBrowseQueries.FlatStyle = FlatStyle.Flat;
            btnBrowseQueries.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnBrowseQueries.ForeColor = Color.Navy;
            btnBrowseQueries.Location = new Point(223, 215);
            btnBrowseQueries.Name = "btnBrowseQueries";
            btnBrowseQueries.Size = new Size(355, 34);
            btnBrowseQueries.TabIndex = 10;
            btnBrowseQueries.Text = "Browse Queries...";
            btnBrowseQueries.UseVisualStyleBackColor = false;
            btnBrowseQueries.Click += btnBrowseQueries_Click;
            // 
            // lblQueriesFilePath
            // 
            lblQueriesFilePath.AutoSize = true;
            lblQueriesFilePath.Location = new Point(223, 195);
            lblQueriesFilePath.Name = "lblQueriesFilePath";
            lblQueriesFilePath.Size = new Size(75, 15);
            lblQueriesFilePath.TabIndex = 9;
            lblQueriesFilePath.Text = "Choose a file";
            // 
            // lblMapFilePath
            // 
            lblMapFilePath.AutoSize = true;
            lblMapFilePath.Location = new Point(223, 80);
            lblMapFilePath.Name = "lblMapFilePath";
            lblMapFilePath.Size = new Size(75, 15);
            lblMapFilePath.TabIndex = 7;
            lblMapFilePath.Text = "Choose a file";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(223, 305);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(355, 23);
            progressBar.TabIndex = 6;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.Navy;
            lblStatus.Location = new Point(223, 285);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(98, 15);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Processing map...";
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.IndianRed;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(387, 350);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(132, 37);
            btnExit.TabIndex = 4;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // btnVisualize
            // 
            btnVisualize.BackColor = Color.RoyalBlue;
            btnVisualize.FlatAppearance.BorderSize = 0;
            btnVisualize.FlatStyle = FlatStyle.Flat;
            btnVisualize.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnVisualize.ForeColor = Color.White;
            btnVisualize.Location = new Point(223, 350);
            btnVisualize.Name = "btnVisualize";
            btnVisualize.Size = new Size(158, 37);
            btnVisualize.TabIndex = 3;
            btnVisualize.Text = "Visualize";
            btnVisualize.UseVisualStyleBackColor = false;
            btnVisualize.Click += btnVisualize_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(223, 45);
            label2.Name = "label2";
            label2.Size = new Size(151, 21);
            label2.TabIndex = 2;
            label2.Text = "Select Map File (.txt):";
            // 
            // btnBrowseMap
            // 
            btnBrowseMap.BackColor = Color.LightSteelBlue;
            btnBrowseMap.FlatAppearance.BorderSize = 0;
            btnBrowseMap.FlatStyle = FlatStyle.Flat;
            btnBrowseMap.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnBrowseMap.ForeColor = Color.Navy;
            btnBrowseMap.Location = new Point(223, 98);
            btnBrowseMap.Name = "btnBrowseMap";
            btnBrowseMap.Size = new Size(355, 34);
            btnBrowseMap.TabIndex = 0;
            btnBrowseMap.Text = "Browse Map...";
            btnBrowseMap.UseVisualStyleBackColor = false;
            btnBrowseMap.Click += btnBrowseMap_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MinimumSize = new Size(800, 600);
            Name = "Form1";
            Text = "MAP Routing Visualizer";
            Load += Form1_Load;
            Resize += Form1_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lblTitle;
        private Panel panel2;
        private Button btnBrowseMap;
        private Label label2;
        private Button btnVisualize;
        private Button btnExit;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Label lblMapFilePath;
        private Label lblQueriesFilePath;
        private Button btnBrowseQueries;
        private Label label3;
    }
}
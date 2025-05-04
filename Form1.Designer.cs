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
            lblFilePath = new Label();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            btnExit = new Button();
            btnVisualize = new Button();
            label2 = new Label();
            btnBrowse = new Button();
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
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
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
            panel2.Controls.Add(lblFilePath);
            panel2.Controls.Add(progressBar);
            panel2.Controls.Add(lblStatus);
            panel2.Controls.Add(btnExit);
            panel2.Controls.Add(btnVisualize);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(btnBrowse);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 100);
            panel2.Name = "panel2";
            panel2.Size = new Size(784, 361);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // lblFilePath
            // 
            lblFilePath.AutoSize = true;
            lblFilePath.Location = new Point(223, 80);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(75, 15);
            lblFilePath.TabIndex = 7;
            lblFilePath.Text = "Choose a file";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(223, 248);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(355, 23);
            progressBar.TabIndex = 6;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblStatus.ForeColor = Color.Navy;
            lblStatus.Location = new Point(223, 230);
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
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(387, 295);
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
            btnVisualize.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnVisualize.ForeColor = Color.White;
            btnVisualize.Location = new Point(223, 295);
            btnVisualize.Name = "btnVisualize";
            btnVisualize.Size = new Size(158, 37);
            btnVisualize.TabIndex = 3;
            btnVisualize.Text = "Visualize Map";
            btnVisualize.UseVisualStyleBackColor = false;
            btnVisualize.Click += btnVisualize_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(223, 45);
            label2.Name = "label2";
            label2.Size = new Size(151, 21);
            label2.TabIndex = 2;
            label2.Text = "Select Map File (.txt):";
            // 
            // btnBrowse
            // 
            btnBrowse.BackColor = Color.LightSteelBlue;
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnBrowse.ForeColor = Color.Navy;
            btnBrowse.Location = new Point(223, 120);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(355, 34);
            btnBrowse.TabIndex = 0;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = false;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(panel2);
            Controls.Add(panel1);
            MinimumSize = new Size(800, 500);
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
        private Button btnBrowse;
        private Label label2;
        private Button btnVisualize;
        private Button btnExit;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Label lblFilePath;
    }
}
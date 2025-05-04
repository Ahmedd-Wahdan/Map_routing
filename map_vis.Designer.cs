namespace MAP_routing
{
    partial class map_vis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            splitContainer1 = new SplitContainer();
            pnlQueryControls = new Panel();
            lblQueryInfo = new Label();
            lblCurrentQuery = new Label();
            btnSaveAllResults = new Button();
            btnRunAllQueries = new Button();
            btnNextQuery = new Button();
            btnPrevQuery = new Button();
            lblQueryCount = new Label();
            lblQueriesFile = new Label();
            btnBrowseQueries = new Button();
            lblQuerySectionTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            pnlQueryControls.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(12, 12);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1MinSize = 600;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pnlQueryControls);
            splitContainer1.Panel2MinSize = 250;
            splitContainer1.Size = new Size(1035, 682);
            splitContainer1.SplitterDistance = 750;
            splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(750, 682);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // pnlQueryControls
            // 
            pnlQueryControls.BackColor = Color.AliceBlue;
            pnlQueryControls.BorderStyle = BorderStyle.FixedSingle;
            pnlQueryControls.Controls.Add(lblQueryInfo);
            pnlQueryControls.Controls.Add(lblCurrentQuery);
            pnlQueryControls.Controls.Add(btnSaveAllResults);
            pnlQueryControls.Controls.Add(btnRunAllQueries);
            pnlQueryControls.Controls.Add(btnNextQuery);
            pnlQueryControls.Controls.Add(btnPrevQuery);
            pnlQueryControls.Controls.Add(lblQueryCount);
            pnlQueryControls.Controls.Add(lblQueriesFile);
            pnlQueryControls.Controls.Add(btnBrowseQueries);
            pnlQueryControls.Controls.Add(lblQuerySectionTitle);
            pnlQueryControls.Dock = DockStyle.Fill;
            pnlQueryControls.Location = new Point(0, 0);
            pnlQueryControls.Name = "pnlQueryControls";
            pnlQueryControls.Size = new Size(281, 682);
            pnlQueryControls.TabIndex = 0;
            // 
            // lblQueryInfo
            // 
            lblQueryInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            lblQueryInfo.BackColor = Color.White;
            lblQueryInfo.BorderStyle = BorderStyle.FixedSingle;
            lblQueryInfo.Font = new Font("Segoe UI", 9F);
            lblQueryInfo.Location = new Point(16, 272);
            lblQueryInfo.Name = "lblQueryInfo";
            lblQueryInfo.Padding = new Padding(5);
            lblQueryInfo.Size = new Size(247, 120);
            lblQueryInfo.TabIndex = 10;
            lblQueryInfo.Text = "No query selected";
            // 
            // lblCurrentQuery
            // 
            lblCurrentQuery.AutoSize = true;
            lblCurrentQuery.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblCurrentQuery.ForeColor = Color.Navy;
            lblCurrentQuery.Location = new Point(16, 249);
            lblCurrentQuery.Name = "lblCurrentQuery";
            lblCurrentQuery.Size = new Size(106, 17);
            lblCurrentQuery.TabIndex = 9;
            lblCurrentQuery.Text = "Current Query: -";
            // 
            // btnSaveAllResults
            // 
            btnSaveAllResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            btnSaveAllResults.BackColor = Color.DarkGreen;
            btnSaveAllResults.FlatAppearance.BorderSize = 0;
            btnSaveAllResults.FlatStyle = FlatStyle.Flat;
            btnSaveAllResults.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnSaveAllResults.ForeColor = Color.White;
            btnSaveAllResults.Location = new Point(16, 597);
            btnSaveAllResults.Name = "btnSaveAllResults";
            btnSaveAllResults.Size = new Size(247, 38);
            btnSaveAllResults.TabIndex = 8;
            btnSaveAllResults.Text = "Save All Results";
            btnSaveAllResults.UseVisualStyleBackColor = false;
            btnSaveAllResults.Click += btnSaveAllResults_Click;
            // 
            // btnRunAllQueries
            // 
            btnRunAllQueries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            btnRunAllQueries.BackColor = Color.DarkBlue;
            btnRunAllQueries.FlatAppearance.BorderSize = 0;
            btnRunAllQueries.FlatStyle = FlatStyle.Flat;
            btnRunAllQueries.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnRunAllQueries.ForeColor = Color.White;
            btnRunAllQueries.Location = new Point(16, 543);
            btnRunAllQueries.Name = "btnRunAllQueries";
            btnRunAllQueries.Size = new Size(247, 38);
            btnRunAllQueries.TabIndex = 7;
            btnRunAllQueries.Text = "Run All Queries";
            btnRunAllQueries.UseVisualStyleBackColor = false;
            btnRunAllQueries.Click += btnRunAllQueries_Click;
            // 
            // btnNextQuery
            // 
            btnNextQuery.BackColor = Color.RoyalBlue;
            btnNextQuery.FlatAppearance.BorderSize = 0;
            btnNextQuery.FlatStyle = FlatStyle.Flat;
            btnNextQuery.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnNextQuery.ForeColor = Color.White;
            btnNextQuery.Location = new Point(148, 207);
            btnNextQuery.Name = "btnNextQuery";
            btnNextQuery.Size = new Size(115, 30);
            btnNextQuery.TabIndex = 6;
            btnNextQuery.Text = "Next Query >";
            btnNextQuery.UseVisualStyleBackColor = false;
            btnNextQuery.Click += btnNextQuery_Click;
            // 
            // btnPrevQuery
            // 
            btnPrevQuery.BackColor = Color.RoyalBlue;
            btnPrevQuery.FlatAppearance.BorderSize = 0;
            btnPrevQuery.FlatStyle = FlatStyle.Flat;
            btnPrevQuery.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnPrevQuery.ForeColor = Color.White;
            btnPrevQuery.Location = new Point(16, 207);
            btnPrevQuery.Name = "btnPrevQuery";
            btnPrevQuery.Size = new Size(115, 30);
            btnPrevQuery.TabIndex = 5;
            btnPrevQuery.Text = "< Prev Query";
            btnPrevQuery.UseVisualStyleBackColor = false;
            btnPrevQuery.Click += btnPrevQuery_Click;
            // 
            // lblQueryCount
            // 
            lblQueryCount.AutoSize = true;
            lblQueryCount.Font = new Font("Segoe UI", 9.75F);
            lblQueryCount.Location = new Point(16, 170);
            lblQueryCount.Name = "lblQueryCount";
            lblQueryCount.Size = new Size(95, 17);
            lblQueryCount.TabIndex = 3;
            lblQueryCount.Text = "Queries found: ";
            // 
            // lblQueriesFile
            // 
            lblQueriesFile.AutoSize = true;
            lblQueriesFile.Font = new Font("Segoe UI", 9F);
            lblQueriesFile.Location = new Point(16, 122);
            lblQueriesFile.Name = "lblQueriesFile";
            lblQueriesFile.Size = new Size(96, 15);
            lblQueriesFile.TabIndex = 2;
            lblQueriesFile.Text = "No file selected...";
            // 
            // btnBrowseQueries
            // 
            btnBrowseQueries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            btnBrowseQueries.BackColor = Color.LightSteelBlue;
            btnBrowseQueries.FlatAppearance.BorderSize = 0;
            btnBrowseQueries.FlatStyle = FlatStyle.Flat;
            btnBrowseQueries.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnBrowseQueries.ForeColor = Color.Navy;
            btnBrowseQueries.Location = new Point(16, 78);
            btnBrowseQueries.Name = "btnBrowseQueries";
            btnBrowseQueries.Size = new Size(247, 34);
            btnBrowseQueries.TabIndex = 1;
            btnBrowseQueries.Text = "Browse Queries File...";
            btnBrowseQueries.UseVisualStyleBackColor = false;
            btnBrowseQueries.Click += btnBrowseQueries_Click;
            // 
            // lblQuerySectionTitle
            // 
            lblQuerySectionTitle.AutoSize = true;
            lblQuerySectionTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblQuerySectionTitle.ForeColor = Color.MidnightBlue;
            lblQuerySectionTitle.Location = new Point(16, 24);
            lblQuerySectionTitle.Name = "lblQuerySectionTitle";
            lblQuerySectionTitle.Size = new Size(171, 25);
            lblQuerySectionTitle.TabIndex = 0;
            lblQuerySectionTitle.Text = "Query Navigation";
            // 
            // map_vis
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1059, 706);
            Controls.Add(splitContainer1);
            MinimumSize = new Size(800, 600);
            Name = "map_vis";
            Text = "MAP VISUALIZATION";
            Load += map_vis_Load_1;
            Resize += map_vis_Resize;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            pnlQueryControls.ResumeLayout(false);
            pnlQueryControls.PerformLayout();
            ResumeLayout(false);
        }
        #endregion
        private Panel panel1;
        private SplitContainer splitContainer1;
        private Panel pnlQueryControls;
        private Label lblQuerySectionTitle;
        private Button btnBrowseQueries;
        private Label lblQueriesFile;
        private Label lblQueryCount;
        private Button btnPrevQuery;
        private Button btnNextQuery;
        private Button btnRunAllQueries;
        private Button btnSaveAllResults;
        private Label lblCurrentQuery;
        private Label lblQueryInfo;
    }
}
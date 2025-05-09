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
            lblPathMetrics = new Label();
            lblDistance = new Label();
            lblQueryInfo = new Label();
            lblCurrentQuery = new Label();
            btnSaveAllResults = new Button();
            btnNextQuery = new Button();
            btnPrevQuery = new Button();
            lblQueryCount = new Label();
            lblQuerySectionTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            pnlQueryControls.SuspendLayout();
            SuspendLayout();
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
            // pnlQueryControls
            // 
            pnlQueryControls.BackColor = Color.AliceBlue;
            pnlQueryControls.BorderStyle = BorderStyle.FixedSingle;
            pnlQueryControls.Controls.Add(lblPathMetrics);
            pnlQueryControls.Controls.Add(lblDistance);
            pnlQueryControls.Controls.Add(lblQueryInfo);
            pnlQueryControls.Controls.Add(lblCurrentQuery);
            pnlQueryControls.Controls.Add(btnSaveAllResults);
            pnlQueryControls.Controls.Add(btnNextQuery);
            pnlQueryControls.Controls.Add(btnPrevQuery);
            pnlQueryControls.Controls.Add(lblQueryCount);
            pnlQueryControls.Controls.Add(lblQuerySectionTitle);
            pnlQueryControls.Dock = DockStyle.Fill;
            pnlQueryControls.Location = new Point(0, 0);
            pnlQueryControls.Name = "pnlQueryControls";
            pnlQueryControls.Size = new Size(281, 682);
            pnlQueryControls.TabIndex = 0;
            // 
            // lblPathMetrics
            // 
            lblPathMetrics.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblPathMetrics.BackColor = Color.White;
            lblPathMetrics.BorderStyle = BorderStyle.FixedSingle;
            lblPathMetrics.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblPathMetrics.Location = new Point(16, 446);
            lblPathMetrics.Name = "lblPathMetrics";
            lblPathMetrics.Padding = new Padding(5);
            lblPathMetrics.Size = new Size(247, 80);
            lblPathMetrics.TabIndex = 12;
            // 
            // lblDistance
            // 
            lblDistance.AutoSize = true;
            lblDistance.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblDistance.Location = new Point(16, 311);
            lblDistance.Name = "lblDistance";
            lblDistance.Size = new Size(156, 17);
            lblDistance.TabIndex = 11;
            lblDistance.Text = "Minimum distance: N/A";
            // 
            // lblQueryInfo
            // 
            lblQueryInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblQueryInfo.BackColor = Color.White;
            lblQueryInfo.BorderStyle = BorderStyle.FixedSingle;
            lblQueryInfo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblQueryInfo.Location = new Point(16, 172);
            lblQueryInfo.Name = "lblQueryInfo";
            lblQueryInfo.Padding = new Padding(5);
            lblQueryInfo.Size = new Size(247, 120);
            lblQueryInfo.TabIndex = 10;
            lblQueryInfo.Text = "No query selected";
            // 
            // lblCurrentQuery
            // 
            lblCurrentQuery.AutoSize = true;
            lblCurrentQuery.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblCurrentQuery.ForeColor = Color.Navy;
            lblCurrentQuery.Location = new Point(16, 142);
            lblCurrentQuery.Name = "lblCurrentQuery";
            lblCurrentQuery.Size = new Size(109, 17);
            lblCurrentQuery.TabIndex = 9;
            lblCurrentQuery.Text = "Current Query: -";
            // 
            // btnSaveAllResults
            // 
            btnSaveAllResults.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSaveAllResults.BackColor = Color.DarkGreen;
            btnSaveAllResults.FlatAppearance.BorderSize = 0;
            btnSaveAllResults.FlatStyle = FlatStyle.Flat;
            btnSaveAllResults.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            btnSaveAllResults.ForeColor = Color.White;
            btnSaveAllResults.Location = new Point(16, 597);
            btnSaveAllResults.Name = "btnSaveAllResults";
            btnSaveAllResults.Size = new Size(247, 38);
            btnSaveAllResults.TabIndex = 8;
            btnSaveAllResults.Text = "Save All Results";
            btnSaveAllResults.UseVisualStyleBackColor = false;
            btnSaveAllResults.Click += btnSaveAllResults_Click;
            // 
            // btnNextQuery
            // 
            btnNextQuery.BackColor = Color.RoyalBlue;
            btnNextQuery.FlatAppearance.BorderSize = 0;
            btnNextQuery.FlatStyle = FlatStyle.Flat;
            btnNextQuery.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnNextQuery.ForeColor = Color.White;
            btnNextQuery.Location = new Point(148, 100);
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
            btnPrevQuery.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnPrevQuery.ForeColor = Color.White;
            btnPrevQuery.Location = new Point(16, 100);
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
            lblQueryCount.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            lblQueryCount.Location = new Point(16, 70);
            lblQueryCount.Name = "lblQueryCount";
            lblQueryCount.Size = new Size(98, 17);
            lblQueryCount.TabIndex = 3;
            lblQueryCount.Text = "Queries found: ";
            // 
            // lblQuerySectionTitle
            // 
            lblQuerySectionTitle.AutoSize = true;
            lblQuerySectionTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblQuerySectionTitle.ForeColor = Color.MidnightBlue;
            lblQuerySectionTitle.Location = new Point(16, 24);
            lblQuerySectionTitle.Name = "lblQuerySectionTitle";
            lblQuerySectionTitle.Size = new Size(170, 25);
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
        private Label lblQueryCount;
        private Button btnPrevQuery;
        private Button btnNextQuery;
        private Button btnSaveAllResults;
        private Label lblCurrentQuery;
        private Label lblQueryInfo;
        private Label lblDistance;
        private Label lblPathMetrics;
    }
}
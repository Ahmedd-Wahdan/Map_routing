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
            lblNodes = new Label();
            lblNodesValue = new Label();
            lblEdges = new Label();
            lblEdgesValue = new Label();
            lblProcessingTime = new Label();
            lblProcessingTimeValue = new Label();
            splitContainer1 = new SplitContainer();
            pnlQueryControls = new Panel();
            lblCurrentEdgeTitle = new Label();
            btnPrevEdge = new Button();
            btnNextEdge = new Button();
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
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(857, 909);
            panel1.TabIndex = 0;
            // 
            // lblNodes
            // 
            lblNodes.AutoSize = true;
            lblNodes.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblNodes.Location = new Point(18, 699);
            lblNodes.Name = "lblNodes";
            lblNodes.Size = new Size(70, 23);
            lblNodes.TabIndex = 16;
            lblNodes.Text = "Nodes: ";
            // 
            // lblNodesValue
            // 
            lblNodesValue.AutoSize = true;
            lblNodesValue.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblNodesValue.ForeColor = Color.RoyalBlue;
            lblNodesValue.Location = new Point(83, 699);
            lblNodesValue.Name = "lblNodesValue";
            lblNodesValue.Size = new Size(20, 23);
            lblNodesValue.TabIndex = 19;
            lblNodesValue.Text = "0";
            // 
            // lblEdges
            // 
            lblEdges.AutoSize = true;
            lblEdges.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblEdges.Location = new Point(168, 699);
            lblEdges.Name = "lblEdges";
            lblEdges.Size = new Size(67, 23);
            lblEdges.TabIndex = 17;
            lblEdges.Text = "Edges: ";
            // 
            // lblEdgesValue
            // 
            lblEdgesValue.AutoSize = true;
            lblEdgesValue.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblEdgesValue.ForeColor = Color.RoyalBlue;
            lblEdgesValue.Location = new Point(233, 699);
            lblEdgesValue.Name = "lblEdgesValue";
            lblEdgesValue.Size = new Size(20, 23);
            lblEdgesValue.TabIndex = 20;
            lblEdgesValue.Text = "0";
            // 
            // lblProcessingTime
            // 
            lblProcessingTime.AutoSize = true;
            lblProcessingTime.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblProcessingTime.Location = new Point(18, 652);
            lblProcessingTime.Name = "lblProcessingTime";
            lblProcessingTime.Size = new Size(193, 23);
            lblProcessingTime.TabIndex = 18;
            lblProcessingTime.Text = "Total Processing Time: ";
            // 
            // lblProcessingTimeValue
            // 
            lblProcessingTimeValue.AutoSize = true;
            lblProcessingTimeValue.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblProcessingTimeValue.ForeColor = Color.RoyalBlue;
            lblProcessingTimeValue.Location = new Point(210, 652);
            lblProcessingTimeValue.Name = "lblProcessingTimeValue";
            lblProcessingTimeValue.Size = new Size(48, 23);
            lblProcessingTimeValue.TabIndex = 21;
            lblProcessingTimeValue.Text = "0 ms";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(14, 16);
            splitContainer1.Margin = new Padding(3, 4, 3, 4);
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
            splitContainer1.Size = new Size(1183, 909);
            splitContainer1.SplitterDistance = 857;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // pnlQueryControls
            // 
            pnlQueryControls.BackColor = Color.FromArgb(240, 245, 255);
            pnlQueryControls.BorderStyle = BorderStyle.FixedSingle;
            pnlQueryControls.Controls.Add(lblEdges);
            pnlQueryControls.Controls.Add(lblEdgesValue);
            pnlQueryControls.Controls.Add(lblNodes);
            pnlQueryControls.Controls.Add(lblCurrentEdgeTitle);
            pnlQueryControls.Controls.Add(btnPrevEdge);
            pnlQueryControls.Controls.Add(lblNodesValue);
            pnlQueryControls.Controls.Add(btnNextEdge);
            pnlQueryControls.Controls.Add(lblPathMetrics);
            pnlQueryControls.Controls.Add(lblProcessingTime);
            pnlQueryControls.Controls.Add(lblProcessingTimeValue);
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
            pnlQueryControls.Margin = new Padding(3, 4, 3, 4);
            pnlQueryControls.Name = "pnlQueryControls";
            pnlQueryControls.Size = new Size(321, 909);
            pnlQueryControls.TabIndex = 0;
            // 
            // lblCurrentEdgeTitle
            // 
            lblCurrentEdgeTitle.AutoSize = true;
            lblCurrentEdgeTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblCurrentEdgeTitle.ForeColor = Color.RoyalBlue;
            lblCurrentEdgeTitle.Location = new Point(18, 521);
            lblCurrentEdgeTitle.Name = "lblCurrentEdgeTitle";
            lblCurrentEdgeTitle.Size = new Size(133, 23);
            lblCurrentEdgeTitle.TabIndex = 15;
            lblCurrentEdgeTitle.Text = "Current Edge: -";
            // 
            // btnPrevEdge
            // 
            btnPrevEdge.BackColor = Color.RoyalBlue;
            btnPrevEdge.FlatAppearance.BorderSize = 0;
            btnPrevEdge.FlatStyle = FlatStyle.Flat;
            btnPrevEdge.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnPrevEdge.ForeColor = Color.White;
            btnPrevEdge.Location = new Point(18, 561);
            btnPrevEdge.Margin = new Padding(3, 4, 3, 4);
            btnPrevEdge.Name = "btnPrevEdge";
            btnPrevEdge.Size = new Size(131, 40);
            btnPrevEdge.TabIndex = 14;
            btnPrevEdge.Text = "< Prev Edge";
            btnPrevEdge.UseVisualStyleBackColor = false;
            btnPrevEdge.Click += btnPrevEdge_Click;
            // 
            // btnNextEdge
            // 
            btnNextEdge.BackColor = Color.RoyalBlue;
            btnNextEdge.FlatAppearance.BorderSize = 0;
            btnNextEdge.FlatStyle = FlatStyle.Flat;
            btnNextEdge.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnNextEdge.ForeColor = Color.White;
            btnNextEdge.Location = new Point(169, 561);
            btnNextEdge.Margin = new Padding(3, 4, 3, 4);
            btnNextEdge.Name = "btnNextEdge";
            btnNextEdge.Size = new Size(131, 40);
            btnNextEdge.TabIndex = 13;
            btnNextEdge.Text = "Next Edge >";
            btnNextEdge.UseVisualStyleBackColor = false;
            btnNextEdge.Click += btnNextEdge_Click;
            // 
            // lblPathMetrics
            // 
            lblPathMetrics.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblPathMetrics.BackColor = Color.White;
            lblPathMetrics.BorderStyle = BorderStyle.FixedSingle;
            lblPathMetrics.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblPathMetrics.Location = new Point(18, 405);
            lblPathMetrics.Name = "lblPathMetrics";
            lblPathMetrics.Padding = new Padding(10);
            lblPathMetrics.Size = new Size(282, 106);
            lblPathMetrics.TabIndex = 12;
            // 
            // lblDistance
            // 
            lblDistance.AutoSize = true;
            lblDistance.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblDistance.Location = new Point(18, 360);
            lblDistance.Name = "lblDistance";
            lblDistance.Size = new Size(202, 23);
            lblDistance.TabIndex = 11;
            lblDistance.Text = "Minimum distance: N/A";
            // 
            // lblQueryInfo
            // 
            lblQueryInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblQueryInfo.BackColor = Color.White;
            lblQueryInfo.BorderStyle = BorderStyle.FixedSingle;
            lblQueryInfo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblQueryInfo.Location = new Point(18, 159);
            lblQueryInfo.Name = "lblQueryInfo";
            lblQueryInfo.Padding = new Padding(10);
            lblQueryInfo.Size = new Size(282, 159);
            lblQueryInfo.TabIndex = 10;
            lblQueryInfo.Text = "No query selected";
            // 
            // lblCurrentQuery
            // 
            lblCurrentQuery.AutoSize = true;
            lblCurrentQuery.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblCurrentQuery.ForeColor = Color.RoyalBlue;
            lblCurrentQuery.Location = new Point(18, 119);
            lblCurrentQuery.Name = "lblCurrentQuery";
            lblCurrentQuery.Size = new Size(142, 23);
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
            btnSaveAllResults.Location = new Point(18, 800);
            btnSaveAllResults.Margin = new Padding(3, 4, 3, 4);
            btnSaveAllResults.Name = "btnSaveAllResults";
            btnSaveAllResults.Size = new Size(282, 51);
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
            btnNextQuery.Location = new Point(169, 83);
            btnNextQuery.Margin = new Padding(3, 4, 3, 4);
            btnNextQuery.Name = "btnNextQuery";
            btnNextQuery.Size = new Size(131, 40);
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
            btnPrevQuery.Location = new Point(18, 83);
            btnPrevQuery.Margin = new Padding(3, 4, 3, 4);
            btnPrevQuery.Name = "btnPrevQuery";
            btnPrevQuery.Size = new Size(131, 40);
            btnPrevQuery.TabIndex = 5;
            btnPrevQuery.Text = "< Prev Query";
            btnPrevQuery.UseVisualStyleBackColor = false;
            btnPrevQuery.Click += btnPrevQuery_Click;
            // 
            // lblQueryCount
            // 
            lblQueryCount.AutoSize = true;
            lblQueryCount.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblQueryCount.Location = new Point(18, 43);
            lblQueryCount.Name = "lblQueryCount";
            lblQueryCount.Size = new Size(133, 23);
            lblQueryCount.TabIndex = 3;
            lblQueryCount.Text = "Queries found: ";
            // 
            // lblQuerySectionTitle
            // 
            lblQuerySectionTitle.AutoSize = true;
            lblQuerySectionTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblQuerySectionTitle.ForeColor = Color.FromArgb(50, 50, 80);
            lblQuerySectionTitle.Location = new Point(18, 10);
            lblQuerySectionTitle.Name = "lblQuerySectionTitle";
            lblQuerySectionTitle.Size = new Size(217, 32);
            lblQuerySectionTitle.TabIndex = 0;
            lblQuerySectionTitle.Text = "Query Navigation";
            // 
            // map_vis
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1210, 941);
            Controls.Add(splitContainer1);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(912, 784);
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
        private Button btnNextEdge;
        private Button btnPrevEdge;
        private Label lblCurrentEdgeTitle;
        private Label lblNodes;
        private Label lblEdges;
        private Label lblProcessingTime;
        private Label lblNodesValue;
        private Label lblEdgesValue;
        private Label lblProcessingTimeValue;
    }
}
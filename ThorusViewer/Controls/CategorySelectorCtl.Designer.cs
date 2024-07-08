namespace OPMedia.UI.Controls
{
    partial class CategorySelectorCtl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.rbMax = new System.Windows.Forms.RadioButton();
            this.rbAvg = new System.Windows.Forms.RadioButton();
            this.rbMin = new System.Windows.Forms.RadioButton();
            this.rbRaw = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(469, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select the graphics category you want to view:";
            // 
            // rbMax
            // 
            this.rbMax.AutoSize = true;
            this.rbMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbMax.Location = new System.Drawing.Point(273, 18);
            this.rbMax.Name = "rbMax";
            this.rbMax.Size = new System.Drawing.Size(120, 19);
            this.rbMax.TabIndex = 2;
            this.rbMax.TabStop = true;
            this.rbMax.Tag = "stats/MAX";
            this.rbMax.Text = "Stats - Max values";
            this.rbMax.UseVisualStyleBackColor = true;
            this.rbMax.CheckedChanged += new System.EventHandler(this.OnCategoryChange);
            // 
            // rbAvg
            // 
            this.rbAvg.AutoSize = true;
            this.rbAvg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbAvg.Location = new System.Drawing.Point(3, 18);
            this.rbAvg.Name = "rbAvg";
            this.rbAvg.Size = new System.Drawing.Size(140, 19);
            this.rbAvg.TabIndex = 3;
            this.rbAvg.TabStop = true;
            this.rbAvg.Tag = "stats/AVG";
            this.rbAvg.Text = "Stats - Average values";
            this.rbAvg.UseVisualStyleBackColor = true;
            this.rbAvg.CheckedChanged += new System.EventHandler(this.OnCategoryChange);
            // 
            // rbMin
            // 
            this.rbMin.AutoSize = true;
            this.rbMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbMin.Location = new System.Drawing.Point(149, 18);
            this.rbMin.Name = "rbMin";
            this.rbMin.Size = new System.Drawing.Size(118, 19);
            this.rbMin.TabIndex = 4;
            this.rbMin.TabStop = true;
            this.rbMin.Tag = "stats/MIN";
            this.rbMin.Text = "Stats - Min values";
            this.rbMin.UseVisualStyleBackColor = true;
            this.rbMin.CheckedChanged += new System.EventHandler(this.OnCategoryChange);
            // 
            // rbRaw
            // 
            this.rbRaw.AutoSize = true;
            this.rbRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbRaw.Location = new System.Drawing.Point(399, 18);
            this.rbRaw.Name = "rbRaw";
            this.rbRaw.Size = new System.Drawing.Size(73, 19);
            this.rbRaw.TabIndex = 5;
            this.rbRaw.TabStop = true;
            this.rbRaw.Tag = "";
            this.rbRaw.Text = "Raw data";
            this.rbRaw.UseVisualStyleBackColor = true;
            this.rbRaw.CheckedChanged += new System.EventHandler(this.OnCategoryChange);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbRaw, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbAvg, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbMax, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbMin, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(475, 40);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // CategorySelectorCtl
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(475, 40);
            this.Name = "CategorySelectorCtl";
            this.Size = new System.Drawing.Size(475, 40);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbMax;
        private System.Windows.Forms.RadioButton rbAvg;
        private System.Windows.Forms.RadioButton rbMin;
        private System.Windows.Forms.RadioButton rbRaw;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

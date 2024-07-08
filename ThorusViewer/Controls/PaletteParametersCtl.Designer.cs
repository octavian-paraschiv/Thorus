namespace OPMedia.UI.Controls
{
    partial class PaletteParametersCtl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkShowContours = new System.Windows.Forms.CheckBox();
            this.cmbLineColor = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbLineWidth = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLevelSpacing = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.chkShowContours, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbLineColor, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbLineWidth, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbLevelSpacing, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(480, 45);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkShowContours
            // 
            this.chkShowContours.AutoSize = true;
            this.chkShowContours.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkShowContours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShowContours.Location = new System.Drawing.Point(3, 18);
            this.chkShowContours.Name = "chkShowContours";
            this.chkShowContours.Size = new System.Drawing.Size(91, 23);
            this.chkShowContours.TabIndex = 1;
            this.chkShowContours.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkShowContours.UseVisualStyleBackColor = true;
            this.chkShowContours.CheckedChanged += new System.EventHandler(this.OnParamsChanged);
            // 
            // cmbLineColor
            // 
            this.cmbLineColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbLineColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLineColor.FormattingEnabled = true;
            this.cmbLineColor.Location = new System.Drawing.Point(354, 18);
            this.cmbLineColor.Name = "cmbLineColor";
            this.cmbLineColor.Size = new System.Drawing.Size(121, 23);
            this.cmbLineColor.TabIndex = 4;
            this.cmbLineColor.SelectedIndexChanged += new System.EventHandler(this.OnParamsChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(354, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Line color:";
            // 
            // cmbLineWidth
            // 
            this.cmbLineWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbLineWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLineWidth.FormattingEnabled = true;
            this.cmbLineWidth.Location = new System.Drawing.Point(227, 18);
            this.cmbLineWidth.Name = "cmbLineWidth";
            this.cmbLineWidth.Size = new System.Drawing.Size(121, 23);
            this.cmbLineWidth.TabIndex = 3;
            this.cmbLineWidth.SelectedIndexChanged += new System.EventHandler(this.OnParamsChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Show Contours:";
            // 
            // cmbLevelSpacing
            // 
            this.cmbLevelSpacing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbLevelSpacing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevelSpacing.FormattingEnabled = true;
            this.cmbLevelSpacing.Location = new System.Drawing.Point(100, 18);
            this.cmbLevelSpacing.Name = "cmbLevelSpacing";
            this.cmbLevelSpacing.Size = new System.Drawing.Size(121, 23);
            this.cmbLevelSpacing.TabIndex = 2;
            this.cmbLevelSpacing.SelectedIndexChanged += new System.EventHandler(this.OnParamsChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(227, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Line width:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(100, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Level spacing:";
            // 
            // PaletteParametersCtl
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(480, 45);
            this.Name = "PaletteParametersCtl";
            this.Size = new System.Drawing.Size(480, 45);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbLevelSpacing;
        private System.Windows.Forms.ComboBox cmbLineWidth;
        private System.Windows.Forms.ComboBox cmbLineColor;
        private System.Windows.Forms.CheckBox chkShowContours;
    }
}

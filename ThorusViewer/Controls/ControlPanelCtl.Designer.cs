namespace OPMedia.UI.Controls
{
    partial class ControlPanelCtl
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
            this.categorySelectorCtl1 = new OPMedia.UI.Controls.CategorySelectorCtl();
            this.viewportSelectorCtl1 = new OPMedia.UI.Controls.ViewportSelectorCtl();
            this.dataTypeSelectorCtl1 = new OPMedia.UI.Controls.DataTypeSelectorCtl();
            this.snaphotNavigatorCtl1 = new OPMedia.UI.Controls.SnaphotNavigatorCtl();
            this.paletteParametersCtl1 = new OPMedia.UI.Controls.PaletteParametersCtl();
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.categorySelectorCtl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewportSelectorCtl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataTypeSelectorCtl1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.snaphotNavigatorCtl1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.paletteParametersCtl1, 5, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1378, 85);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // categorySelectorCtl1
            // 
            this.categorySelectorCtl1.AutoSize = true;
            this.categorySelectorCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.categorySelectorCtl1.Category = "";
            this.tableLayoutPanel1.SetColumnSpan(this.categorySelectorCtl1, 6);
            this.categorySelectorCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categorySelectorCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categorySelectorCtl1.Location = new System.Drawing.Point(5, 0);
            this.categorySelectorCtl1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.categorySelectorCtl1.MinimumSize = new System.Drawing.Size(475, 40);
            this.categorySelectorCtl1.Name = "categorySelectorCtl1";
            this.categorySelectorCtl1.Size = new System.Drawing.Size(1368, 40);
            this.categorySelectorCtl1.TabIndex = 0;
            // 
            // viewportSelectorCtl1
            // 
            this.viewportSelectorCtl1.AutoSize = true;
            this.viewportSelectorCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.viewportSelectorCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewportSelectorCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewportSelectorCtl1.Location = new System.Drawing.Point(5, 40);
            this.viewportSelectorCtl1.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.viewportSelectorCtl1.MinimumSize = new System.Drawing.Size(190, 45);
            this.viewportSelectorCtl1.Name = "viewportSelectorCtl1";
            this.viewportSelectorCtl1.Size = new System.Drawing.Size(256, 45);
            this.viewportSelectorCtl1.TabIndex = 1;
            // 
            // dataTypeSelectorCtl1
            // 
            this.dataTypeSelectorCtl1.AutoSize = true;
            this.dataTypeSelectorCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dataTypeSelectorCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTypeSelectorCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTypeSelectorCtl1.Location = new System.Drawing.Point(261, 40);
            this.dataTypeSelectorCtl1.Margin = new System.Windows.Forms.Padding(0);
            this.dataTypeSelectorCtl1.MinimumSize = new System.Drawing.Size(210, 45);
            this.dataTypeSelectorCtl1.Name = "dataTypeSelectorCtl1";
            this.dataTypeSelectorCtl1.Size = new System.Drawing.Size(256, 45);
            this.dataTypeSelectorCtl1.TabIndex = 2;
            // 
            // snaphotNavigatorCtl1
            // 
            this.snaphotNavigatorCtl1.AutoSize = true;
            this.snaphotNavigatorCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.snaphotNavigatorCtl1.Category = "stats/AVG";
            this.snaphotNavigatorCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.snaphotNavigatorCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.snaphotNavigatorCtl1.Location = new System.Drawing.Point(517, 40);
            this.snaphotNavigatorCtl1.Margin = new System.Windows.Forms.Padding(0);
            this.snaphotNavigatorCtl1.MinimumSize = new System.Drawing.Size(305, 45);
            this.snaphotNavigatorCtl1.Name = "snaphotNavigatorCtl1";
            this.snaphotNavigatorCtl1.Size = new System.Drawing.Size(376, 45);
            this.snaphotNavigatorCtl1.TabIndex = 3;
            // 
            // paletteParametersCtl1
            // 
            this.paletteParametersCtl1.AutoSize = true;
            this.paletteParametersCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paletteParametersCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paletteParametersCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paletteParametersCtl1.Location = new System.Drawing.Point(893, 40);
            this.paletteParametersCtl1.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.paletteParametersCtl1.MinimumSize = new System.Drawing.Size(480, 45);
            this.paletteParametersCtl1.Name = "paletteParametersCtl1";
            this.paletteParametersCtl1.Size = new System.Drawing.Size(480, 45);
            this.paletteParametersCtl1.TabIndex = 4;
            // 
            // ControlPanelCtl
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ControlPanelCtl";
            this.Size = new System.Drawing.Size(1378, 85);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CategorySelectorCtl categorySelectorCtl1;
        private ViewportSelectorCtl viewportSelectorCtl1;
        private DataTypeSelectorCtl dataTypeSelectorCtl1;
        private SnaphotNavigatorCtl snaphotNavigatorCtl1;
        private PaletteParametersCtl paletteParametersCtl1;
    }
}

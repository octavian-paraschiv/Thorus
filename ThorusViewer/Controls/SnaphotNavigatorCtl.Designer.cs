namespace OPMedia.UI.Controls
{
    partial class SnaphotNavigatorCtl
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
            this.pnlLayout = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSnapshots = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayout
            // 
            this.pnlLayout.AutoSize = true;
            this.pnlLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlLayout.ColumnCount = 8;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.Controls.Add(this.label1, 0, 0);
            this.pnlLayout.Controls.Add(this.cmbSnapshots, 0, 1);
            this.pnlLayout.Controls.Add(this.button1, 1, 1);
            this.pnlLayout.Controls.Add(this.button2, 2, 1);
            this.pnlLayout.Controls.Add(this.button3, 3, 1);
            this.pnlLayout.Controls.Add(this.button4, 4, 1);
            this.pnlLayout.Controls.Add(this.button5, 5, 1);
            this.pnlLayout.Controls.Add(this.button6, 6, 1);
            this.pnlLayout.Controls.Add(this.btnRefresh, 7, 1);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.RowCount = 3;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.Size = new System.Drawing.Size(305, 45);
            this.pnlLayout.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.pnlLayout.SetColumnSpan(this.label1, 8);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a snapshot to view:";
            // 
            // cmbSnapshots
            // 
            this.cmbSnapshots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSnapshots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSnapshots.FormattingEnabled = true;
            this.cmbSnapshots.Location = new System.Drawing.Point(3, 18);
            this.cmbSnapshots.Name = "cmbSnapshots";
            this.cmbSnapshots.Size = new System.Drawing.Size(110, 23);
            this.cmbSnapshots.TabIndex = 1;
            this.cmbSnapshots.SelectedIndexChanged += new System.EventHandler(this.OnSelectedSnapshotChanged);
            // 
            // button1
            // 
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(119, 18);
            this.button1.MaximumSize = new System.Drawing.Size(21, 21);
            this.button1.MinimumSize = new System.Drawing.Size(21, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 21);
            this.button1.TabIndex = 2;
            this.button1.Tag = "-32768";
            this.button1.Text = "|<";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button_Click);
            // 
            // button2
            // 
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(146, 18);
            this.button2.MaximumSize = new System.Drawing.Size(21, 21);
            this.button2.MinimumSize = new System.Drawing.Size(21, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(21, 21);
            this.button2.TabIndex = 3;
            this.button2.Tag = "-24";
            this.button2.Text = "-24h";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button_Click);
            // 
            // button3
            // 
            this.button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(173, 18);
            this.button3.MaximumSize = new System.Drawing.Size(21, 21);
            this.button3.MinimumSize = new System.Drawing.Size(21, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(21, 21);
            this.button3.TabIndex = 4;
            this.button3.Tag = "-3";
            this.button3.Text = "<<";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button_Click);
            // 
            // button4
            // 
            this.button4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Location = new System.Drawing.Point(200, 18);
            this.button4.MaximumSize = new System.Drawing.Size(21, 21);
            this.button4.MinimumSize = new System.Drawing.Size(21, 21);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(21, 21);
            this.button4.TabIndex = 5;
            this.button4.Tag = "3";
            this.button4.Text = ">>";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button_Click);
            // 
            // button5
            // 
            this.button5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.Location = new System.Drawing.Point(227, 18);
            this.button5.MaximumSize = new System.Drawing.Size(21, 21);
            this.button5.MinimumSize = new System.Drawing.Size(21, 21);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(21, 21);
            this.button5.TabIndex = 6;
            this.button5.Tag = "24";
            this.button5.Text = "+24h";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button_Click);
            // 
            // button6
            // 
            this.button6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button6.Location = new System.Drawing.Point(254, 18);
            this.button6.MaximumSize = new System.Drawing.Size(21, 21);
            this.button6.MinimumSize = new System.Drawing.Size(21, 21);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(21, 21);
            this.button6.TabIndex = 7;
            this.button6.Tag = "32767";
            this.button6.Text = ">|";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Image = global::ThorusViewer.Resources.Reload;
            this.btnRefresh.Location = new System.Drawing.Point(281, 18);
            this.btnRefresh.MaximumSize = new System.Drawing.Size(21, 21);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(21, 21);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(21, 21);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Button_Refresh);
            // 
            // SnaphotNavigatorCtl
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pnlLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(305, 45);
            this.Name = "SnaphotNavigatorCtl";
            this.Size = new System.Drawing.Size(305, 45);
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSnapshots;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnRefresh;
    }
}

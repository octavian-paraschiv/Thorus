namespace ThorusViewer.Forms
{
    partial class DataFetcherDlg
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSimProcOut = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbSST = new System.Windows.Forms.ComboBox();
            this.lblSST = new System.Windows.Forms.Label();
            this.pbGRIB = new System.Windows.Forms.PictureBox();
            this.pbSST = new System.Windows.Forms.PictureBox();
            this.lblGRIB = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnFetchSstData = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGRIB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.txtSimProcOut, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnFetchSstData, 4, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(665, 280);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtSimProcOut
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtSimProcOut, 5);
            this.txtSimProcOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSimProcOut.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSimProcOut.Location = new System.Drawing.Point(3, 70);
            this.txtSimProcOut.Multiline = true;
            this.txtSimProcOut.Name = "txtSimProcOut";
            this.txtSimProcOut.ReadOnly = true;
            this.txtSimProcOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSimProcOut.Size = new System.Drawing.Size(659, 172);
            this.txtSimProcOut.TabIndex = 4;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel6, 4);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.cmbSST, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblSST, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.pbGRIB, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.pbSST, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblGRIB, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 6;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(192, 53);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // cmbSST
            // 
            this.cmbSST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSST.Location = new System.Drawing.Point(68, 3);
            this.cmbSST.Name = "cmbSST";
            this.cmbSST.Size = new System.Drawing.Size(121, 21);
            this.cmbSST.TabIndex = 17;
            // 
            // lblSST
            // 
            this.lblSST.AutoSize = true;
            this.lblSST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSST.Location = new System.Drawing.Point(3, 0);
            this.lblSST.Name = "lblSST";
            this.lblSST.Size = new System.Drawing.Size(33, 27);
            this.lblSST.TabIndex = 0;
            this.lblSST.Text = "SST";
            this.lblSST.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbGRIB
            // 
            this.pbGRIB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGRIB.Location = new System.Drawing.Point(42, 30);
            this.pbGRIB.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbGRIB.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbGRIB.Name = "pbGRIB";
            this.pbGRIB.Size = new System.Drawing.Size(20, 20);
            this.pbGRIB.TabIndex = 16;
            this.pbGRIB.TabStop = false;
            // 
            // pbSST
            // 
            this.pbSST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSST.Location = new System.Drawing.Point(42, 3);
            this.pbSST.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbSST.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbSST.Name = "pbSST";
            this.pbSST.Size = new System.Drawing.Size(20, 20);
            this.pbSST.TabIndex = 10;
            this.pbSST.TabStop = false;
            // 
            // lblGRIB
            // 
            this.lblGRIB.AutoSize = true;
            this.lblGRIB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGRIB.Location = new System.Drawing.Point(3, 27);
            this.lblGRIB.Name = "lblGRIB";
            this.lblGRIB.Size = new System.Drawing.Size(33, 26);
            this.lblGRIB.TabIndex = 8;
            this.lblGRIB.Text = "GRIB";
            this.lblGRIB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnAbort);
            this.flowLayoutPanel1.Controls.Add(this.btnDone);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(500, 248);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(162, 29);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAbort.Location = new System.Drawing.Point(3, 3);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 6;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDone.Location = new System.Drawing.Point(84, 3);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 5;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            // 
            // btnFetchSstData
            // 
            this.btnFetchSstData.AutoSize = true;
            this.btnFetchSstData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFetchSstData.Location = new System.Drawing.Point(201, 3);
            this.btnFetchSstData.Name = "btnFetchSstData";
            this.btnFetchSstData.Size = new System.Drawing.Size(39, 23);
            this.btnFetchSstData.TabIndex = 2;
            this.btnFetchSstData.Text = "Start";
            this.btnFetchSstData.UseVisualStyleBackColor = true;
            this.btnFetchSstData.Click += new System.EventHandler(this.OnFetchSstData);
            // 
            // DataFetcherDlg
            // 
            this.AcceptButton = this.btnDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnAbort;
            this.ClientSize = new System.Drawing.Size(665, 280);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataFetcherDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download initial conditions files (SST file must be present)";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGRIB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnFetchSstData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label lblSST;
        private System.Windows.Forms.PictureBox pbGRIB;
        private System.Windows.Forms.PictureBox pbSST;
        private System.Windows.Forms.Label lblGRIB;
        private System.Windows.Forms.TextBox txtSimProcOut;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.ComboBox cmbSST;
    }
}
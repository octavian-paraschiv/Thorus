namespace ThorusViewer.WinForms
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSstDate = new System.Windows.Forms.ComboBox();
            this.btnFetchSstData = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.pbZZ = new System.Windows.Forms.PictureBox();
            this.pbTT = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pbHH = new System.Windows.Forms.PictureBox();
            this.pbSNOW = new System.Windows.Forms.PictureBox();
            this.pbSST = new System.Windows.Forms.PictureBox();
            this.pbSOIL = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAbort = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSNOW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSOIL)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtSimProcOut, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbSstDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFetchSstData, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 3, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(902, 476);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtSimProcOut
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtSimProcOut, 4);
            this.txtSimProcOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSimProcOut.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSimProcOut.Location = new System.Drawing.Point(3, 124);
            this.txtSimProcOut.Multiline = true;
            this.txtSimProcOut.Name = "txtSimProcOut";
            this.txtSimProcOut.ReadOnly = true;
            this.txtSimProcOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSimProcOut.Size = new System.Drawing.Size(896, 314);
            this.txtSimProcOut.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fetch weather conditions at date:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSstDate
            // 
            this.cmbSstDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSstDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSstDate.FormattingEnabled = true;
            this.cmbSstDate.Location = new System.Drawing.Point(174, 3);
            this.cmbSstDate.Name = "cmbSstDate";
            this.cmbSstDate.Size = new System.Drawing.Size(127, 21);
            this.cmbSstDate.TabIndex = 1;
            // 
            // btnFetchSstData
            // 
            this.btnFetchSstData.AutoSize = true;
            this.btnFetchSstData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFetchSstData.Location = new System.Drawing.Point(307, 3);
            this.btnFetchSstData.Name = "btnFetchSstData";
            this.btnFetchSstData.Size = new System.Drawing.Size(39, 23);
            this.btnFetchSstData.TabIndex = 2;
            this.btnFetchSstData.Text = "Start";
            this.btnFetchSstData.UseVisualStyleBackColor = true;
            this.btnFetchSstData.Click += new System.EventHandler(this.btnFetchSstData_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel6, 3);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.pbZZ, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.pbTT, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.pbHH, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.pbSNOW, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.pbSST, 3, 1);
            this.tableLayoutPanel6.Controls.Add(this.pbSOIL, 3, 2);
            this.tableLayoutPanel6.Controls.Add(this.label10, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.label11, 2, 1);
            this.tableLayoutPanel6.Controls.Add(this.label12, 2, 2);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 6;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(407, 78);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // pbZZ
            // 
            this.pbZZ.Location = new System.Drawing.Point(188, 55);
            this.pbZZ.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbZZ.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbZZ.Name = "pbZZ";
            this.pbZZ.Size = new System.Drawing.Size(20, 20);
            this.pbZZ.TabIndex = 6;
            this.pbZZ.TabStop = false;
            // 
            // pbTT
            // 
            this.pbTT.Location = new System.Drawing.Point(188, 29);
            this.pbTT.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbTT.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbTT.Name = "pbTT";
            this.pbTT.Size = new System.Drawing.Size(20, 20);
            this.pbTT.TabIndex = 5;
            this.pbTT.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 26);
            this.label7.TabIndex = 0;
            this.label7.Text = "Absolute humidities (SPFH_PRES)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(179, 26);
            this.label8.TabIndex = 1;
            this.label8.Text = "Absolute temperatures (TMP_PRES)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(179, 26);
            this.label9.TabIndex = 2;
            this.label9.Text = "Geopotential heights (HGT_PRES)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbHH
            // 
            this.pbHH.Location = new System.Drawing.Point(188, 3);
            this.pbHH.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbHH.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbHH.Name = "pbHH";
            this.pbHH.Size = new System.Drawing.Size(20, 20);
            this.pbHH.TabIndex = 4;
            this.pbHH.TabStop = false;
            // 
            // pbSNOW
            // 
            this.pbSNOW.Location = new System.Drawing.Point(384, 3);
            this.pbSNOW.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbSNOW.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbSNOW.Name = "pbSNOW";
            this.pbSNOW.Size = new System.Drawing.Size(20, 20);
            this.pbSNOW.TabIndex = 7;
            this.pbSNOW.TabStop = false;
            // 
            // pbSST
            // 
            this.pbSST.Location = new System.Drawing.Point(384, 29);
            this.pbSST.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbSST.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbSST.Name = "pbSST";
            this.pbSST.Size = new System.Drawing.Size(20, 20);
            this.pbSST.TabIndex = 10;
            this.pbSST.TabStop = false;
            // 
            // pbSOIL
            // 
            this.pbSOIL.Location = new System.Drawing.Point(384, 55);
            this.pbSOIL.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbSOIL.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbSOIL.Name = "pbSOIL";
            this.pbSOIL.Size = new System.Drawing.Size(20, 20);
            this.pbSOIL.TabIndex = 11;
            this.pbSOIL.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(214, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(164, 26);
            this.label10.TabIndex = 3;
            this.label10.Text = "Snow cover (WEASD_SFC)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(214, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(164, 26);
            this.label11.TabIndex = 8;
            this.label11.Text = "SST (Sea Surface Temperature):";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(214, 52);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(164, 26);
            this.label12.TabIndex = 9;
            this.label12.Text = "Soil Temperature (TMP_BGRND)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnAbort);
            this.flowLayoutPanel1.Controls.Add(this.btnDone);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(737, 444);
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
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // DataFetcherDlg
            // 
            this.AcceptButton = this.btnDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnAbort;
            this.ClientSize = new System.Drawing.Size(902, 476);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataFetcherDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download initial conditions files";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSNOW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSOIL)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSstDate;
        private System.Windows.Forms.Button btnFetchSstData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.PictureBox pbZZ;
        private System.Windows.Forms.PictureBox pbTT;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pbHH;
        private System.Windows.Forms.PictureBox pbSNOW;
        private System.Windows.Forms.PictureBox pbSST;
        private System.Windows.Forms.PictureBox pbSOIL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtSimProcOut;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnAbort;
    }
}
﻿namespace ThorusViewer.WinForms
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
            this.label7 = new System.Windows.Forms.Label();
            this.pbGrib = new System.Windows.Forms.PictureBox();
            this.pbSST = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVID = new System.Windows.Forms.TextBox();
            this.txtDID = new System.Windows.Forms.TextBox();
            this.txtTID = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGrib)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 11;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtSimProcOut, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbSstDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFetchSstData, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 10, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtVID, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDID, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTID, 7, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(663, 497);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtSimProcOut
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtSimProcOut, 11);
            this.txtSimProcOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSimProcOut.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSimProcOut.Location = new System.Drawing.Point(3, 98);
            this.txtSimProcOut.Multiline = true;
            this.txtSimProcOut.Name = "txtSimProcOut";
            this.txtSimProcOut.ReadOnly = true;
            this.txtSimProcOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSimProcOut.Size = new System.Drawing.Size(657, 332);
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
            this.btnFetchSstData.Location = new System.Drawing.Point(616, 3);
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
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel6, 10);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.pbGrib, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.pbSST, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.label11, 0, 1);
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
            this.tableLayoutPanel6.Size = new System.Drawing.Size(652, 52);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 26);
            this.label7.TabIndex = 0;
            this.label7.Text = "GRIB";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbGrib
            // 
            this.pbGrib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGrib.Location = new System.Drawing.Point(42, 3);
            this.pbGrib.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbGrib.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbGrib.Name = "pbGrib";
            this.pbGrib.Size = new System.Drawing.Size(20, 20);
            this.pbGrib.TabIndex = 16;
            this.pbGrib.TabStop = false;
            // 
            // pbSST
            // 
            this.pbSST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSST.Location = new System.Drawing.Point(42, 29);
            this.pbSST.MaximumSize = new System.Drawing.Size(20, 20);
            this.pbSST.MinimumSize = new System.Drawing.Size(20, 20);
            this.pbSST.Name = "pbSST";
            this.pbSST.Size = new System.Drawing.Size(20, 20);
            this.pbSST.TabIndex = 10;
            this.pbSST.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 26);
            this.label11.TabIndex = 8;
            this.label11.Text = "SST";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnAbort);
            this.flowLayoutPanel1.Controls.Add(this.btnDone);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(661, 436);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1, 58);
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
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDone.Location = new System.Drawing.Point(3, 32);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 5;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(307, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "VID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(407, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 29);
            this.label3.TabIndex = 8;
            this.label3.Text = "DID:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(508, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 29);
            this.label4.TabIndex = 9;
            this.label4.Text = "TID:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtVID
            // 
            this.txtVID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVID.Location = new System.Drawing.Point(341, 3);
            this.txtVID.Name = "txtVID";
            this.txtVID.Size = new System.Drawing.Size(60, 20);
            this.txtVID.TabIndex = 10;
            this.txtVID.Text = "1296";
            // 
            // txtDID
            // 
            this.txtDID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDID.Location = new System.Drawing.Point(442, 3);
            this.txtDID.Name = "txtDID";
            this.txtDID.Size = new System.Drawing.Size(60, 20);
            this.txtDID.TabIndex = 11;
            this.txtDID.Text = "62";
            // 
            // txtTID
            // 
            this.txtTID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTID.Location = new System.Drawing.Point(542, 3);
            this.txtTID.Name = "txtTID";
            this.txtTID.Size = new System.Drawing.Size(60, 20);
            this.txtTID.TabIndex = 12;
            this.txtTID.Text = "101060";
            // 
            // DataFetcherDlg
            // 
            this.AcceptButton = this.btnDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnAbort;
            this.ClientSize = new System.Drawing.Size(663, 497);
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
            ((System.ComponentModel.ISupportInitialize)(this.pbGrib)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSstDate;
        private System.Windows.Forms.Button btnFetchSstData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pbGrib;
        private System.Windows.Forms.PictureBox pbSST;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSimProcOut;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtVID;
        private System.Windows.Forms.TextBox txtDID;
        private System.Windows.Forms.TextBox txtTID;
    }
}
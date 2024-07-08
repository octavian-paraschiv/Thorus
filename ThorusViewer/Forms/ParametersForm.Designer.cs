using ThorusViewer.Controls;

namespace ThorusViewer
{
    partial class ParametersForm
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pgProperties = new ThorusViewer.Controls.PropertyGridEx();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmReset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.pgProperties, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(580, 580);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // pgProperties
            // 
            this.pgProperties.CausesValidation = false;
            this.tableLayoutPanel1.SetColumnSpan(this.pgProperties, 3);
            this.pgProperties.ContextMenuStrip = this.cms;
            this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgProperties.Location = new System.Drawing.Point(3, 3);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Size = new System.Drawing.Size(574, 545);
            this.pgProperties.TabIndex = 0;
            this.pgProperties.ToolbarVisible = false;
            this.pgProperties.Click += new System.EventHandler(this.pgProperties_Click);
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmReset});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(188, 26);
            this.cms.Opening += new System.ComponentModel.CancelEventHandler(this.cms_Opening);
            // 
            // tsmReset
            // 
            this.tsmReset.Name = "tsmReset";
            this.tsmReset.Size = new System.Drawing.Size(187, 22);
            this.tsmReset.Text = "Reset to default value";
            this.tsmReset.Click += new System.EventHandler(this.resetToDefaultValueToolStripMenuItem_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(502, 554);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(421, 554);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ParametersForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ParametersForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Simulation Parameters";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGridEx pgProperties;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem tsmReset;
    }
}
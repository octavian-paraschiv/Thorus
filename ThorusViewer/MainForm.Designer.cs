namespace ThorusViewer.Forms
{
    partial class MainForm
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
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLoadDataset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiLaunchSimulation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmImages = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAutoSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSubregionData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGenerateSubregionData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPublishSubregionData = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mapView = new OPMedia.UI.Controls.MapViewCtl();
            this.controlPanelCtl1 = new OPMedia.UI.Controls.ControlPanelCtl();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.msMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.AutoSize = false;
            this.msMain.BackColor = System.Drawing.SystemColors.ControlLight;
            this.msMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmImages,
            this.tsmiSubregionData});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.msMain.Size = new System.Drawing.Size(1289, 27);
            this.msMain.TabIndex = 1;
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLoadDataset,
            this.toolStripSeparator1,
            this.tsmiLaunchSimulation,
            this.toolStripSeparator2,
            this.tsmiSettings});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(37, 23);
            this.tsmiFile.Text = "File";
            // 
            // tsmiLoadDataset
            // 
            this.tsmiLoadDataset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiLoadDataset.Name = "tsmiLoadDataset";
            this.tsmiLoadDataset.ShortcutKeyDisplayString = "";
            this.tsmiLoadDataset.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiLoadDataset.Size = new System.Drawing.Size(287, 22);
            this.tsmiLoadDataset.Text = "Load Dataset...";
            this.tsmiLoadDataset.Click += new System.EventHandler(this.OnLoadDataSet);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(284, 6);
            // 
            // tsmiLaunchSimulation
            // 
            this.tsmiLaunchSimulation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiLaunchSimulation.Name = "tsmiLaunchSimulation";
            this.tsmiLaunchSimulation.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.tsmiLaunchSimulation.Size = new System.Drawing.Size(287, 22);
            this.tsmiLaunchSimulation.Text = "Simulation Control Panel...";
            this.tsmiLaunchSimulation.Click += new System.EventHandler(this.OnSimulation);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmiSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.tsmiSettings.Size = new System.Drawing.Size(287, 22);
            this.tsmiSettings.Text = "Settings...";
            this.tsmiSettings.Click += new System.EventHandler(this.OnGlobalSettings);
            // 
            // tsmImages
            // 
            this.tsmImages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSaveImage,
            this.tsmiAutoSaveImage});
            this.tsmImages.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmImages.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmImages.Name = "tsmImages";
            this.tsmImages.Size = new System.Drawing.Size(57, 23);
            this.tsmImages.Text = "Images";
            this.tsmImages.Click += new System.EventHandler(this.OnToggleAutoSave);
            // 
            // tsmiSaveImage
            // 
            this.tsmiSaveImage.Name = "tsmiSaveImage";
            this.tsmiSaveImage.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.I)));
            this.tsmiSaveImage.Size = new System.Drawing.Size(226, 22);
            this.tsmiSaveImage.Text = "Save as image...";
            this.tsmiSaveImage.Click += new System.EventHandler(this.OnSaveAsImage);
            // 
            // tsmiAutoSaveImage
            // 
            this.tsmiAutoSaveImage.CheckOnClick = true;
            this.tsmiAutoSaveImage.Name = "tsmiAutoSaveImage";
            this.tsmiAutoSaveImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.tsmiAutoSaveImage.Size = new System.Drawing.Size(226, 22);
            this.tsmiAutoSaveImage.Text = "Auto-save images";
            this.tsmiAutoSaveImage.Click += new System.EventHandler(this.OnToggleAutoSave);
            // 
            // tsmiSubregionData
            // 
            this.tsmiSubregionData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGenerateSubregionData,
            this.tsmiPublishSubregionData});
            this.tsmiSubregionData.Name = "tsmiSubregionData";
            this.tsmiSubregionData.Size = new System.Drawing.Size(100, 23);
            this.tsmiSubregionData.Text = "Subregion Data";
            // 
            // tsmiGenerateSubregionData
            // 
            this.tsmiGenerateSubregionData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiGenerateSubregionData.Name = "tsmiGenerateSubregionData";
            this.tsmiGenerateSubregionData.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.tsmiGenerateSubregionData.Size = new System.Drawing.Size(279, 22);
            this.tsmiGenerateSubregionData.Text = "Generate Subregion Data";
            this.tsmiGenerateSubregionData.Click += new System.EventHandler(this.OnGenerateSubregionData);
            // 
            // tsmiPublishSubregionData
            // 
            this.tsmiPublishSubregionData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiPublishSubregionData.Name = "tsmiPublishSubregionData";
            this.tsmiPublishSubregionData.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.tsmiPublishSubregionData.Size = new System.Drawing.Size(279, 22);
            this.tsmiPublishSubregionData.Text = "Publish Subregion Data";
            this.tsmiPublishSubregionData.Click += new System.EventHandler(this.OnPublish);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.mapView, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.controlPanelCtl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1289, 500);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // mapView
            // 
            this.mapView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mapView.BackColor = System.Drawing.Color.White;
            this.mapView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapView.Location = new System.Drawing.Point(0, 107);
            this.mapView.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.mapView.MinimumSize = new System.Drawing.Size(350, 42);
            this.mapView.Name = "mapView";
            this.mapView.Size = new System.Drawing.Size(1289, 390);
            this.mapView.TabIndex = 0;
            // 
            // controlPanelCtl1
            // 
            this.controlPanelCtl1.AutoSize = true;
            this.controlPanelCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.controlPanelCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanelCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlPanelCtl1.Location = new System.Drawing.Point(3, 8);
            this.controlPanelCtl1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.controlPanelCtl1.Name = "controlPanelCtl1";
            this.controlPanelCtl1.Size = new System.Drawing.Size(1283, 85);
            this.controlPanelCtl1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1283, 3);
            this.label1.TabIndex = 2;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(284, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1289, 527);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.msMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.msMain;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thorus Weather Studio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OPMedia.UI.Controls.MapViewCtl mapView;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoadDataset;
        private System.Windows.Forms.ToolStripMenuItem tsmImages;
        private System.Windows.Forms.ToolStripMenuItem tsmiGenerateSubregionData;
        private System.Windows.Forms.ToolStripMenuItem tsmiPublishSubregionData;
        private System.Windows.Forms.ToolStripMenuItem tsmiLaunchSimulation;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private OPMedia.UI.Controls.ControlPanelCtl controlPanelCtl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiAutoSaveImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiSubregionData;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
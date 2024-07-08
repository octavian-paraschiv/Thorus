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
            this.tsmLoadDataset = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAutoSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmGenerateSubregionData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPublishSubregionData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLaunchSimulation = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mapView = new OPMedia.UI.Controls.MapViewCtl();
            this.controlPanelCtl1 = new OPMedia.UI.Controls.ControlPanelCtl();
            this.msMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.AutoSize = false;
            this.msMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmLoadDataset,
            this.tsmSaveImage,
            this.tsmAutoSaveImage,
            this.tsmGenerateSubregionData,
            this.tsmPublishSubregionData,
            this.tsmSettings,
            this.tsmLaunchSimulation});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.msMain.Size = new System.Drawing.Size(1064, 27);
            this.msMain.TabIndex = 1;
            // 
            // tsmLoadDataset
            // 
            this.tsmLoadDataset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmLoadDataset.Name = "tsmLoadDataset";
            this.tsmLoadDataset.Size = new System.Drawing.Size(96, 23);
            this.tsmLoadDataset.Text = "Load Dataset...";
            this.tsmLoadDataset.Click += new System.EventHandler(this.OnLoadDataSet);
            // 
            // tsmSaveImage
            // 
            this.tsmSaveImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmSaveImage.Name = "tsmSaveImage";
            this.tsmSaveImage.Size = new System.Drawing.Size(79, 23);
            this.tsmSaveImage.Text = "Save Image";
            this.tsmSaveImage.Click += new System.EventHandler(this.OnSaveAsImage);
            // 
            // tsmAutoSaveImage
            // 
            this.tsmAutoSaveImage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmAutoSaveImage.Name = "tsmAutoSaveImage";
            this.tsmAutoSaveImage.Size = new System.Drawing.Size(109, 23);
            this.tsmAutoSaveImage.Text = "Auto-save Image";
            this.tsmAutoSaveImage.Click += new System.EventHandler(this.OnToggleAutoSave);
            // 
            // tsmGenerateSubregionData
            // 
            this.tsmGenerateSubregionData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmGenerateSubregionData.Name = "tsmGenerateSubregionData";
            this.tsmGenerateSubregionData.Size = new System.Drawing.Size(150, 23);
            this.tsmGenerateSubregionData.Text = "Generate Subregion Data";
            this.tsmGenerateSubregionData.Click += new System.EventHandler(this.OnGenerateSubregionData);
            // 
            // tsmPublishSubregionData
            // 
            this.tsmPublishSubregionData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmPublishSubregionData.Name = "tsmPublishSubregionData";
            this.tsmPublishSubregionData.Size = new System.Drawing.Size(142, 23);
            this.tsmPublishSubregionData.Text = "Publish Subregion Data";
            this.tsmPublishSubregionData.Click += new System.EventHandler(this.OnPublish);
            // 
            // tsmSettings
            // 
            this.tsmSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmSettings.Name = "tsmSettings";
            this.tsmSettings.Size = new System.Drawing.Size(61, 23);
            this.tsmSettings.Text = "Settings";
            this.tsmSettings.Click += new System.EventHandler(this.OnGlobalSettings);
            // 
            // tsmLaunchSimulation
            // 
            this.tsmLaunchSimulation.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmLaunchSimulation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmLaunchSimulation.Name = "tsmLaunchSimulation";
            this.tsmLaunchSimulation.Size = new System.Drawing.Size(118, 23);
            this.tsmLaunchSimulation.Text = "Launch Simulation";
            this.tsmLaunchSimulation.Click += new System.EventHandler(this.OnSimulation);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.mapView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.controlPanelCtl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1064, 500);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // mapView
            // 
            this.mapView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mapView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapView.Location = new System.Drawing.Point(2, 110);
            this.mapView.Margin = new System.Windows.Forms.Padding(2);
            this.mapView.MinimumSize = new System.Drawing.Size(350, 42);
            this.mapView.Name = "mapView";
            this.mapView.Size = new System.Drawing.Size(1060, 388);
            this.mapView.TabIndex = 0;
            // 
            // controlPanelCtl1
            // 
            this.controlPanelCtl1.AutoSize = true;
            this.controlPanelCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.controlPanelCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanelCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlPanelCtl1.Location = new System.Drawing.Point(3, 3);
            this.controlPanelCtl1.MinimumSize = new System.Drawing.Size(1373, 102);
            this.controlPanelCtl1.Name = "controlPanelCtl1";
            this.controlPanelCtl1.Size = new System.Drawing.Size(1373, 102);
            this.controlPanelCtl1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 527);
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
        private System.Windows.Forms.ToolStripMenuItem tsmLoadDataset;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveImage;
        private System.Windows.Forms.ToolStripMenuItem tsmAutoSaveImage;
        private System.Windows.Forms.ToolStripMenuItem tsmGenerateSubregionData;
        private System.Windows.Forms.ToolStripMenuItem tsmPublishSubregionData;
        private System.Windows.Forms.ToolStripMenuItem tsmLaunchSimulation;
        private System.Windows.Forms.ToolStripMenuItem tsmSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private OPMedia.UI.Controls.ControlPanelCtl controlPanelCtl1;
    }
}
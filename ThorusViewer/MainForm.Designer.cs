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
            msMain = new System.Windows.Forms.MenuStrip();
            tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            tsmiLoadDataset = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiLaunchSimulation = new System.Windows.Forms.ToolStripMenuItem();
            tsmiPublishSubregionData = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            tsmImages = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            tsmiAutoSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            mapView = new OPMedia.UI.Controls.MapViewCtl();
            controlPanelCtl1 = new OPMedia.UI.Controls.ControlPanelCtl();
            label1 = new System.Windows.Forms.Label();
            tsmiGenerateAnimations = new System.Windows.Forms.ToolStripMenuItem();
            msMain.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // msMain
            // 
            msMain.AutoSize = false;
            msMain.BackColor = System.Drawing.SystemColors.ControlLight;
            msMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            msMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile, tsmImages });
            msMain.Location = new System.Drawing.Point(0, 0);
            msMain.Name = "msMain";
            msMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            msMain.Size = new System.Drawing.Size(1289, 27);
            msMain.TabIndex = 1;
            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiLoadDataset, toolStripSeparator1, tsmiLaunchSimulation, tsmiPublishSubregionData, tsmiGenerateAnimations, toolStripSeparator2, tsmiSettings });
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Size = new System.Drawing.Size(46, 23);
            tsmiFile.Text = "File";
            // 
            // tsmiLoadDataset
            // 
            tsmiLoadDataset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsmiLoadDataset.Name = "tsmiLoadDataset";
            tsmiLoadDataset.ShortcutKeyDisplayString = "";
            tsmiLoadDataset.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            tsmiLoadDataset.Size = new System.Drawing.Size(403, 26);
            tsmiLoadDataset.Text = "Load Dataset...";
            tsmiLoadDataset.Click += OnLoadDataSet;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(400, 6);
            // 
            // tsmiLaunchSimulation
            // 
            tsmiLaunchSimulation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsmiLaunchSimulation.Name = "tsmiLaunchSimulation";
            tsmiLaunchSimulation.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            tsmiLaunchSimulation.Size = new System.Drawing.Size(403, 26);
            tsmiLaunchSimulation.Text = "Simulation Control Panel...";
            tsmiLaunchSimulation.Click += OnSimulation;
            // 
            // tsmiPublishSubregionData
            // 
            tsmiPublishSubregionData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsmiPublishSubregionData.Name = "tsmiPublishSubregionData";
            tsmiPublishSubregionData.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.P;
            tsmiPublishSubregionData.Size = new System.Drawing.Size(403, 26);
            tsmiPublishSubregionData.Text = "Generate/Publish Subregion Data";
            tsmiPublishSubregionData.Click += OnPublish;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(400, 6);
            // 
            // tsmiSettings
            // 
            tsmiSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            tsmiSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsmiSettings.Name = "tsmiSettings";
            tsmiSettings.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S;
            tsmiSettings.Size = new System.Drawing.Size(403, 26);
            tsmiSettings.Text = "Settings...";
            tsmiSettings.Click += OnGlobalSettings;
            // 
            // tsmImages
            // 
            tsmImages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiSaveImage, tsmiAutoSaveImage });
            tsmImages.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tsmImages.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            tsmImages.Name = "tsmImages";
            tsmImages.Size = new System.Drawing.Size(71, 23);
            tsmImages.Text = "Images";
            tsmImages.Click += OnToggleAutoSave;
            // 
            // tsmiSaveImage
            // 
            tsmiSaveImage.Name = "tsmiSaveImage";
            tsmiSaveImage.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.I;
            tsmiSaveImage.Size = new System.Drawing.Size(282, 26);
            tsmiSaveImage.Text = "Save as image...";
            tsmiSaveImage.Click += OnSaveAsImage;
            // 
            // tsmiAutoSaveImage
            // 
            tsmiAutoSaveImage.CheckOnClick = true;
            tsmiAutoSaveImage.Name = "tsmiAutoSaveImage";
            tsmiAutoSaveImage.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I;
            tsmiAutoSaveImage.Size = new System.Drawing.Size(282, 26);
            tsmiAutoSaveImage.Text = "Auto-save images";
            tsmiAutoSaveImage.Click += OnToggleAutoSave;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(mapView, 0, 2);
            tableLayoutPanel1.Controls.Add(controlPanelCtl1, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1289, 500);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // mapView
            // 
            mapView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            mapView.BackColor = System.Drawing.Color.White;
            mapView.Dock = System.Windows.Forms.DockStyle.Fill;
            mapView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            mapView.Location = new System.Drawing.Point(0, 126);
            mapView.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            mapView.MinimumSize = new System.Drawing.Size(350, 42);
            mapView.Name = "mapView";
            mapView.Size = new System.Drawing.Size(1289, 371);
            mapView.TabIndex = 0;
            // 
            // controlPanelCtl1
            // 
            controlPanelCtl1.AutoSize = true;
            controlPanelCtl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            controlPanelCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            controlPanelCtl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            controlPanelCtl1.Location = new System.Drawing.Point(3, 8);
            controlPanelCtl1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            controlPanelCtl1.Name = "controlPanelCtl1";
            controlPanelCtl1.Size = new System.Drawing.Size(1283, 104);
            controlPanelCtl1.TabIndex = 1;
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.SystemColors.ControlLight;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Location = new System.Drawing.Point(3, 120);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(1283, 3);
            label1.TabIndex = 2;
            // 
            // tsmiGenerateAnimations
            // 
            tsmiGenerateAnimations.Name = "tsmiGenerateAnimations";
            tsmiGenerateAnimations.Size = new System.Drawing.Size(403, 26);
            tsmiGenerateAnimations.Text = "Generate Animations";
            tsmiGenerateAnimations.Click += tsmiGenerateAnimations_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(1289, 527);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(msMain);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            MainMenuStrip = msMain;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Thorus Weather Studio";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            msMain.ResumeLayout(false);
            msMain.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private OPMedia.UI.Controls.MapViewCtl mapView;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoadDataset;
        private System.Windows.Forms.ToolStripMenuItem tsmImages;
        private System.Windows.Forms.ToolStripMenuItem tsmiPublishSubregionData;
        private System.Windows.Forms.ToolStripMenuItem tsmiLaunchSimulation;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private OPMedia.UI.Controls.ControlPanelCtl controlPanelCtl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiAutoSaveImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiGenerateAnimations;
    }
}
namespace ThorusViewer
{
    partial class SimControlPanel
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            cbAutoClose = new System.Windows.Forms.CheckBox();
            gbSimParams = new System.Windows.Forms.GroupBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            dtpSimStop = new System.Windows.Forms.DateTimePicker();
            label5 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lnkSimParams = new System.Windows.Forms.LinkLabel();
            dtpSimStart = new System.Windows.Forms.DateTimePicker();
            groupBox2 = new System.Windows.Forms.GroupBox();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            cbRunStatsOnly = new System.Windows.Forms.CheckBox();
            cbRunStats = new System.Windows.Forms.CheckBox();
            label6 = new System.Windows.Forms.Label();
            cmbRange = new System.Windows.Forms.ComboBox();
            btnClose = new System.Windows.Forms.Button();
            cbAutoExport = new System.Windows.Forms.CheckBox();
            cmbStepLen = new System.Windows.Forms.ComboBox();
            gbInitialConditions = new System.Windows.Forms.GroupBox();
            tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            groupBox5 = new System.Windows.Forms.GroupBox();
            tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            pbSST = new System.Windows.Forms.PictureBox();
            pbGrib = new System.Windows.Forms.PictureBox();
            label11 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            btnFetchData = new System.Windows.Forms.Button();
            gbSimControl = new System.Windows.Forms.GroupBox();
            txtSimProcOut = new System.Windows.Forms.TextBox();
            pbSimProgress = new System.Windows.Forms.ProgressBar();
            btnSimStart = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            gbSimParams.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            gbInitialConditions.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbSST).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbGrib).BeginInit();
            gbSimControl.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(cbAutoClose, 0, 4);
            tableLayoutPanel1.Controls.Add(gbSimParams, 0, 2);
            tableLayoutPanel1.Controls.Add(gbInitialConditions, 0, 1);
            tableLayoutPanel1.Controls.Add(gbSimControl, 0, 3);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(901, 636);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // cbAutoClose
            // 
            cbAutoClose.AutoSize = true;
            cbAutoClose.Dock = System.Windows.Forms.DockStyle.Fill;
            cbAutoClose.Location = new System.Drawing.Point(4, 614);
            cbAutoClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbAutoClose.Name = "cbAutoClose";
            cbAutoClose.Size = new System.Drawing.Size(893, 19);
            cbAutoClose.TabIndex = 2;
            cbAutoClose.Text = "Automatically close this dialog after the simulation ends";
            cbAutoClose.UseVisualStyleBackColor = true;
            // 
            // gbSimParams
            // 
            gbSimParams.AutoSize = true;
            gbSimParams.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            gbSimParams.Controls.Add(tableLayoutPanel2);
            gbSimParams.Dock = System.Windows.Forms.DockStyle.Fill;
            gbSimParams.Location = new System.Drawing.Point(4, 117);
            gbSimParams.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbSimParams.Name = "gbSimParams";
            gbSimParams.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbSimParams.Size = new System.Drawing.Size(893, 143);
            gbSimParams.TabIndex = 1;
            gbSimParams.TabStop = false;
            gbSimParams.Text = "Simulation Parameters";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(dtpSimStop, 1, 1);
            tableLayoutPanel2.Controls.Add(label5, 0, 3);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(label2, 0, 1);
            tableLayoutPanel2.Controls.Add(label3, 0, 2);
            tableLayoutPanel2.Controls.Add(lnkSimParams, 1, 3);
            tableLayoutPanel2.Controls.Add(dtpSimStart, 1, 0);
            tableLayoutPanel2.Controls.Add(groupBox2, 3, 0);
            tableLayoutPanel2.Controls.Add(cmbStepLen, 1, 2);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel2.Size = new System.Drawing.Size(885, 121);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // dtpSimStop
            // 
            dtpSimStop.CustomFormat = "yyyy-MM-dd_HH";
            dtpSimStop.Dock = System.Windows.Forms.DockStyle.Fill;
            dtpSimStop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpSimStop.Location = new System.Drawing.Point(131, 32);
            dtpSimStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dtpSimStop.Name = "dtpSimStop";
            dtpSimStop.Size = new System.Drawing.Size(141, 23);
            dtpSimStop.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = System.Windows.Forms.DockStyle.Fill;
            label5.Location = new System.Drawing.Point(4, 87);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(119, 34);
            label5.TabIndex = 3;
            label5.Text = "Model params:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Location = new System.Drawing.Point(4, 0);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(119, 29);
            label1.TabIndex = 0;
            label1.Text = "Start Date/Time:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Location = new System.Drawing.Point(4, 29);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(119, 29);
            label2.TabIndex = 1;
            label2.Text = "Stop Date/Time:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.Enabled = false;
            label3.Location = new System.Drawing.Point(4, 58);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(119, 29);
            label3.TabIndex = 2;
            label3.Text = "Sim step length (hrs):";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label3.Visible = false;
            // 
            // lnkSimParams
            // 
            tableLayoutPanel2.SetColumnSpan(lnkSimParams, 2);
            lnkSimParams.Dock = System.Windows.Forms.DockStyle.Fill;
            lnkSimParams.Location = new System.Drawing.Point(131, 87);
            lnkSimParams.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lnkSimParams.Name = "lnkSimParams";
            lnkSimParams.Size = new System.Drawing.Size(164, 34);
            lnkSimParams.TabIndex = 4;
            lnkSimParams.TabStop = true;
            lnkSimParams.Text = "Click to view and edit ...";
            lnkSimParams.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lnkSimParams.LinkClicked += OnLinkClicked;
            // 
            // dtpSimStart
            // 
            dtpSimStart.CustomFormat = "yyyy-MM-dd_HH";
            dtpSimStart.Dock = System.Windows.Forms.DockStyle.Fill;
            dtpSimStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpSimStart.Location = new System.Drawing.Point(131, 3);
            dtpSimStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dtpSimStart.Name = "dtpSimStart";
            dtpSimStart.Size = new System.Drawing.Size(141, 23);
            dtpSimStart.TabIndex = 5;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel4);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox2.Location = new System.Drawing.Point(303, 3);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel2.SetRowSpan(groupBox2, 4);
            groupBox2.Size = new System.Drawing.Size(578, 115);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Post-simulation";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.AutoSize = true;
            tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel4.Controls.Add(cbRunStatsOnly, 0, 2);
            tableLayoutPanel4.Controls.Add(cbRunStats, 0, 1);
            tableLayoutPanel4.Controls.Add(label6, 0, 3);
            tableLayoutPanel4.Controls.Add(cmbRange, 1, 3);
            tableLayoutPanel4.Controls.Add(btnClose, 2, 2);
            tableLayoutPanel4.Controls.Add(cbAutoExport, 0, 0);
            tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel4.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel4.Size = new System.Drawing.Size(570, 93);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // cbRunStatsOnly
            // 
            cbRunStatsOnly.AutoSize = true;
            tableLayoutPanel4.SetColumnSpan(cbRunStatsOnly, 2);
            cbRunStatsOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            cbRunStatsOnly.Location = new System.Drawing.Point(3, 49);
            cbRunStatsOnly.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            cbRunStatsOnly.Name = "cbRunStatsOnly";
            cbRunStatsOnly.Size = new System.Drawing.Size(555, 20);
            cbRunStatsOnly.TabIndex = 7;
            cbRunStatsOnly.Text = "Run statistical analisys only (on existing simulation data)";
            cbRunStatsOnly.UseVisualStyleBackColor = true;
            cbRunStatsOnly.CheckedChanged += OnRunStatsOnlyCheckedChanged;
            // 
            // cbRunStats
            // 
            cbRunStats.AutoSize = true;
            tableLayoutPanel4.SetColumnSpan(cbRunStats, 2);
            cbRunStats.Dock = System.Windows.Forms.DockStyle.Fill;
            cbRunStats.Location = new System.Drawing.Point(3, 26);
            cbRunStats.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            cbRunStats.Name = "cbRunStats";
            cbRunStats.Size = new System.Drawing.Size(555, 20);
            cbRunStats.TabIndex = 0;
            cbRunStats.Text = "Automatically run statistical analisys after the simulation ends";
            cbRunStats.UseVisualStyleBackColor = true;
            cbRunStats.CheckedChanged += OnRunStatsCheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = System.Windows.Forms.DockStyle.Fill;
            label6.Location = new System.Drawing.Point(3, 72);
            label6.Margin = new System.Windows.Forms.Padding(3);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(145, 18);
            label6.TabIndex = 2;
            label6.Text = "Statistic Range Size (days):";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbRange
            // 
            cmbRange.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRange.FormattingEnabled = true;
            cmbRange.Items.AddRange(new object[] { "1", "2", "5", "7", "10", "15", "30", "120", "240", "365" });
            cmbRange.Location = new System.Drawing.Point(151, 69);
            cmbRange.Margin = new System.Windows.Forms.Padding(0);
            cmbRange.Name = "cmbRange";
            cmbRange.Size = new System.Drawing.Size(410, 23);
            cmbRange.TabIndex = 5;
            // 
            // btnClose
            // 
            btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnClose.Location = new System.Drawing.Point(565, 49);
            btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(1, 1);
            btnClose.TabIndex = 8;
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += button1_Click;
            // 
            // cbAutoExport
            // 
            cbAutoExport.AutoSize = true;
            tableLayoutPanel4.SetColumnSpan(cbAutoExport, 3);
            cbAutoExport.Dock = System.Windows.Forms.DockStyle.Fill;
            cbAutoExport.Location = new System.Drawing.Point(3, 3);
            cbAutoExport.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            cbAutoExport.Name = "cbAutoExport";
            cbAutoExport.Size = new System.Drawing.Size(564, 20);
            cbAutoExport.TabIndex = 9;
            cbAutoExport.Text = "Automatically export subregion data after the simulation ends";
            cbAutoExport.Checked = true;
            cbAutoExport.UseVisualStyleBackColor = true;
            // 
            // cmbStepLen
            // 
            cmbStepLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbStepLen.Enabled = false;
            cmbStepLen.FormattingEnabled = true;
            cmbStepLen.Items.AddRange(new object[] { "2", "3", "6", "8", "12", "24" });
            cmbStepLen.Location = new System.Drawing.Point(131, 61);
            cmbStepLen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbStepLen.Name = "cmbStepLen";
            cmbStepLen.Size = new System.Drawing.Size(140, 23);
            cmbStepLen.TabIndex = 11;
            cmbStepLen.Visible = false;
            // 
            // gbInitialConditions
            // 
            gbInitialConditions.Controls.Add(tableLayoutPanel5);
            gbInitialConditions.Location = new System.Drawing.Point(4, 3);
            gbInitialConditions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbInitialConditions.Name = "gbInitialConditions";
            gbInitialConditions.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbInitialConditions.Size = new System.Drawing.Size(893, 108);
            gbInitialConditions.TabIndex = 0;
            gbInitialConditions.TabStop = false;
            gbInitialConditions.Text = "Initial Model Conditions";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.AutoSize = true;
            tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel5.Controls.Add(groupBox5, 0, 0);
            tableLayoutPanel5.Controls.Add(btnFetchData, 1, 0);
            tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel5.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel5.Size = new System.Drawing.Size(885, 86);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // groupBox5
            // 
            groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            groupBox5.Controls.Add(tableLayoutPanel6);
            groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox5.Location = new System.Drawing.Point(4, 3);
            groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel5.SetRowSpan(groupBox5, 2);
            groupBox5.Size = new System.Drawing.Size(172, 80);
            groupBox5.TabIndex = 3;
            groupBox5.TabStop = false;
            groupBox5.Text = "Initial conditions validity";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.AutoSize = true;
            tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel6.ColumnCount = 4;
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel6.Controls.Add(pbSST, 3, 0);
            tableLayoutPanel6.Controls.Add(pbGrib, 3, 1);
            tableLayoutPanel6.Controls.Add(label11, 2, 0);
            tableLayoutPanel6.Controls.Add(label12, 2, 1);
            tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel6.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel6.Size = new System.Drawing.Size(164, 58);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // pbSST
            // 
            pbSST.Location = new System.Drawing.Point(44, 3);
            pbSST.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pbSST.MaximumSize = new System.Drawing.Size(23, 23);
            pbSST.MinimumSize = new System.Drawing.Size(23, 23);
            pbSST.Name = "pbSST";
            pbSST.Size = new System.Drawing.Size(23, 23);
            pbSST.TabIndex = 10;
            pbSST.TabStop = false;
            // 
            // pbGrib
            // 
            pbGrib.Location = new System.Drawing.Point(44, 32);
            pbGrib.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pbGrib.MaximumSize = new System.Drawing.Size(23, 23);
            pbGrib.MinimumSize = new System.Drawing.Size(23, 23);
            pbGrib.Name = "pbGrib";
            pbGrib.Size = new System.Drawing.Size(23, 23);
            pbGrib.TabIndex = 11;
            pbGrib.TabStop = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = System.Windows.Forms.DockStyle.Fill;
            label11.Location = new System.Drawing.Point(4, 0);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(32, 29);
            label11.TabIndex = 8;
            label11.Text = "SST";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = System.Windows.Forms.DockStyle.Fill;
            label12.Location = new System.Drawing.Point(4, 29);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(32, 29);
            label12.TabIndex = 9;
            label12.Text = "GRIB";
            label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFetchData
            // 
            btnFetchData.AutoSize = true;
            btnFetchData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnFetchData.Location = new System.Drawing.Point(184, 27);
            btnFetchData.Margin = new System.Windows.Forms.Padding(4, 27, 4, 3);
            btnFetchData.Name = "btnFetchData";
            tableLayoutPanel5.SetRowSpan(btnFetchData, 2);
            btnFetchData.Size = new System.Drawing.Size(165, 40);
            btnFetchData.TabIndex = 6;
            btnFetchData.Text = "Download initial conditions \r\nfiles from NOAA server";
            btnFetchData.UseVisualStyleBackColor = true;
            btnFetchData.Click += OnFetchDataClick;
            // 
            // gbSimControl
            // 
            gbSimControl.Controls.Add(txtSimProcOut);
            gbSimControl.Controls.Add(pbSimProgress);
            gbSimControl.Controls.Add(btnSimStart);
            gbSimControl.Dock = System.Windows.Forms.DockStyle.Fill;
            gbSimControl.Location = new System.Drawing.Point(4, 266);
            gbSimControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbSimControl.Name = "gbSimControl";
            gbSimControl.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbSimControl.Size = new System.Drawing.Size(893, 342);
            gbSimControl.TabIndex = 3;
            gbSimControl.TabStop = false;
            gbSimControl.Text = "Simulation Progress and Control";
            // 
            // txtSimProcOut
            // 
            txtSimProcOut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSimProcOut.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            txtSimProcOut.Location = new System.Drawing.Point(7, 59);
            txtSimProcOut.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtSimProcOut.Multiline = true;
            txtSimProcOut.Name = "txtSimProcOut";
            txtSimProcOut.ReadOnly = true;
            txtSimProcOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtSimProcOut.Size = new System.Drawing.Size(881, 275);
            txtSimProcOut.TabIndex = 2;
            txtSimProcOut.WordWrap = false;
            // 
            // pbSimProgress
            // 
            pbSimProgress.Location = new System.Drawing.Point(142, 22);
            pbSimProgress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pbSimProgress.Name = "pbSimProgress";
            pbSimProgress.Size = new System.Drawing.Size(748, 27);
            pbSimProgress.TabIndex = 1;
            // 
            // btnSimStart
            // 
            btnSimStart.Location = new System.Drawing.Point(7, 22);
            btnSimStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSimStart.Name = "btnSimStart";
            btnSimStart.Size = new System.Drawing.Size(113, 27);
            btnSimStart.TabIndex = 0;
            btnSimStart.Text = "Start Simulation";
            btnSimStart.UseVisualStyleBackColor = true;
            btnSimStart.Click += OnStartStopSimulation;
            // 
            // SimControlPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new System.Drawing.Size(901, 636);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SimControlPanel";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Simulation Control Panel";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            gbSimParams.ResumeLayout(false);
            gbSimParams.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            gbInitialConditions.ResumeLayout(false);
            gbInitialConditions.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbSST).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbGrib).EndInit();
            gbSimControl.ResumeLayout(false);
            gbSimControl.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbSimParams;
        private System.Windows.Forms.GroupBox gbInitialConditions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel lnkSimParams;
        private System.Windows.Forms.DateTimePicker dtpSimStop;
        private System.Windows.Forms.DateTimePicker dtpSimStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox cbRunStats;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbRange;
        private System.Windows.Forms.GroupBox gbSimControl;
        private System.Windows.Forms.Button btnSimStart;
        private System.Windows.Forms.TextBox txtSimProcOut;
        private System.Windows.Forms.ProgressBar pbSimProgress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.CheckBox cbRunStatsOnly;
        private System.Windows.Forms.PictureBox pbSST;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStepLen;
        private System.Windows.Forms.CheckBox cbAutoClose;
        private System.Windows.Forms.Button btnFetchData;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pbGrib;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox cbAutoExport;
    }
}
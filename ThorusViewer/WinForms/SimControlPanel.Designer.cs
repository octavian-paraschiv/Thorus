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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbAutoClose = new System.Windows.Forms.CheckBox();
            this.gbSimParams = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpSimStop = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkSimParams = new System.Windows.Forms.LinkLabel();
            this.dtpSimStart = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.cbRunStatsOnly = new System.Windows.Forms.CheckBox();
            this.cbRunStats = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbRange = new System.Windows.Forms.ComboBox();
            this.cmbStepLen = new System.Windows.Forms.ComboBox();
            this.gbInitialConditions = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
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
            this.btnFetchData = new System.Windows.Forms.Button();
            this.gbSimControl = new System.Windows.Forms.GroupBox();
            this.txtSimProcOut = new System.Windows.Forms.TextBox();
            this.pbSimProgress = new System.Windows.Forms.ProgressBar();
            this.btnSimStart = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbSimParams.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.gbInitialConditions.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSNOW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSOIL)).BeginInit();
            this.gbSimControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbAutoClose, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.gbSimParams, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gbInitialConditions, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gbSimControl, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(772, 551);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cbAutoClose
            // 
            this.cbAutoClose.AutoSize = true;
            this.cbAutoClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbAutoClose.Location = new System.Drawing.Point(3, 531);
            this.cbAutoClose.Name = "cbAutoClose";
            this.cbAutoClose.Size = new System.Drawing.Size(766, 17);
            this.cbAutoClose.TabIndex = 2;
            this.cbAutoClose.Text = "Automatically close this dialog after the simulation ends";
            this.cbAutoClose.UseVisualStyleBackColor = true;
            // 
            // gbSimParams
            // 
            this.gbSimParams.AutoSize = true;
            this.gbSimParams.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbSimParams.Controls.Add(this.tableLayoutPanel2);
            this.gbSimParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSimParams.Location = new System.Drawing.Point(3, 134);
            this.gbSimParams.Name = "gbSimParams";
            this.gbSimParams.Size = new System.Drawing.Size(766, 126);
            this.gbSimParams.TabIndex = 1;
            this.gbSimParams.TabStop = false;
            this.gbSimParams.Text = "Simulation Parameters";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dtpSimStop, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lnkSimParams, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.dtpSimStart, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbStepLen, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(760, 107);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // dtpSimStop
            // 
            this.dtpSimStop.CustomFormat = "yyyy-MM-dd_HH";
            this.dtpSimStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpSimStop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSimStop.Location = new System.Drawing.Point(114, 29);
            this.dtpSimStop.Name = "dtpSimStop";
            this.dtpSimStop.Size = new System.Drawing.Size(121, 20);
            this.dtpSimStop.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 28);
            this.label5.TabIndex = 3;
            this.label5.Text = "Model params:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Date/Time:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Stop Date/Time:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(3, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 27);
            this.label3.TabIndex = 2;
            this.label3.Text = "Sim step length (hrs):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Visible = false;
            // 
            // lnkSimParams
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.lnkSimParams, 2);
            this.lnkSimParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lnkSimParams.Location = new System.Drawing.Point(114, 79);
            this.lnkSimParams.Name = "lnkSimParams";
            this.lnkSimParams.Size = new System.Drawing.Size(141, 28);
            this.lnkSimParams.TabIndex = 4;
            this.lnkSimParams.TabStop = true;
            this.lnkSimParams.Text = "Click to view and edit ...";
            this.lnkSimParams.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkSimParams.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSimParams_LinkClicked);
            // 
            // dtpSimStart
            // 
            this.dtpSimStart.CustomFormat = "yyyy-MM-dd_HH";
            this.dtpSimStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpSimStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSimStart.Location = new System.Drawing.Point(114, 3);
            this.dtpSimStart.Name = "dtpSimStart";
            this.dtpSimStart.Size = new System.Drawing.Size(121, 20);
            this.dtpSimStart.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(261, 3);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel2.SetRowSpan(this.groupBox2, 4);
            this.groupBox2.Size = new System.Drawing.Size(496, 100);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistic analisys";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.cbRunStatsOnly, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.cbRunStats, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.cmbRange, 1, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(490, 81);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // cbRunStatsOnly
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.cbRunStatsOnly, 2);
            this.cbRunStatsOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRunStatsOnly.Location = new System.Drawing.Point(3, 30);
            this.cbRunStatsOnly.Name = "cbRunStatsOnly";
            this.cbRunStatsOnly.Size = new System.Drawing.Size(484, 21);
            this.cbRunStatsOnly.TabIndex = 7;
            this.cbRunStatsOnly.Text = "Run statistical analisys only (on existing simulation data)";
            this.cbRunStatsOnly.UseVisualStyleBackColor = true;
            this.cbRunStatsOnly.CheckedChanged += new System.EventHandler(this.cbRunStatsOnly_CheckedChanged);
            // 
            // cbRunStats
            // 
            this.cbRunStats.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.cbRunStats, 2);
            this.cbRunStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRunStats.Location = new System.Drawing.Point(3, 3);
            this.cbRunStats.Name = "cbRunStats";
            this.cbRunStats.Size = new System.Drawing.Size(484, 21);
            this.cbRunStats.TabIndex = 0;
            this.cbRunStats.Text = "Automatically run statistical analisys after the simulation ends";
            this.cbRunStats.UseVisualStyleBackColor = true;
            this.cbRunStats.CheckedChanged += new System.EventHandler(this.cbRunStats_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 27);
            this.label6.TabIndex = 2;
            this.label6.Text = "Statistic Range Size (days):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbRange
            // 
            this.cmbRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRange.FormattingEnabled = true;
            this.cmbRange.Items.AddRange(new object[] {
            "1",
            "2",
            "5",
            "7",
            "10",
            "15",
            "30",
            "120",
            "240",
            "365"});
            this.cmbRange.Location = new System.Drawing.Point(145, 57);
            this.cmbRange.Name = "cmbRange";
            this.cmbRange.Size = new System.Drawing.Size(342, 21);
            this.cmbRange.TabIndex = 5;
            // 
            // cmbStepLen
            // 
            this.cmbStepLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStepLen.Enabled = false;
            this.cmbStepLen.FormattingEnabled = true;
            this.cmbStepLen.Items.AddRange(new object[] {
            "2",
            "3",
            "6",
            "8",
            "12",
            "24"});
            this.cmbStepLen.Location = new System.Drawing.Point(114, 55);
            this.cmbStepLen.Name = "cmbStepLen";
            this.cmbStepLen.Size = new System.Drawing.Size(121, 21);
            this.cmbStepLen.TabIndex = 11;
            this.cmbStepLen.Visible = false;
            // 
            // gbInitialConditions
            // 
            this.gbInitialConditions.Controls.Add(this.tableLayoutPanel5);
            this.gbInitialConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInitialConditions.Location = new System.Drawing.Point(3, 3);
            this.gbInitialConditions.Name = "gbInitialConditions";
            this.gbInitialConditions.Size = new System.Drawing.Size(766, 125);
            this.gbInitialConditions.TabIndex = 0;
            this.gbInitialConditions.TabStop = false;
            this.gbInitialConditions.Text = "Initial Model Conditions";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.groupBox5, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnFetchData, 2, 2);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(760, 106);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // groupBox5
            // 
            this.groupBox5.AutoSize = true;
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.Controls.Add(this.tableLayoutPanel6);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.tableLayoutPanel5.SetRowSpan(this.groupBox5, 4);
            this.groupBox5.Size = new System.Drawing.Size(413, 100);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Initial conditions validity";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 4;
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
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 6;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(407, 81);
            this.tableLayoutPanel6.TabIndex = 0;
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
            // btnFetchData
            // 
            this.btnFetchData.AutoSize = true;
            this.btnFetchData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFetchData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFetchData.Location = new System.Drawing.Point(612, 43);
            this.btnFetchData.Name = "btnFetchData";
            this.btnFetchData.Size = new System.Drawing.Size(145, 36);
            this.btnFetchData.TabIndex = 6;
            this.btnFetchData.Text = "Download initial conditions \r\nfiles from NOAA server";
            this.btnFetchData.UseVisualStyleBackColor = true;
            this.btnFetchData.Click += new System.EventHandler(this.btnFetchData_Click);
            // 
            // gbSimControl
            // 
            this.gbSimControl.Controls.Add(this.txtSimProcOut);
            this.gbSimControl.Controls.Add(this.pbSimProgress);
            this.gbSimControl.Controls.Add(this.btnSimStart);
            this.gbSimControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSimControl.Location = new System.Drawing.Point(3, 266);
            this.gbSimControl.Name = "gbSimControl";
            this.gbSimControl.Size = new System.Drawing.Size(766, 259);
            this.gbSimControl.TabIndex = 3;
            this.gbSimControl.TabStop = false;
            this.gbSimControl.Text = "Simulation Progress and Control";
            // 
            // txtSimProcOut
            // 
            this.txtSimProcOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSimProcOut.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSimProcOut.Location = new System.Drawing.Point(6, 51);
            this.txtSimProcOut.Multiline = true;
            this.txtSimProcOut.Name = "txtSimProcOut";
            this.txtSimProcOut.ReadOnly = true;
            this.txtSimProcOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSimProcOut.Size = new System.Drawing.Size(757, 202);
            this.txtSimProcOut.TabIndex = 2;
            this.txtSimProcOut.WordWrap = false;
            // 
            // pbSimProgress
            // 
            this.pbSimProgress.Location = new System.Drawing.Point(122, 19);
            this.pbSimProgress.Name = "pbSimProgress";
            this.pbSimProgress.Size = new System.Drawing.Size(641, 23);
            this.pbSimProgress.TabIndex = 1;
            // 
            // btnSimStart
            // 
            this.btnSimStart.Location = new System.Drawing.Point(6, 19);
            this.btnSimStart.Name = "btnSimStart";
            this.btnSimStart.Size = new System.Drawing.Size(97, 23);
            this.btnSimStart.TabIndex = 0;
            this.btnSimStart.Text = "Start Simulation";
            this.btnSimStart.UseVisualStyleBackColor = true;
            this.btnSimStart.Click += new System.EventHandler(this.OnStartStopSimulation);
            // 
            // SimControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 551);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SimControlPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Simulation Control Panel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbSimParams.ResumeLayout(false);
            this.gbSimParams.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.gbInitialConditions.ResumeLayout(false);
            this.gbInitialConditions.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSNOW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSOIL)).EndInit();
            this.gbSimControl.ResumeLayout(false);
            this.gbSimControl.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pbSNOW;
        private System.Windows.Forms.PictureBox pbZZ;
        private System.Windows.Forms.PictureBox pbTT;
        private System.Windows.Forms.PictureBox pbHH;
        private System.Windows.Forms.CheckBox cbRunStatsOnly;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pbSST;
        private System.Windows.Forms.PictureBox pbSOIL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStepLen;
        private System.Windows.Forms.CheckBox cbAutoClose;
        private System.Windows.Forms.Button btnFetchData;
    }
}
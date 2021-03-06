﻿namespace InspectionSystemManager
{
    partial class ucCogLeadFormInspection
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelTitle = new CustomControl.GradientLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnLeadFormAlignAreaCheck = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.gradientLabel14 = new CustomControl.GradientLabel();
            this.btnLeadFormAlignAreaSet = new System.Windows.Forms.Button();
            this.btnLeadFormAlignAreaShow = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.numUpDownLeadFormAlignPitchSpec = new System.Windows.Forms.NumericUpDown();
            this.numUpDownAlignSkewSpec = new System.Windows.Forms.NumericUpDown();
            this.numUpDownLeadCount = new System.Windows.Forms.NumericUpDown();
            this.btnLeadPositionValueSave = new System.Windows.Forms.Button();
            this.btnNewLeadPositionValueShow = new System.Windows.Forms.Button();
            this.btnNowLeadPositionValueShow = new System.Windows.Forms.Button();
            this.gradientLabel3 = new CustomControl.GradientLabel();
            this.gradientLabel28 = new CustomControl.GradientLabel();
            this.QuickGridViewLeadFormAlignPitch = new CustomControl.QuickDataGridView();
            this.gridLeadNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridLeadPositionX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridLeadPositionY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gradientLabel25 = new CustomControl.GradientLabel();
            this.gradientLabel26 = new CustomControl.GradientLabel();
            this.hScrollBarLeadFormAlignThreshold = new System.Windows.Forms.HScrollBar();
            this.gradientLabel13 = new CustomControl.GradientLabel();
            this.graLabelLeadFormAlignThresholdValue = new CustomControl.GradientLabel();
            this.chUseLeadFormAlign = new System.Windows.Forms.CheckBox();
            this.gradientLabel5 = new CustomControl.GradientLabel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chUseLeadFormOrigin = new System.Windows.Forms.CheckBox();
            this.gradientLabel1 = new CustomControl.GradientLabel();
            this.btnLeadFormOriginAreaCheck = new System.Windows.Forms.Button();
            this.gbBodyAreaSetting = new System.Windows.Forms.GroupBox();
            this.gradientLabel2 = new CustomControl.GradientLabel();
            this.btnLeadFormOriginAreaSet = new System.Windows.Forms.Button();
            this.btnLeadFormOriginAreaShow = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLeadFormAlignPitchSpec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownAlignSkewSpec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLeadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuickGridViewLeadFormAlignPitch)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.gbBodyAreaSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.White;
            this.labelTitle.ColorBottom = System.Drawing.Color.White;
            this.labelTitle.ColorTop = System.Drawing.Color.SteelBlue;
            this.labelTitle.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.labelTitle.Location = new System.Drawing.Point(2, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(580, 28);
            this.labelTitle.TabIndex = 14;
            this.labelTitle.Text = " Lead Form Inspection Teaching Window";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.tabControl1.Location = new System.Drawing.Point(2, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(580, 325);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage2.Controls.Add(this.btnLeadFormAlignAreaCheck);
            this.tabPage2.Controls.Add(this.groupBox10);
            this.tabPage2.Controls.Add(this.groupBox9);
            this.tabPage2.Controls.Add(this.chUseLeadFormAlign);
            this.tabPage2.Controls.Add(this.gradientLabel5);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(572, 298);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Lead Form Align";
            // 
            // btnLeadFormAlignAreaCheck
            // 
            this.btnLeadFormAlignAreaCheck.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadFormAlignAreaCheck.ForeColor = System.Drawing.Color.Black;
            this.btnLeadFormAlignAreaCheck.Location = new System.Drawing.Point(174, 10);
            this.btnLeadFormAlignAreaCheck.Name = "btnLeadFormAlignAreaCheck";
            this.btnLeadFormAlignAreaCheck.Size = new System.Drawing.Size(134, 29);
            this.btnLeadFormAlignAreaCheck.TabIndex = 101;
            this.btnLeadFormAlignAreaCheck.Tag = "0";
            this.btnLeadFormAlignAreaCheck.Text = "Check";
            this.btnLeadFormAlignAreaCheck.UseVisualStyleBackColor = true;
            this.btnLeadFormAlignAreaCheck.Click += new System.EventHandler(this.btnLeadFormAlignAreaCheck_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.gradientLabel14);
            this.groupBox10.Controls.Add(this.btnLeadFormAlignAreaSet);
            this.groupBox10.Controls.Add(this.btnLeadFormAlignAreaShow);
            this.groupBox10.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox10.ForeColor = System.Drawing.Color.White;
            this.groupBox10.Location = new System.Drawing.Point(7, 45);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(559, 53);
            this.groupBox10.TabIndex = 105;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = " Lead Form Align Area Setting ";
            // 
            // gradientLabel14
            // 
            this.gradientLabel14.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel14.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel14.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel14.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel14.ForeColor = System.Drawing.Color.White;
            this.gradientLabel14.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel14.Location = new System.Drawing.Point(12, 17);
            this.gradientLabel14.Name = "gradientLabel14";
            this.gradientLabel14.Size = new System.Drawing.Size(149, 26);
            this.gradientLabel14.TabIndex = 92;
            this.gradientLabel14.Text = "Area Set";
            this.gradientLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLeadFormAlignAreaSet
            // 
            this.btnLeadFormAlignAreaSet.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadFormAlignAreaSet.ForeColor = System.Drawing.Color.Black;
            this.btnLeadFormAlignAreaSet.Location = new System.Drawing.Point(234, 16);
            this.btnLeadFormAlignAreaSet.Name = "btnLeadFormAlignAreaSet";
            this.btnLeadFormAlignAreaSet.Size = new System.Drawing.Size(66, 29);
            this.btnLeadFormAlignAreaSet.TabIndex = 91;
            this.btnLeadFormAlignAreaSet.Tag = "0";
            this.btnLeadFormAlignAreaSet.Text = "Set";
            this.btnLeadFormAlignAreaSet.UseVisualStyleBackColor = true;
            this.btnLeadFormAlignAreaSet.Click += new System.EventHandler(this.btnLeadFormAlignAreaSet_Click);
            // 
            // btnLeadFormAlignAreaShow
            // 
            this.btnLeadFormAlignAreaShow.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadFormAlignAreaShow.ForeColor = System.Drawing.Color.Black;
            this.btnLeadFormAlignAreaShow.Location = new System.Drawing.Point(167, 16);
            this.btnLeadFormAlignAreaShow.Name = "btnLeadFormAlignAreaShow";
            this.btnLeadFormAlignAreaShow.Size = new System.Drawing.Size(66, 29);
            this.btnLeadFormAlignAreaShow.TabIndex = 90;
            this.btnLeadFormAlignAreaShow.Tag = "0";
            this.btnLeadFormAlignAreaShow.Text = "Show";
            this.btnLeadFormAlignAreaShow.UseVisualStyleBackColor = true;
            this.btnLeadFormAlignAreaShow.Click += new System.EventHandler(this.btnLeadFormAlignAreaShow_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.numUpDownLeadFormAlignPitchSpec);
            this.groupBox9.Controls.Add(this.numUpDownAlignSkewSpec);
            this.groupBox9.Controls.Add(this.numUpDownLeadCount);
            this.groupBox9.Controls.Add(this.btnLeadPositionValueSave);
            this.groupBox9.Controls.Add(this.btnNewLeadPositionValueShow);
            this.groupBox9.Controls.Add(this.btnNowLeadPositionValueShow);
            this.groupBox9.Controls.Add(this.gradientLabel3);
            this.groupBox9.Controls.Add(this.gradientLabel28);
            this.groupBox9.Controls.Add(this.QuickGridViewLeadFormAlignPitch);
            this.groupBox9.Controls.Add(this.gradientLabel25);
            this.groupBox9.Controls.Add(this.gradientLabel26);
            this.groupBox9.Controls.Add(this.hScrollBarLeadFormAlignThreshold);
            this.groupBox9.Controls.Add(this.gradientLabel13);
            this.groupBox9.Controls.Add(this.graLabelLeadFormAlignThresholdValue);
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(7, 104);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(559, 190);
            this.groupBox9.TabIndex = 104;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = " Lead Form Align Condition ";
            // 
            // numUpDownLeadFormAlignPitchSpec
            // 
            this.numUpDownLeadFormAlignPitchSpec.DecimalPlaces = 3;
            this.numUpDownLeadFormAlignPitchSpec.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numUpDownLeadFormAlignPitchSpec.Location = new System.Drawing.Point(427, 71);
            this.numUpDownLeadFormAlignPitchSpec.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numUpDownLeadFormAlignPitchSpec.Name = "numUpDownLeadFormAlignPitchSpec";
            this.numUpDownLeadFormAlignPitchSpec.Size = new System.Drawing.Size(93, 21);
            this.numUpDownLeadFormAlignPitchSpec.TabIndex = 115;
            this.numUpDownLeadFormAlignPitchSpec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDownLeadFormAlignPitchSpec.Value = new decimal(new int[] {
            75,
            0,
            0,
            196608});
            // 
            // numUpDownAlignSkewSpec
            // 
            this.numUpDownAlignSkewSpec.DecimalPlaces = 3;
            this.numUpDownAlignSkewSpec.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numUpDownAlignSkewSpec.Location = new System.Drawing.Point(167, 71);
            this.numUpDownAlignSkewSpec.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numUpDownAlignSkewSpec.Name = "numUpDownAlignSkewSpec";
            this.numUpDownAlignSkewSpec.Size = new System.Drawing.Size(93, 21);
            this.numUpDownAlignSkewSpec.TabIndex = 114;
            this.numUpDownAlignSkewSpec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDownAlignSkewSpec.Value = new decimal(new int[] {
            35,
            0,
            0,
            196608});
            this.numUpDownAlignSkewSpec.ValueChanged += new System.EventHandler(this.numUpDownAlignSkewSpec_ValueChanged);
            // 
            // numUpDownLeadCount
            // 
            this.numUpDownLeadCount.Location = new System.Drawing.Point(167, 45);
            this.numUpDownLeadCount.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numUpDownLeadCount.Name = "numUpDownLeadCount";
            this.numUpDownLeadCount.Size = new System.Drawing.Size(93, 21);
            this.numUpDownLeadCount.TabIndex = 113;
            this.numUpDownLeadCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLeadPositionValueSave
            // 
            this.btnLeadPositionValueSave.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadPositionValueSave.ForeColor = System.Drawing.Color.Black;
            this.btnLeadPositionValueSave.Location = new System.Drawing.Point(479, 157);
            this.btnLeadPositionValueSave.Name = "btnLeadPositionValueSave";
            this.btnLeadPositionValueSave.Size = new System.Drawing.Size(76, 29);
            this.btnLeadPositionValueSave.TabIndex = 112;
            this.btnLeadPositionValueSave.Tag = "0";
            this.btnLeadPositionValueSave.Text = "Save";
            this.btnLeadPositionValueSave.UseVisualStyleBackColor = true;
            this.btnLeadPositionValueSave.Click += new System.EventHandler(this.btnLeadPositionValueSave_Click);
            // 
            // btnNewLeadPositionValueShow
            // 
            this.btnNewLeadPositionValueShow.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNewLeadPositionValueShow.ForeColor = System.Drawing.Color.Black;
            this.btnNewLeadPositionValueShow.Location = new System.Drawing.Point(479, 128);
            this.btnNewLeadPositionValueShow.Name = "btnNewLeadPositionValueShow";
            this.btnNewLeadPositionValueShow.Size = new System.Drawing.Size(76, 29);
            this.btnNewLeadPositionValueShow.TabIndex = 111;
            this.btnNewLeadPositionValueShow.Tag = "0";
            this.btnNewLeadPositionValueShow.Text = "New Value";
            this.btnNewLeadPositionValueShow.UseVisualStyleBackColor = true;
            this.btnNewLeadPositionValueShow.Click += new System.EventHandler(this.btnNewLeadPositionValueShow_Click);
            // 
            // btnNowLeadPositionValueShow
            // 
            this.btnNowLeadPositionValueShow.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNowLeadPositionValueShow.ForeColor = System.Drawing.Color.Black;
            this.btnNowLeadPositionValueShow.Location = new System.Drawing.Point(479, 98);
            this.btnNowLeadPositionValueShow.Name = "btnNowLeadPositionValueShow";
            this.btnNowLeadPositionValueShow.Size = new System.Drawing.Size(76, 29);
            this.btnNowLeadPositionValueShow.TabIndex = 93;
            this.btnNowLeadPositionValueShow.Tag = "0";
            this.btnNowLeadPositionValueShow.Text = "Now Value";
            this.btnNowLeadPositionValueShow.UseVisualStyleBackColor = true;
            this.btnNowLeadPositionValueShow.Click += new System.EventHandler(this.btnNowLeadPositionValueShow_Click);
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel3.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel3.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel3.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.White;
            this.gradientLabel3.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel3.Location = new System.Drawing.Point(12, 69);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(150, 24);
            this.gradientLabel3.TabIndex = 110;
            this.gradientLabel3.Text = "Skew Enable Spec (±)";
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel28
            // 
            this.gradientLabel28.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel28.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel28.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel28.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel28.ForeColor = System.Drawing.Color.White;
            this.gradientLabel28.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel28.Location = new System.Drawing.Point(12, 43);
            this.gradientLabel28.Name = "gradientLabel28";
            this.gradientLabel28.Size = new System.Drawing.Size(150, 24);
            this.gradientLabel28.TabIndex = 107;
            this.gradientLabel28.Text = "Lead Count";
            this.gradientLabel28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // QuickGridViewLeadFormAlignPitch
            // 
            this.QuickGridViewLeadFormAlignPitch.AllowUserToAddRows = false;
            this.QuickGridViewLeadFormAlignPitch.AllowUserToDeleteRows = false;
            this.QuickGridViewLeadFormAlignPitch.AllowUserToResizeColumns = false;
            this.QuickGridViewLeadFormAlignPitch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.QuickGridViewLeadFormAlignPitch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.QuickGridViewLeadFormAlignPitch.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.QuickGridViewLeadFormAlignPitch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.QuickGridViewLeadFormAlignPitch.ColumnHeadersHeight = 22;
            this.QuickGridViewLeadFormAlignPitch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.QuickGridViewLeadFormAlignPitch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridLeadNum,
            this.gridLeadPositionX,
            this.gridLeadPositionY});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.NullValue = "0";
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.QuickGridViewLeadFormAlignPitch.DefaultCellStyle = dataGridViewCellStyle6;
            this.QuickGridViewLeadFormAlignPitch.EnableHeadersVisualStyles = false;
            this.QuickGridViewLeadFormAlignPitch.Location = new System.Drawing.Point(12, 96);
            this.QuickGridViewLeadFormAlignPitch.MultiSelect = false;
            this.QuickGridViewLeadFormAlignPitch.Name = "QuickGridViewLeadFormAlignPitch";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.QuickGridViewLeadFormAlignPitch.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.QuickGridViewLeadFormAlignPitch.RowHeadersVisible = false;
            this.QuickGridViewLeadFormAlignPitch.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.QuickGridViewLeadFormAlignPitch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.QuickGridViewLeadFormAlignPitch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.QuickGridViewLeadFormAlignPitch.Size = new System.Drawing.Size(465, 90);
            this.QuickGridViewLeadFormAlignPitch.TabIndex = 105;
            this.QuickGridViewLeadFormAlignPitch.Tag = "Size : 418, 511";
            // 
            // gridLeadNum
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridLeadNum.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridLeadNum.HeaderText = "Num";
            this.gridLeadNum.Name = "gridLeadNum";
            this.gridLeadNum.ReadOnly = true;
            this.gridLeadNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLeadNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.gridLeadNum.Width = 50;
            // 
            // gridLeadPositionX
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridLeadPositionX.DefaultCellStyle = dataGridViewCellStyle4;
            this.gridLeadPositionX.HeaderText = "Lead Position X (mm)";
            this.gridLeadPositionX.Name = "gridLeadPositionX";
            this.gridLeadPositionX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLeadPositionX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.gridLeadPositionX.Width = 200;
            // 
            // gridLeadPositionY
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.gridLeadPositionY.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridLeadPositionY.HeaderText = "Lead Position Y (mm)";
            this.gridLeadPositionY.Name = "gridLeadPositionY";
            this.gridLeadPositionY.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLeadPositionY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.gridLeadPositionY.Width = 200;
            // 
            // gradientLabel25
            // 
            this.gradientLabel25.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel25.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel25.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel25.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel25.ForeColor = System.Drawing.Color.White;
            this.gradientLabel25.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel25.Location = new System.Drawing.Point(273, 69);
            this.gradientLabel25.Name = "gradientLabel25";
            this.gradientLabel25.Size = new System.Drawing.Size(150, 24);
            this.gradientLabel25.TabIndex = 104;
            this.gradientLabel25.Text = "Skew Disable Spec(±)";
            this.gradientLabel25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel26
            // 
            this.gradientLabel26.AutoSize = true;
            this.gradientLabel26.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel26.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel26.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel26.ForeColor = System.Drawing.Color.White;
            this.gradientLabel26.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel26.Location = new System.Drawing.Point(519, 74);
            this.gradientLabel26.Name = "gradientLabel26";
            this.gradientLabel26.Size = new System.Drawing.Size(29, 14);
            this.gradientLabel26.TabIndex = 102;
            this.gradientLabel26.Text = "mm";
            // 
            // hScrollBarLeadFormAlignThreshold
            // 
            this.hScrollBarLeadFormAlignThreshold.Location = new System.Drawing.Point(167, 15);
            this.hScrollBarLeadFormAlignThreshold.Maximum = 255;
            this.hScrollBarLeadFormAlignThreshold.Name = "hScrollBarLeadFormAlignThreshold";
            this.hScrollBarLeadFormAlignThreshold.Size = new System.Drawing.Size(346, 26);
            this.hScrollBarLeadFormAlignThreshold.TabIndex = 18;
            this.hScrollBarLeadFormAlignThreshold.Value = 128;
            this.hScrollBarLeadFormAlignThreshold.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarLeadFormAlignThreshold_Scroll);
            // 
            // gradientLabel13
            // 
            this.gradientLabel13.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel13.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel13.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel13.ForeColor = System.Drawing.Color.White;
            this.gradientLabel13.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel13.Location = new System.Drawing.Point(12, 17);
            this.gradientLabel13.Name = "gradientLabel13";
            this.gradientLabel13.Size = new System.Drawing.Size(150, 24);
            this.gradientLabel13.TabIndex = 17;
            this.gradientLabel13.Text = "Threshold";
            this.gradientLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // graLabelLeadFormAlignThresholdValue
            // 
            this.graLabelLeadFormAlignThresholdValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.graLabelLeadFormAlignThresholdValue.ColorBottom = System.Drawing.Color.Empty;
            this.graLabelLeadFormAlignThresholdValue.ColorTop = System.Drawing.Color.Empty;
            this.graLabelLeadFormAlignThresholdValue.ForeColor = System.Drawing.Color.White;
            this.graLabelLeadFormAlignThresholdValue.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.graLabelLeadFormAlignThresholdValue.Location = new System.Drawing.Point(510, 17);
            this.graLabelLeadFormAlignThresholdValue.Name = "graLabelLeadFormAlignThresholdValue";
            this.graLabelLeadFormAlignThresholdValue.Size = new System.Drawing.Size(38, 24);
            this.graLabelLeadFormAlignThresholdValue.TabIndex = 19;
            this.graLabelLeadFormAlignThresholdValue.Text = "128";
            this.graLabelLeadFormAlignThresholdValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chUseLeadFormAlign
            // 
            this.chUseLeadFormAlign.Appearance = System.Windows.Forms.Appearance.Button;
            this.chUseLeadFormAlign.Checked = true;
            this.chUseLeadFormAlign.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chUseLeadFormAlign.ForeColor = System.Drawing.Color.Black;
            this.chUseLeadFormAlign.Location = new System.Drawing.Point(329, 11);
            this.chUseLeadFormAlign.Name = "chUseLeadFormAlign";
            this.chUseLeadFormAlign.Size = new System.Drawing.Size(133, 26);
            this.chUseLeadFormAlign.TabIndex = 103;
            this.chUseLeadFormAlign.Text = "Used";
            this.chUseLeadFormAlign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chUseLeadFormAlign.UseVisualStyleBackColor = true;
            this.chUseLeadFormAlign.Visible = false;
            // 
            // gradientLabel5
            // 
            this.gradientLabel5.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel5.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel5.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel5.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel5.ForeColor = System.Drawing.Color.White;
            this.gradientLabel5.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel5.Location = new System.Drawing.Point(19, 10);
            this.gradientLabel5.Name = "gradientLabel5";
            this.gradientLabel5.Size = new System.Drawing.Size(150, 26);
            this.gradientLabel5.TabIndex = 102;
            this.gradientLabel5.Text = "Use lead form align";
            this.gradientLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage1.Controls.Add(this.chUseLeadFormOrigin);
            this.tabPage1.Controls.Add(this.gradientLabel1);
            this.tabPage1.Controls.Add(this.btnLeadFormOriginAreaCheck);
            this.tabPage1.Controls.Add(this.gbBodyAreaSetting);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(572, 298);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lead Form Origin";
            // 
            // chUseLeadFormOrigin
            // 
            this.chUseLeadFormOrigin.Appearance = System.Windows.Forms.Appearance.Button;
            this.chUseLeadFormOrigin.Checked = true;
            this.chUseLeadFormOrigin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chUseLeadFormOrigin.ForeColor = System.Drawing.Color.Black;
            this.chUseLeadFormOrigin.Location = new System.Drawing.Point(174, 10);
            this.chUseLeadFormOrigin.Name = "chUseLeadFormOrigin";
            this.chUseLeadFormOrigin.Size = new System.Drawing.Size(133, 26);
            this.chUseLeadFormOrigin.TabIndex = 101;
            this.chUseLeadFormOrigin.Text = "Used";
            this.chUseLeadFormOrigin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chUseLeadFormOrigin.UseVisualStyleBackColor = true;
            this.chUseLeadFormOrigin.Visible = false;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackColor = System.Drawing.Color.SeaGreen;
            this.gradientLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel1.ColorBottom = System.Drawing.Color.DarkGray;
            this.gradientLabel1.ColorTop = System.Drawing.Color.SeaGreen;
            this.gradientLabel1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.White;
            this.gradientLabel1.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel1.Location = new System.Drawing.Point(19, 10);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(150, 26);
            this.gradientLabel1.TabIndex = 99;
            this.gradientLabel1.Text = "Use lead origin";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gradientLabel1.Visible = false;
            // 
            // btnLeadFormOriginAreaCheck
            // 
            this.btnLeadFormOriginAreaCheck.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadFormOriginAreaCheck.ForeColor = System.Drawing.Color.Black;
            this.btnLeadFormOriginAreaCheck.Location = new System.Drawing.Point(500, 10);
            this.btnLeadFormOriginAreaCheck.Name = "btnLeadFormOriginAreaCheck";
            this.btnLeadFormOriginAreaCheck.Size = new System.Drawing.Size(66, 29);
            this.btnLeadFormOriginAreaCheck.TabIndex = 98;
            this.btnLeadFormOriginAreaCheck.Tag = "0";
            this.btnLeadFormOriginAreaCheck.Text = "Check";
            this.btnLeadFormOriginAreaCheck.UseVisualStyleBackColor = true;
            this.btnLeadFormOriginAreaCheck.Visible = false;
            this.btnLeadFormOriginAreaCheck.Click += new System.EventHandler(this.btnLeadFormOriginAreaCheck_Click);
            // 
            // gbBodyAreaSetting
            // 
            this.gbBodyAreaSetting.Controls.Add(this.gradientLabel2);
            this.gbBodyAreaSetting.Controls.Add(this.btnLeadFormOriginAreaSet);
            this.gbBodyAreaSetting.Controls.Add(this.btnLeadFormOriginAreaShow);
            this.gbBodyAreaSetting.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.gbBodyAreaSetting.ForeColor = System.Drawing.Color.White;
            this.gbBodyAreaSetting.Location = new System.Drawing.Point(7, 45);
            this.gbBodyAreaSetting.Name = "gbBodyAreaSetting";
            this.gbBodyAreaSetting.Size = new System.Drawing.Size(559, 53);
            this.gbBodyAreaSetting.TabIndex = 96;
            this.gbBodyAreaSetting.TabStop = false;
            this.gbBodyAreaSetting.Text = " Lead Origin Area Setting ";
            this.gbBodyAreaSetting.Visible = false;
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackColor = System.Drawing.Color.SeaGreen;
            this.gradientLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel2.ColorBottom = System.Drawing.Color.DarkGray;
            this.gradientLabel2.ColorTop = System.Drawing.Color.SeaGreen;
            this.gradientLabel2.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.ForeColor = System.Drawing.Color.White;
            this.gradientLabel2.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel2.Location = new System.Drawing.Point(12, 17);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(150, 26);
            this.gradientLabel2.TabIndex = 92;
            this.gradientLabel2.Text = "Area Set";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLeadFormOriginAreaSet
            // 
            this.btnLeadFormOriginAreaSet.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadFormOriginAreaSet.ForeColor = System.Drawing.Color.Black;
            this.btnLeadFormOriginAreaSet.Location = new System.Drawing.Point(234, 16);
            this.btnLeadFormOriginAreaSet.Name = "btnLeadFormOriginAreaSet";
            this.btnLeadFormOriginAreaSet.Size = new System.Drawing.Size(66, 29);
            this.btnLeadFormOriginAreaSet.TabIndex = 91;
            this.btnLeadFormOriginAreaSet.Tag = "0";
            this.btnLeadFormOriginAreaSet.Text = "Set";
            this.btnLeadFormOriginAreaSet.UseVisualStyleBackColor = true;
            this.btnLeadFormOriginAreaSet.Click += new System.EventHandler(this.btnLeadFormOriginAreaSet_Click);
            // 
            // btnLeadFormOriginAreaShow
            // 
            this.btnLeadFormOriginAreaShow.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadFormOriginAreaShow.ForeColor = System.Drawing.Color.Black;
            this.btnLeadFormOriginAreaShow.Location = new System.Drawing.Point(167, 16);
            this.btnLeadFormOriginAreaShow.Name = "btnLeadFormOriginAreaShow";
            this.btnLeadFormOriginAreaShow.Size = new System.Drawing.Size(66, 29);
            this.btnLeadFormOriginAreaShow.TabIndex = 90;
            this.btnLeadFormOriginAreaShow.Tag = "0";
            this.btnLeadFormOriginAreaShow.Text = "Show";
            this.btnLeadFormOriginAreaShow.UseVisualStyleBackColor = true;
            this.btnLeadFormOriginAreaShow.Click += new System.EventHandler(this.btnLeadFormOriginAreaShow_Click);
            // 
            // ucCogLeadFormInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelTitle);
            this.Name = "ucCogLeadFormInspection";
            this.Size = new System.Drawing.Size(583, 358);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLeadFormAlignPitchSpec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownAlignSkewSpec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLeadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuickGridViewLeadFormAlignPitch)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.gbBodyAreaSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControl.GradientLabel labelTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chUseLeadFormOrigin;
        private CustomControl.GradientLabel gradientLabel1;
        private System.Windows.Forms.Button btnLeadFormOriginAreaCheck;
        private System.Windows.Forms.GroupBox gbBodyAreaSetting;
        private CustomControl.GradientLabel gradientLabel2;
        private System.Windows.Forms.Button btnLeadFormOriginAreaSet;
        private System.Windows.Forms.Button btnLeadFormOriginAreaShow;
        private System.Windows.Forms.Button btnLeadFormAlignAreaCheck;
        private System.Windows.Forms.GroupBox groupBox10;
        private CustomControl.GradientLabel gradientLabel14;
        private System.Windows.Forms.Button btnLeadFormAlignAreaSet;
        private System.Windows.Forms.Button btnLeadFormAlignAreaShow;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.HScrollBar hScrollBarLeadFormAlignThreshold;
        private CustomControl.GradientLabel gradientLabel13;
        private CustomControl.GradientLabel graLabelLeadFormAlignThresholdValue;
        private System.Windows.Forms.CheckBox chUseLeadFormAlign;
        private CustomControl.GradientLabel gradientLabel5;
        private CustomControl.GradientLabel gradientLabel25;
        private CustomControl.GradientLabel gradientLabel26;
        private CustomControl.QuickDataGridView QuickGridViewLeadFormAlignPitch;
        private CustomControl.GradientLabel gradientLabel28;
        private CustomControl.GradientLabel gradientLabel3;
        private System.Windows.Forms.Button btnLeadPositionValueSave;
        private System.Windows.Forms.Button btnNewLeadPositionValueShow;
        private System.Windows.Forms.Button btnNowLeadPositionValueShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridLeadNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridLeadPositionX;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridLeadPositionY;
        private System.Windows.Forms.NumericUpDown numUpDownLeadCount;
        private System.Windows.Forms.NumericUpDown numUpDownAlignSkewSpec;
        private System.Windows.Forms.NumericUpDown numUpDownLeadFormAlignPitchSpec;
    }
}

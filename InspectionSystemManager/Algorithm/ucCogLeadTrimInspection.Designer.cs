namespace InspectionSystemManager
{
    partial class ucCogLeadTrimInspection
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
            this.labelTitle = new CustomControl.GradientLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.gradientLabel1 = new CustomControl.GradientLabel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnBodyAreaCheck = new System.Windows.Forms.Button();
            this.gradientLabel6 = new CustomControl.GradientLabel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnMaskingAdd = new System.Windows.Forms.Button();
            this.btnMaskingSet = new System.Windows.Forms.Button();
            this.btnMaskingClear = new System.Windows.Forms.Button();
            this.gradientLabel4 = new CustomControl.GradientLabel();
            this.gradientLabel3 = new CustomControl.GradientLabel();
            this.btnMaskingRedo = new System.Windows.Forms.Button();
            this.btnMaskingUndo = new System.Windows.Forms.Button();
            this.btnMaskingShow = new System.Windows.Forms.Button();
            this.gbBodyAreaSetting = new System.Windows.Forms.GroupBox();
            this.gradientLabel2 = new CustomControl.GradientLabel();
            this.btnLeadBodyAreaSet = new System.Windows.Forms.Button();
            this.btnLeadBodyAreaShow = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.gradientLabel5 = new CustomControl.GradientLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnShowAreaBottomLeft = new System.Windows.Forms.Button();
            this.kpCogDisplayControl1 = new KPDisplay.KPCogDisplayControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.btnShowAreaBottomRight = new System.Windows.Forms.Button();
            this.kpCogDisplayControl2 = new KPDisplay.KPCogDisplayControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnLeadBodyCheck = new System.Windows.Forms.Button();
            this.numericUpDownFindCount = new System.Windows.Forms.NumericUpDown();
            this.gradientLabel8 = new CustomControl.GradientLabel();
            this.numericUpDownFindScore = new System.Windows.Forms.NumericUpDown();
            this.gradientLabel9 = new CustomControl.GradientLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFindLeft = new System.Windows.Forms.Button();
            this.btnPatternModifyLeft = new System.Windows.Forms.Button();
            this.btnPatternAddLeft = new System.Windows.Forms.Button();
            this.btnShowAreaTopLeft = new System.Windows.Forms.Button();
            this.kpPatternDisplay = new KPDisplay.KPCogDisplayControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFindRight = new System.Windows.Forms.Button();
            this.btnPatternModifyRight = new System.Windows.Forms.Button();
            this.btnPatternAddRight = new System.Windows.Forms.Button();
            this.btnShowAreaTopRight = new System.Windows.Forms.Button();
            this.kpPatternDisplay1 = new KPDisplay.KPCogDisplayControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.gbBodyAreaSetting.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFindCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFindScore)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.labelTitle.TabIndex = 13;
            this.labelTitle.Text = " Lead Inspection Teaching Window";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.tabControl1.Location = new System.Drawing.Point(2, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(580, 325);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.gradientLabel1);
            this.tabPage1.Controls.Add(this.groupBox8);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.gbBodyAreaSetting);
            this.tabPage1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(572, 298);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = " Lead Body";
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.ForeColor = System.Drawing.Color.Black;
            this.checkBox1.Location = new System.Drawing.Point(142, 10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(133, 26);
            this.checkBox1.TabIndex = 95;
            this.checkBox1.Text = "Used";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackColor = System.Drawing.Color.SeaGreen;
            this.gradientLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel1.ColorBottom = System.Drawing.Color.LightGray;
            this.gradientLabel1.ColorTop = System.Drawing.Color.SeaGreen;
            this.gradientLabel1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.White;
            this.gradientLabel1.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel1.Location = new System.Drawing.Point(9, 10);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel1.TabIndex = 92;
            this.gradientLabel1.Text = "Use lead body";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Controls.Add(this.textBox1);
            this.groupBox8.Controls.Add(this.btnBodyAreaCheck);
            this.groupBox8.Controls.Add(this.gradientLabel6);
            this.groupBox8.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox8.ForeColor = System.Drawing.Color.White;
            this.groupBox8.Location = new System.Drawing.Point(6, 227);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(558, 65);
            this.groupBox8.TabIndex = 94;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = " Body Area Check ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(343, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 14);
            this.label1.TabIndex = 94;
            this.label1.Text = "(Left top, right top based on)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(136, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(64, 21);
            this.textBox1.TabIndex = 93;
            this.textBox1.Text = "20.25";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnBodyAreaCheck
            // 
            this.btnBodyAreaCheck.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBodyAreaCheck.ForeColor = System.Drawing.Color.Black;
            this.btnBodyAreaCheck.Location = new System.Drawing.Point(271, 21);
            this.btnBodyAreaCheck.Name = "btnBodyAreaCheck";
            this.btnBodyAreaCheck.Size = new System.Drawing.Size(66, 29);
            this.btnBodyAreaCheck.TabIndex = 89;
            this.btnBodyAreaCheck.Tag = "0";
            this.btnBodyAreaCheck.Text = "Check";
            this.btnBodyAreaCheck.UseVisualStyleBackColor = true;
            this.btnBodyAreaCheck.Click += new System.EventHandler(this.btnBodyAreaCheck_Click);
            // 
            // gradientLabel6
            // 
            this.gradientLabel6.BackColor = System.Drawing.Color.SeaGreen;
            this.gradientLabel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel6.ColorBottom = System.Drawing.Color.LightGray;
            this.gradientLabel6.ColorTop = System.Drawing.Color.SeaGreen;
            this.gradientLabel6.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel6.ForeColor = System.Drawing.Color.White;
            this.gradientLabel6.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel6.Location = new System.Drawing.Point(6, 22);
            this.gradientLabel6.Name = "gradientLabel6";
            this.gradientLabel6.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel6.TabIndex = 92;
            this.gradientLabel6.Text = "Body Angle";
            this.gradientLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnMaskingAdd);
            this.groupBox6.Controls.Add(this.btnMaskingSet);
            this.groupBox6.Controls.Add(this.btnMaskingClear);
            this.groupBox6.Controls.Add(this.gradientLabel4);
            this.groupBox6.Controls.Add(this.gradientLabel3);
            this.groupBox6.Controls.Add(this.btnMaskingRedo);
            this.groupBox6.Controls.Add(this.btnMaskingUndo);
            this.groupBox6.Controls.Add(this.btnMaskingShow);
            this.groupBox6.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(6, 126);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(558, 86);
            this.groupBox6.TabIndex = 88;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = " Masking Area Setting ";
            // 
            // btnMaskingAdd
            // 
            this.btnMaskingAdd.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaskingAdd.ForeColor = System.Drawing.Color.Black;
            this.btnMaskingAdd.Location = new System.Drawing.Point(203, 21);
            this.btnMaskingAdd.Name = "btnMaskingAdd";
            this.btnMaskingAdd.Size = new System.Drawing.Size(66, 29);
            this.btnMaskingAdd.TabIndex = 96;
            this.btnMaskingAdd.Tag = "0";
            this.btnMaskingAdd.Text = "Add";
            this.btnMaskingAdd.UseVisualStyleBackColor = true;
            this.btnMaskingAdd.Click += new System.EventHandler(this.btnMaskingAdd_Click);
            // 
            // btnMaskingSet
            // 
            this.btnMaskingSet.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaskingSet.ForeColor = System.Drawing.Color.Black;
            this.btnMaskingSet.Location = new System.Drawing.Point(270, 21);
            this.btnMaskingSet.Name = "btnMaskingSet";
            this.btnMaskingSet.Size = new System.Drawing.Size(66, 29);
            this.btnMaskingSet.TabIndex = 95;
            this.btnMaskingSet.Tag = "0";
            this.btnMaskingSet.Text = "Set";
            this.btnMaskingSet.UseVisualStyleBackColor = true;
            this.btnMaskingSet.Click += new System.EventHandler(this.btnMaskingSet_Click);
            // 
            // btnMaskingClear
            // 
            this.btnMaskingClear.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaskingClear.ForeColor = System.Drawing.Color.Black;
            this.btnMaskingClear.Location = new System.Drawing.Point(270, 50);
            this.btnMaskingClear.Name = "btnMaskingClear";
            this.btnMaskingClear.Size = new System.Drawing.Size(66, 29);
            this.btnMaskingClear.TabIndex = 94;
            this.btnMaskingClear.Tag = "0";
            this.btnMaskingClear.Text = "Clear";
            this.btnMaskingClear.UseVisualStyleBackColor = true;
            this.btnMaskingClear.Click += new System.EventHandler(this.btnMaskingClear_Click);
            // 
            // gradientLabel4
            // 
            this.gradientLabel4.BackColor = System.Drawing.Color.SeaGreen;
            this.gradientLabel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel4.ColorBottom = System.Drawing.Color.LightGray;
            this.gradientLabel4.ColorTop = System.Drawing.Color.SeaGreen;
            this.gradientLabel4.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel4.ForeColor = System.Drawing.Color.White;
            this.gradientLabel4.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel4.Location = new System.Drawing.Point(6, 51);
            this.gradientLabel4.Name = "gradientLabel4";
            this.gradientLabel4.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel4.TabIndex = 92;
            this.gradientLabel4.Text = "Masking Edit";
            this.gradientLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackColor = System.Drawing.Color.SeaGreen;
            this.gradientLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel3.ColorBottom = System.Drawing.Color.LightGray;
            this.gradientLabel3.ColorTop = System.Drawing.Color.SeaGreen;
            this.gradientLabel3.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.White;
            this.gradientLabel3.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel3.Location = new System.Drawing.Point(6, 22);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel3.TabIndex = 92;
            this.gradientLabel3.Text = "Masking Tool";
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMaskingRedo
            // 
            this.btnMaskingRedo.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaskingRedo.ForeColor = System.Drawing.Color.Black;
            this.btnMaskingRedo.Location = new System.Drawing.Point(203, 50);
            this.btnMaskingRedo.Name = "btnMaskingRedo";
            this.btnMaskingRedo.Size = new System.Drawing.Size(66, 29);
            this.btnMaskingRedo.TabIndex = 93;
            this.btnMaskingRedo.Tag = "0";
            this.btnMaskingRedo.Text = "Redo";
            this.btnMaskingRedo.UseVisualStyleBackColor = true;
            this.btnMaskingRedo.Click += new System.EventHandler(this.btnMaskingRedo_Click);
            // 
            // btnMaskingUndo
            // 
            this.btnMaskingUndo.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaskingUndo.ForeColor = System.Drawing.Color.Black;
            this.btnMaskingUndo.Location = new System.Drawing.Point(136, 50);
            this.btnMaskingUndo.Name = "btnMaskingUndo";
            this.btnMaskingUndo.Size = new System.Drawing.Size(66, 29);
            this.btnMaskingUndo.TabIndex = 92;
            this.btnMaskingUndo.Tag = "0";
            this.btnMaskingUndo.Text = "Undo";
            this.btnMaskingUndo.UseVisualStyleBackColor = true;
            this.btnMaskingUndo.Click += new System.EventHandler(this.btnMaskingUndo_Click);
            // 
            // btnMaskingShow
            // 
            this.btnMaskingShow.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaskingShow.ForeColor = System.Drawing.Color.Black;
            this.btnMaskingShow.Location = new System.Drawing.Point(135, 21);
            this.btnMaskingShow.Name = "btnMaskingShow";
            this.btnMaskingShow.Size = new System.Drawing.Size(66, 29);
            this.btnMaskingShow.TabIndex = 91;
            this.btnMaskingShow.Tag = "0";
            this.btnMaskingShow.Text = "Show";
            this.btnMaskingShow.UseVisualStyleBackColor = true;
            this.btnMaskingShow.Click += new System.EventHandler(this.btnMaskingShow_Click);
            // 
            // gbBodyAreaSetting
            // 
            this.gbBodyAreaSetting.Controls.Add(this.gradientLabel2);
            this.gbBodyAreaSetting.Controls.Add(this.btnLeadBodyAreaSet);
            this.gbBodyAreaSetting.Controls.Add(this.btnLeadBodyAreaShow);
            this.gbBodyAreaSetting.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.gbBodyAreaSetting.ForeColor = System.Drawing.Color.White;
            this.gbBodyAreaSetting.Location = new System.Drawing.Point(6, 51);
            this.gbBodyAreaSetting.Name = "gbBodyAreaSetting";
            this.gbBodyAreaSetting.Size = new System.Drawing.Size(558, 60);
            this.gbBodyAreaSetting.TabIndex = 87;
            this.gbBodyAreaSetting.TabStop = false;
            this.gbBodyAreaSetting.Text = " Body Area Setting ";
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackColor = System.Drawing.Color.SeaGreen;
            this.gradientLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel2.ColorBottom = System.Drawing.Color.LightGray;
            this.gradientLabel2.ColorTop = System.Drawing.Color.SeaGreen;
            this.gradientLabel2.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.ForeColor = System.Drawing.Color.White;
            this.gradientLabel2.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel2.Location = new System.Drawing.Point(6, 22);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel2.TabIndex = 92;
            this.gradientLabel2.Text = "Area Set";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLeadBodyAreaSet
            // 
            this.btnLeadBodyAreaSet.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadBodyAreaSet.ForeColor = System.Drawing.Color.Black;
            this.btnLeadBodyAreaSet.Location = new System.Drawing.Point(203, 21);
            this.btnLeadBodyAreaSet.Name = "btnLeadBodyAreaSet";
            this.btnLeadBodyAreaSet.Size = new System.Drawing.Size(66, 29);
            this.btnLeadBodyAreaSet.TabIndex = 91;
            this.btnLeadBodyAreaSet.Tag = "0";
            this.btnLeadBodyAreaSet.Text = "Set";
            this.btnLeadBodyAreaSet.UseVisualStyleBackColor = true;
            this.btnLeadBodyAreaSet.Click += new System.EventHandler(this.btnLeadBodyAreaSet_Click);
            // 
            // btnLeadBodyAreaShow
            // 
            this.btnLeadBodyAreaShow.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadBodyAreaShow.ForeColor = System.Drawing.Color.Black;
            this.btnLeadBodyAreaShow.Location = new System.Drawing.Point(136, 21);
            this.btnLeadBodyAreaShow.Name = "btnLeadBodyAreaShow";
            this.btnLeadBodyAreaShow.Size = new System.Drawing.Size(66, 29);
            this.btnLeadBodyAreaShow.TabIndex = 90;
            this.btnLeadBodyAreaShow.Tag = "0";
            this.btnLeadBodyAreaShow.Text = "Show";
            this.btnLeadBodyAreaShow.UseVisualStyleBackColor = true;
            this.btnLeadBodyAreaShow.Click += new System.EventHandler(this.btnLeadBodyAreaShow_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage2.Controls.Add(this.checkBox2);
            this.tabPage2.Controls.Add(this.gradientLabel5);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(572, 298);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = " Mold ChipOut";
            // 
            // checkBox2
            // 
            this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox2.ForeColor = System.Drawing.Color.Black;
            this.checkBox2.Location = new System.Drawing.Point(141, 10);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(133, 26);
            this.checkBox2.TabIndex = 97;
            this.checkBox2.Text = "Unused";
            this.checkBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox2.UseVisualStyleBackColor = true;
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
            this.gradientLabel5.Location = new System.Drawing.Point(8, 10);
            this.gradientLabel5.Name = "gradientLabel5";
            this.gradientLabel5.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel5.TabIndex = 96;
            this.gradientLabel5.Text = "Use lead body";
            this.gradientLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(572, 298);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = " Bent/Length ";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(572, 298);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Shoulder Burr/Lead";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(572, 298);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Tip Burr";
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.tabPage6.Controls.Add(this.groupBox4);
            this.tabPage6.Controls.Add(this.groupBox5);
            this.tabPage6.Controls.Add(this.groupBox3);
            this.tabPage6.Controls.Add(this.groupBox1);
            this.tabPage6.Controls.Add(this.groupBox2);
            this.tabPage6.Location = new System.Drawing.Point(4, 23);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(572, 298);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Gate Remaining";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.btnShowAreaBottomLeft);
            this.groupBox4.Controls.Add(this.kpCogDisplayControl1);
            this.groupBox4.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(289, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(136, 206);
            this.groupBox4.TabIndex = 87;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = " Bottom Left ";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(70, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 30);
            this.button1.TabIndex = 78;
            this.button1.Tag = "0";
            this.button1.Text = "Find";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(3, 170);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 30);
            this.button2.TabIndex = 77;
            this.button2.Tag = "0";
            this.button2.Text = "Modify";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(70, 140);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(62, 30);
            this.button3.TabIndex = 76;
            this.button3.Tag = "0";
            this.button3.Text = "Add";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnShowAreaBottomLeft
            // 
            this.btnShowAreaBottomLeft.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShowAreaBottomLeft.ForeColor = System.Drawing.Color.Black;
            this.btnShowAreaBottomLeft.Location = new System.Drawing.Point(3, 140);
            this.btnShowAreaBottomLeft.Name = "btnShowAreaBottomLeft";
            this.btnShowAreaBottomLeft.Size = new System.Drawing.Size(62, 30);
            this.btnShowAreaBottomLeft.TabIndex = 75;
            this.btnShowAreaBottomLeft.Tag = "0";
            this.btnShowAreaBottomLeft.Text = "Area";
            this.btnShowAreaBottomLeft.UseVisualStyleBackColor = true;
            // 
            // kpCogDisplayControl1
            // 
            this.kpCogDisplayControl1.BackColor = System.Drawing.Color.White;
            this.kpCogDisplayControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kpCogDisplayControl1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.kpCogDisplayControl1.Location = new System.Drawing.Point(5, 13);
            this.kpCogDisplayControl1.Name = "kpCogDisplayControl1";
            this.kpCogDisplayControl1.Size = new System.Drawing.Size(127, 127);
            this.kpCogDisplayControl1.TabIndex = 13;
            this.kpCogDisplayControl1.Tag = "0";
            this.kpCogDisplayControl1.UseStatusBar = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Controls.Add(this.button6);
            this.groupBox5.Controls.Add(this.button7);
            this.groupBox5.Controls.Add(this.btnShowAreaBottomRight);
            this.groupBox5.Controls.Add(this.kpCogDisplayControl2);
            this.groupBox5.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(431, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(136, 206);
            this.groupBox5.TabIndex = 89;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Bottom Right";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button5.ForeColor = System.Drawing.Color.Black;
            this.button5.Location = new System.Drawing.Point(70, 170);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(62, 30);
            this.button5.TabIndex = 82;
            this.button5.Tag = "1";
            this.button5.Text = "Find";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button6.ForeColor = System.Drawing.Color.Black;
            this.button6.Location = new System.Drawing.Point(5, 170);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(62, 30);
            this.button6.TabIndex = 81;
            this.button6.Tag = "1";
            this.button6.Text = "Modify";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button7.ForeColor = System.Drawing.Color.Black;
            this.button7.Location = new System.Drawing.Point(70, 140);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(62, 30);
            this.button7.TabIndex = 80;
            this.button7.Tag = "1";
            this.button7.Text = "Add";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // btnShowAreaBottomRight
            // 
            this.btnShowAreaBottomRight.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShowAreaBottomRight.ForeColor = System.Drawing.Color.Black;
            this.btnShowAreaBottomRight.Location = new System.Drawing.Point(5, 140);
            this.btnShowAreaBottomRight.Name = "btnShowAreaBottomRight";
            this.btnShowAreaBottomRight.Size = new System.Drawing.Size(62, 30);
            this.btnShowAreaBottomRight.TabIndex = 79;
            this.btnShowAreaBottomRight.Tag = "1";
            this.btnShowAreaBottomRight.Text = "Area";
            this.btnShowAreaBottomRight.UseVisualStyleBackColor = true;
            // 
            // kpCogDisplayControl2
            // 
            this.kpCogDisplayControl2.BackColor = System.Drawing.Color.White;
            this.kpCogDisplayControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kpCogDisplayControl2.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.kpCogDisplayControl2.Location = new System.Drawing.Point(5, 13);
            this.kpCogDisplayControl2.Name = "kpCogDisplayControl2";
            this.kpCogDisplayControl2.Size = new System.Drawing.Size(127, 127);
            this.kpCogDisplayControl2.TabIndex = 13;
            this.kpCogDisplayControl2.Tag = "1";
            this.kpCogDisplayControl2.UseStatusBar = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnLeadBodyCheck);
            this.groupBox3.Controls.Add(this.numericUpDownFindCount);
            this.groupBox3.Controls.Add(this.gradientLabel8);
            this.groupBox3.Controls.Add(this.numericUpDownFindScore);
            this.groupBox3.Controls.Add(this.gradientLabel9);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(5, 215);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(562, 80);
            this.groupBox3.TabIndex = 88;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Condition Setting ";
            // 
            // btnLeadBodyCheck
            // 
            this.btnLeadBodyCheck.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLeadBodyCheck.ForeColor = System.Drawing.Color.Black;
            this.btnLeadBodyCheck.Location = new System.Drawing.Point(490, 33);
            this.btnLeadBodyCheck.Name = "btnLeadBodyCheck";
            this.btnLeadBodyCheck.Size = new System.Drawing.Size(66, 36);
            this.btnLeadBodyCheck.TabIndex = 79;
            this.btnLeadBodyCheck.Tag = "0";
            this.btnLeadBodyCheck.Text = "Check";
            this.btnLeadBodyCheck.UseVisualStyleBackColor = true;
            // 
            // numericUpDownFindCount
            // 
            this.numericUpDownFindCount.Location = new System.Drawing.Point(139, 48);
            this.numericUpDownFindCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFindCount.Name = "numericUpDownFindCount";
            this.numericUpDownFindCount.Size = new System.Drawing.Size(101, 21);
            this.numericUpDownFindCount.TabIndex = 63;
            this.numericUpDownFindCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownFindCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gradientLabel8
            // 
            this.gradientLabel8.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel8.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel8.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel8.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel8.ForeColor = System.Drawing.Color.White;
            this.gradientLabel8.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel8.Location = new System.Drawing.Point(8, 46);
            this.gradientLabel8.Name = "gradientLabel8";
            this.gradientLabel8.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel8.TabIndex = 62;
            this.gradientLabel8.Text = "Find Count";
            this.gradientLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownFindScore
            // 
            this.numericUpDownFindScore.DecimalPlaces = 2;
            this.numericUpDownFindScore.Location = new System.Drawing.Point(139, 18);
            this.numericUpDownFindScore.Name = "numericUpDownFindScore";
            this.numericUpDownFindScore.Size = new System.Drawing.Size(101, 21);
            this.numericUpDownFindScore.TabIndex = 61;
            this.numericUpDownFindScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownFindScore.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // gradientLabel9
            // 
            this.gradientLabel9.BackColor = System.Drawing.Color.SteelBlue;
            this.gradientLabel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientLabel9.ColorBottom = System.Drawing.Color.Empty;
            this.gradientLabel9.ColorTop = System.Drawing.Color.Empty;
            this.gradientLabel9.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel9.ForeColor = System.Drawing.Color.White;
            this.gradientLabel9.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel9.Location = new System.Drawing.Point(8, 16);
            this.gradientLabel9.Name = "gradientLabel9";
            this.gradientLabel9.Size = new System.Drawing.Size(124, 26);
            this.gradientLabel9.TabIndex = 60;
            this.gradientLabel9.Text = "Find Score";
            this.gradientLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(243, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 14);
            this.label5.TabIndex = 74;
            this.label5.Text = "ea";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(244, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 14);
            this.label4.TabIndex = 75;
            this.label4.Text = "%";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFindLeft);
            this.groupBox1.Controls.Add(this.btnPatternModifyLeft);
            this.groupBox1.Controls.Add(this.btnPatternAddLeft);
            this.groupBox1.Controls.Add(this.btnShowAreaTopLeft);
            this.groupBox1.Controls.Add(this.kpPatternDisplay);
            this.groupBox1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 206);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Top Left ";
            // 
            // btnFindLeft
            // 
            this.btnFindLeft.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFindLeft.ForeColor = System.Drawing.Color.Black;
            this.btnFindLeft.Location = new System.Drawing.Point(70, 170);
            this.btnFindLeft.Name = "btnFindLeft";
            this.btnFindLeft.Size = new System.Drawing.Size(62, 30);
            this.btnFindLeft.TabIndex = 78;
            this.btnFindLeft.Tag = "0";
            this.btnFindLeft.Text = "Find";
            this.btnFindLeft.UseVisualStyleBackColor = true;
            // 
            // btnPatternModifyLeft
            // 
            this.btnPatternModifyLeft.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPatternModifyLeft.ForeColor = System.Drawing.Color.Black;
            this.btnPatternModifyLeft.Location = new System.Drawing.Point(3, 170);
            this.btnPatternModifyLeft.Name = "btnPatternModifyLeft";
            this.btnPatternModifyLeft.Size = new System.Drawing.Size(62, 30);
            this.btnPatternModifyLeft.TabIndex = 77;
            this.btnPatternModifyLeft.Tag = "0";
            this.btnPatternModifyLeft.Text = "Modify";
            this.btnPatternModifyLeft.UseVisualStyleBackColor = true;
            // 
            // btnPatternAddLeft
            // 
            this.btnPatternAddLeft.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPatternAddLeft.ForeColor = System.Drawing.Color.Black;
            this.btnPatternAddLeft.Location = new System.Drawing.Point(70, 140);
            this.btnPatternAddLeft.Name = "btnPatternAddLeft";
            this.btnPatternAddLeft.Size = new System.Drawing.Size(62, 30);
            this.btnPatternAddLeft.TabIndex = 76;
            this.btnPatternAddLeft.Tag = "0";
            this.btnPatternAddLeft.Text = "Add";
            this.btnPatternAddLeft.UseVisualStyleBackColor = true;
            // 
            // btnShowAreaTopLeft
            // 
            this.btnShowAreaTopLeft.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShowAreaTopLeft.ForeColor = System.Drawing.Color.Black;
            this.btnShowAreaTopLeft.Location = new System.Drawing.Point(3, 140);
            this.btnShowAreaTopLeft.Name = "btnShowAreaTopLeft";
            this.btnShowAreaTopLeft.Size = new System.Drawing.Size(62, 30);
            this.btnShowAreaTopLeft.TabIndex = 75;
            this.btnShowAreaTopLeft.Tag = "0";
            this.btnShowAreaTopLeft.Text = "Area";
            this.btnShowAreaTopLeft.UseVisualStyleBackColor = true;
            // 
            // kpPatternDisplay
            // 
            this.kpPatternDisplay.BackColor = System.Drawing.Color.White;
            this.kpPatternDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kpPatternDisplay.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.kpPatternDisplay.Location = new System.Drawing.Point(5, 13);
            this.kpPatternDisplay.Name = "kpPatternDisplay";
            this.kpPatternDisplay.Size = new System.Drawing.Size(127, 127);
            this.kpPatternDisplay.TabIndex = 13;
            this.kpPatternDisplay.Tag = "0";
            this.kpPatternDisplay.UseStatusBar = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFindRight);
            this.groupBox2.Controls.Add(this.btnPatternModifyRight);
            this.groupBox2.Controls.Add(this.btnPatternAddRight);
            this.groupBox2.Controls.Add(this.btnShowAreaTopRight);
            this.groupBox2.Controls.Add(this.kpPatternDisplay1);
            this.groupBox2.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(147, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(136, 206);
            this.groupBox2.TabIndex = 86;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Top Right ";
            // 
            // btnFindRight
            // 
            this.btnFindRight.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFindRight.ForeColor = System.Drawing.Color.Black;
            this.btnFindRight.Location = new System.Drawing.Point(70, 170);
            this.btnFindRight.Name = "btnFindRight";
            this.btnFindRight.Size = new System.Drawing.Size(62, 30);
            this.btnFindRight.TabIndex = 82;
            this.btnFindRight.Tag = "1";
            this.btnFindRight.Text = "Find";
            this.btnFindRight.UseVisualStyleBackColor = true;
            // 
            // btnPatternModifyRight
            // 
            this.btnPatternModifyRight.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPatternModifyRight.ForeColor = System.Drawing.Color.Black;
            this.btnPatternModifyRight.Location = new System.Drawing.Point(5, 170);
            this.btnPatternModifyRight.Name = "btnPatternModifyRight";
            this.btnPatternModifyRight.Size = new System.Drawing.Size(62, 30);
            this.btnPatternModifyRight.TabIndex = 81;
            this.btnPatternModifyRight.Tag = "1";
            this.btnPatternModifyRight.Text = "Modify";
            this.btnPatternModifyRight.UseVisualStyleBackColor = true;
            // 
            // btnPatternAddRight
            // 
            this.btnPatternAddRight.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPatternAddRight.ForeColor = System.Drawing.Color.Black;
            this.btnPatternAddRight.Location = new System.Drawing.Point(70, 140);
            this.btnPatternAddRight.Name = "btnPatternAddRight";
            this.btnPatternAddRight.Size = new System.Drawing.Size(62, 30);
            this.btnPatternAddRight.TabIndex = 80;
            this.btnPatternAddRight.Tag = "1";
            this.btnPatternAddRight.Text = "Add";
            this.btnPatternAddRight.UseVisualStyleBackColor = true;
            // 
            // btnShowAreaTopRight
            // 
            this.btnShowAreaTopRight.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnShowAreaTopRight.ForeColor = System.Drawing.Color.Black;
            this.btnShowAreaTopRight.Location = new System.Drawing.Point(5, 140);
            this.btnShowAreaTopRight.Name = "btnShowAreaTopRight";
            this.btnShowAreaTopRight.Size = new System.Drawing.Size(62, 30);
            this.btnShowAreaTopRight.TabIndex = 79;
            this.btnShowAreaTopRight.Tag = "1";
            this.btnShowAreaTopRight.Text = "Area";
            this.btnShowAreaTopRight.UseVisualStyleBackColor = true;
            // 
            // kpPatternDisplay1
            // 
            this.kpPatternDisplay1.BackColor = System.Drawing.Color.White;
            this.kpPatternDisplay1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kpPatternDisplay1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.kpPatternDisplay1.Location = new System.Drawing.Point(5, 13);
            this.kpPatternDisplay1.Name = "kpPatternDisplay1";
            this.kpPatternDisplay1.Size = new System.Drawing.Size(127, 127);
            this.kpPatternDisplay1.TabIndex = 13;
            this.kpPatternDisplay1.Tag = "1";
            this.kpPatternDisplay1.UseStatusBar = false;
            // 
            // ucCogLeadTrimInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelTitle);
            this.Name = "ucCogLeadTrimInspection";
            this.Size = new System.Drawing.Size(583, 358);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.gbBodyAreaSetting.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFindCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFindScore)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControl.GradientLabel labelTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button btnBodyAreaCheck;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnMaskingRedo;
        private System.Windows.Forms.Button btnMaskingUndo;
        private System.Windows.Forms.Button btnMaskingShow;
        private System.Windows.Forms.GroupBox gbBodyAreaSetting;
        private System.Windows.Forms.Button btnLeadBodyAreaSet;
        private System.Windows.Forms.Button btnLeadBodyAreaShow;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnShowAreaBottomLeft;
        private KPDisplay.KPCogDisplayControl kpCogDisplayControl1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnShowAreaBottomRight;
        private KPDisplay.KPCogDisplayControl kpCogDisplayControl2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnLeadBodyCheck;
        private System.Windows.Forms.NumericUpDown numericUpDownFindCount;
        private CustomControl.GradientLabel gradientLabel8;
        private System.Windows.Forms.NumericUpDown numericUpDownFindScore;
        private CustomControl.GradientLabel gradientLabel9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFindLeft;
        private System.Windows.Forms.Button btnPatternModifyLeft;
        private System.Windows.Forms.Button btnPatternAddLeft;
        private System.Windows.Forms.Button btnShowAreaTopLeft;
        private KPDisplay.KPCogDisplayControl kpPatternDisplay;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFindRight;
        private System.Windows.Forms.Button btnPatternModifyRight;
        private System.Windows.Forms.Button btnPatternAddRight;
        private System.Windows.Forms.Button btnShowAreaTopRight;
        private KPDisplay.KPCogDisplayControl kpPatternDisplay1;
        private System.Windows.Forms.Button btnMaskingClear;
        private CustomControl.GradientLabel gradientLabel4;
        private CustomControl.GradientLabel gradientLabel3;
        private CustomControl.GradientLabel gradientLabel2;
        private System.Windows.Forms.Button btnMaskingSet;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox textBox1;
        private CustomControl.GradientLabel gradientLabel6;
        private System.Windows.Forms.Button btnMaskingAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private CustomControl.GradientLabel gradientLabel1;
        private System.Windows.Forms.CheckBox checkBox2;
        private CustomControl.GradientLabel gradientLabel5;
    }
}

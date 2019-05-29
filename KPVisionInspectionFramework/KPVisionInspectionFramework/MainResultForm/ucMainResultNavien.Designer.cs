namespace KPVisionInspectionFramework
{
    partial class ucMainResultNavien
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBarcode = new System.Windows.Forms.TextBox();
            this.dataGridViewLeft = new System.Windows.Forms.DataGridView();
            this.gradientLabel9 = new CustomControl.GradientLabel();
            this.gradientLabelResultLeft2 = new CustomControl.GradientLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnResultTest = new System.Windows.Forms.Button();
            this.gradientLabel7 = new CustomControl.GradientLabel();
            this.gradientLabel4 = new CustomControl.GradientLabel();
            this.gradientLabelResultLeft1 = new CustomControl.GradientLabel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.gradientLabel6 = new CustomControl.GradientLabel();
            this.gradientLabelResultRight3 = new CustomControl.GradientLabel();
            this.gradientLabel1 = new CustomControl.GradientLabel();
            this.gradientLabelResultRight2 = new CustomControl.GradientLabel();
            this.gradientLabel3 = new CustomControl.GradientLabel();
            this.gradientLabelResultRight1 = new CustomControl.GradientLabel();
            this.dataGridViewRight = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.gradientLabel8 = new CustomControl.GradientLabel();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeft)).BeginInit();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.panelLeft.Controls.Add(this.label1);
            this.panelLeft.Controls.Add(this.textBoxBarcode);
            this.panelLeft.Controls.Add(this.dataGridViewLeft);
            this.panelLeft.Controls.Add(this.gradientLabel9);
            this.panelLeft.Controls.Add(this.gradientLabelResultLeft2);
            this.panelLeft.Controls.Add(this.button1);
            this.panelLeft.Controls.Add(this.btnResultTest);
            this.panelLeft.Controls.Add(this.gradientLabel7);
            this.panelLeft.Controls.Add(this.gradientLabel4);
            this.panelLeft.Controls.Add(this.gradientLabelResultLeft1);
            this.panelLeft.Location = new System.Drawing.Point(0, 1);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(955, 234);
            this.panelLeft.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 22);
            this.label1.TabIndex = 50;
            this.label1.Text = "BarCode";
            // 
            // textBoxBarcode
            // 
            this.textBoxBarcode.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxBarcode.Location = new System.Drawing.Point(5, 119);
            this.textBoxBarcode.Name = "textBoxBarcode";
            this.textBoxBarcode.Size = new System.Drawing.Size(190, 29);
            this.textBoxBarcode.TabIndex = 49;
            this.textBoxBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxBarcode_KeyDown);
            // 
            // dataGridViewLeft
            // 
            this.dataGridViewLeft.AllowUserToAddRows = false;
            this.dataGridViewLeft.AllowUserToDeleteRows = false;
            this.dataGridViewLeft.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewLeft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewLeft.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewLeft.Location = new System.Drawing.Point(594, 34);
            this.dataGridViewLeft.Name = "dataGridViewLeft";
            this.dataGridViewLeft.ReadOnly = true;
            this.dataGridViewLeft.RowHeadersVisible = false;
            this.dataGridViewLeft.RowTemplate.Height = 23;
            this.dataGridViewLeft.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLeft.Size = new System.Drawing.Size(358, 196);
            this.dataGridViewLeft.TabIndex = 46;
            this.dataGridViewLeft.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLeft_CellClick);
            // 
            // gradientLabel9
            // 
            this.gradientLabel9.BackColor = System.Drawing.Color.White;
            this.gradientLabel9.ColorBottom = System.Drawing.Color.DarkGray;
            this.gradientLabel9.ColorTop = System.Drawing.Color.DimGray;
            this.gradientLabel9.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel9.ForeColor = System.Drawing.Color.White;
            this.gradientLabel9.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabel9.Location = new System.Drawing.Point(396, 34);
            this.gradientLabel9.Name = "gradientLabel9";
            this.gradientLabel9.Size = new System.Drawing.Size(191, 29);
            this.gradientLabel9.TabIndex = 45;
            this.gradientLabel9.Text = "Result 2";
            this.gradientLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabelResultLeft2
            // 
            this.gradientLabelResultLeft2.BackColor = System.Drawing.Color.DarkGreen;
            this.gradientLabelResultLeft2.ColorBottom = System.Drawing.Color.White;
            this.gradientLabelResultLeft2.ColorTop = System.Drawing.Color.White;
            this.gradientLabelResultLeft2.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientLabelResultLeft2.ForeColor = System.Drawing.Color.Lime;
            this.gradientLabelResultLeft2.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabelResultLeft2.Location = new System.Drawing.Point(396, 66);
            this.gradientLabelResultLeft2.Name = "gradientLabelResultLeft2";
            this.gradientLabelResultLeft2.Size = new System.Drawing.Size(191, 164);
            this.gradientLabelResultLeft2.TabIndex = 44;
            this.gradientLabelResultLeft2.Tag = "1";
            this.gradientLabelResultLeft2.Text = "-";
            this.gradientLabelResultLeft2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gradientLabelResultLeft2.DoubleClick += new System.EventHandler(this.gradientLabelLeftResult_DoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(122, 830);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 43);
            this.button1.TabIndex = 34;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnResultTest
            // 
            this.btnResultTest.Location = new System.Drawing.Point(0, 830);
            this.btnResultTest.Name = "btnResultTest";
            this.btnResultTest.Size = new System.Drawing.Size(121, 43);
            this.btnResultTest.TabIndex = 33;
            this.btnResultTest.Text = "Test";
            this.btnResultTest.UseVisualStyleBackColor = true;
            // 
            // gradientLabel7
            // 
            this.gradientLabel7.BackColor = System.Drawing.Color.White;
            this.gradientLabel7.ColorBottom = System.Drawing.Color.DarkGray;
            this.gradientLabel7.ColorTop = System.Drawing.Color.DimGray;
            this.gradientLabel7.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel7.ForeColor = System.Drawing.Color.White;
            this.gradientLabel7.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabel7.Location = new System.Drawing.Point(199, 34);
            this.gradientLabel7.Name = "gradientLabel7";
            this.gradientLabel7.Size = new System.Drawing.Size(191, 29);
            this.gradientLabel7.TabIndex = 31;
            this.gradientLabel7.Text = "Result 1";
            this.gradientLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabel4
            // 
            this.gradientLabel4.BackColor = System.Drawing.Color.White;
            this.gradientLabel4.ColorBottom = System.Drawing.Color.LightSlateGray;
            this.gradientLabel4.ColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(48)))));
            this.gradientLabel4.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel4.ForeColor = System.Drawing.Color.White;
            this.gradientLabel4.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel4.Location = new System.Drawing.Point(2, 1);
            this.gradientLabel4.Name = "gradientLabel4";
            this.gradientLabel4.Size = new System.Drawing.Size(950, 30);
            this.gradientLabel4.TabIndex = 29;
            this.gradientLabel4.Text = " Left Result";
            this.gradientLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gradientLabelResultLeft1
            // 
            this.gradientLabelResultLeft1.BackColor = System.Drawing.Color.DarkGreen;
            this.gradientLabelResultLeft1.ColorBottom = System.Drawing.Color.White;
            this.gradientLabelResultLeft1.ColorTop = System.Drawing.Color.White;
            this.gradientLabelResultLeft1.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientLabelResultLeft1.ForeColor = System.Drawing.Color.Lime;
            this.gradientLabelResultLeft1.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabelResultLeft1.Location = new System.Drawing.Point(199, 66);
            this.gradientLabelResultLeft1.Name = "gradientLabelResultLeft1";
            this.gradientLabelResultLeft1.Size = new System.Drawing.Size(191, 164);
            this.gradientLabelResultLeft1.TabIndex = 27;
            this.gradientLabelResultLeft1.Tag = "0";
            this.gradientLabelResultLeft1.Text = "-";
            this.gradientLabelResultLeft1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gradientLabelResultLeft1.DoubleClick += new System.EventHandler(this.gradientLabelLeftResult_DoubleClick);
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.panelRight.Controls.Add(this.gradientLabel6);
            this.panelRight.Controls.Add(this.gradientLabelResultRight3);
            this.panelRight.Controls.Add(this.gradientLabel1);
            this.panelRight.Controls.Add(this.gradientLabelResultRight2);
            this.panelRight.Controls.Add(this.gradientLabel3);
            this.panelRight.Controls.Add(this.gradientLabelResultRight1);
            this.panelRight.Controls.Add(this.dataGridViewRight);
            this.panelRight.Controls.Add(this.button2);
            this.panelRight.Controls.Add(this.button3);
            this.panelRight.Controls.Add(this.gradientLabel8);
            this.panelRight.Location = new System.Drawing.Point(955, 1);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(955, 234);
            this.panelRight.TabIndex = 45;
            // 
            // gradientLabel6
            // 
            this.gradientLabel6.BackColor = System.Drawing.Color.White;
            this.gradientLabel6.ColorBottom = System.Drawing.Color.DarkGray;
            this.gradientLabel6.ColorTop = System.Drawing.Color.DimGray;
            this.gradientLabel6.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel6.ForeColor = System.Drawing.Color.White;
            this.gradientLabel6.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabel6.Location = new System.Drawing.Point(396, 34);
            this.gradientLabel6.Name = "gradientLabel6";
            this.gradientLabel6.Size = new System.Drawing.Size(191, 29);
            this.gradientLabel6.TabIndex = 53;
            this.gradientLabel6.Text = "Result 3";
            this.gradientLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabelResultRight3
            // 
            this.gradientLabelResultRight3.BackColor = System.Drawing.Color.DarkGreen;
            this.gradientLabelResultRight3.ColorBottom = System.Drawing.Color.White;
            this.gradientLabelResultRight3.ColorTop = System.Drawing.Color.White;
            this.gradientLabelResultRight3.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientLabelResultRight3.ForeColor = System.Drawing.Color.Lime;
            this.gradientLabelResultRight3.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabelResultRight3.Location = new System.Drawing.Point(396, 66);
            this.gradientLabelResultRight3.Name = "gradientLabelResultRight3";
            this.gradientLabelResultRight3.Size = new System.Drawing.Size(191, 164);
            this.gradientLabelResultRight3.TabIndex = 52;
            this.gradientLabelResultRight3.Tag = "2";
            this.gradientLabelResultRight3.Text = "-";
            this.gradientLabelResultRight3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gradientLabelResultRight3.DoubleClick += new System.EventHandler(this.gradientLabelRightResult_DoubleClick);
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackColor = System.Drawing.Color.White;
            this.gradientLabel1.ColorBottom = System.Drawing.Color.DarkGray;
            this.gradientLabel1.ColorTop = System.Drawing.Color.DimGray;
            this.gradientLabel1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.ForeColor = System.Drawing.Color.White;
            this.gradientLabel1.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabel1.Location = new System.Drawing.Point(199, 34);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(191, 29);
            this.gradientLabel1.TabIndex = 51;
            this.gradientLabel1.Text = "Result 2";
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabelResultRight2
            // 
            this.gradientLabelResultRight2.BackColor = System.Drawing.Color.DarkGreen;
            this.gradientLabelResultRight2.ColorBottom = System.Drawing.Color.White;
            this.gradientLabelResultRight2.ColorTop = System.Drawing.Color.White;
            this.gradientLabelResultRight2.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientLabelResultRight2.ForeColor = System.Drawing.Color.Lime;
            this.gradientLabelResultRight2.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabelResultRight2.Location = new System.Drawing.Point(199, 66);
            this.gradientLabelResultRight2.Name = "gradientLabelResultRight2";
            this.gradientLabelResultRight2.Size = new System.Drawing.Size(191, 164);
            this.gradientLabelResultRight2.TabIndex = 50;
            this.gradientLabelResultRight2.Tag = "1";
            this.gradientLabelResultRight2.Text = "-";
            this.gradientLabelResultRight2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gradientLabelResultRight2.DoubleClick += new System.EventHandler(this.gradientLabelRightResult_DoubleClick);
            // 
            // gradientLabel3
            // 
            this.gradientLabel3.BackColor = System.Drawing.Color.White;
            this.gradientLabel3.ColorBottom = System.Drawing.Color.DarkGray;
            this.gradientLabel3.ColorTop = System.Drawing.Color.DimGray;
            this.gradientLabel3.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel3.ForeColor = System.Drawing.Color.White;
            this.gradientLabel3.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabel3.Location = new System.Drawing.Point(2, 34);
            this.gradientLabel3.Name = "gradientLabel3";
            this.gradientLabel3.Size = new System.Drawing.Size(191, 29);
            this.gradientLabel3.TabIndex = 49;
            this.gradientLabel3.Text = "Result 1";
            this.gradientLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradientLabelResultRight1
            // 
            this.gradientLabelResultRight1.BackColor = System.Drawing.Color.DarkGreen;
            this.gradientLabelResultRight1.ColorBottom = System.Drawing.Color.White;
            this.gradientLabelResultRight1.ColorTop = System.Drawing.Color.White;
            this.gradientLabelResultRight1.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientLabelResultRight1.ForeColor = System.Drawing.Color.Lime;
            this.gradientLabelResultRight1.GradientDirection = CustomControl.GradientLabel.Direction.Horizon;
            this.gradientLabelResultRight1.Location = new System.Drawing.Point(2, 66);
            this.gradientLabelResultRight1.Name = "gradientLabelResultRight1";
            this.gradientLabelResultRight1.Size = new System.Drawing.Size(191, 164);
            this.gradientLabelResultRight1.TabIndex = 48;
            this.gradientLabelResultRight1.Tag = "0";
            this.gradientLabelResultRight1.Text = "-";
            this.gradientLabelResultRight1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gradientLabelResultRight1.DoubleClick += new System.EventHandler(this.gradientLabelRightResult_DoubleClick);
            // 
            // dataGridViewRight
            // 
            this.dataGridViewRight.AllowUserToAddRows = false;
            this.dataGridViewRight.AllowUserToDeleteRows = false;
            this.dataGridViewRight.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewRight.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewRight.Location = new System.Drawing.Point(594, 34);
            this.dataGridViewRight.Name = "dataGridViewRight";
            this.dataGridViewRight.ReadOnly = true;
            this.dataGridViewRight.RowHeadersVisible = false;
            this.dataGridViewRight.RowTemplate.Height = 23;
            this.dataGridViewRight.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRight.Size = new System.Drawing.Size(358, 196);
            this.dataGridViewRight.TabIndex = 47;
            this.dataGridViewRight.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRight_CellClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(122, 830);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 43);
            this.button2.TabIndex = 34;
            this.button2.Text = "Test";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(0, 830);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 43);
            this.button3.TabIndex = 33;
            this.button3.Text = "Test";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // gradientLabel8
            // 
            this.gradientLabel8.BackColor = System.Drawing.Color.White;
            this.gradientLabel8.ColorBottom = System.Drawing.Color.LightSlateGray;
            this.gradientLabel8.ColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(48)))));
            this.gradientLabel8.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel8.ForeColor = System.Drawing.Color.White;
            this.gradientLabel8.GradientDirection = CustomControl.GradientLabel.Direction.Vertical;
            this.gradientLabel8.Location = new System.Drawing.Point(2, 1);
            this.gradientLabel8.Name = "gradientLabel8";
            this.gradientLabel8.Size = new System.Drawing.Size(950, 30);
            this.gradientLabel8.TabIndex = 29;
            this.gradientLabel8.Text = " Right Result";
            this.gradientLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucMainResultNavien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "ucMainResultNavien";
            this.Size = new System.Drawing.Size(1910, 236);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeft)).EndInit();
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnResultTest;
        private CustomControl.GradientLabel gradientLabel7;
        private CustomControl.GradientLabel gradientLabel4;
        private CustomControl.GradientLabel gradientLabelResultLeft1;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private CustomControl.GradientLabel gradientLabel8;
        private CustomControl.GradientLabel gradientLabel9;
        private CustomControl.GradientLabel gradientLabelResultLeft2;
        private System.Windows.Forms.DataGridView dataGridViewLeft;
        private System.Windows.Forms.DataGridView dataGridViewRight;
        private CustomControl.GradientLabel gradientLabel6;
        private CustomControl.GradientLabel gradientLabelResultRight3;
        private CustomControl.GradientLabel gradientLabel1;
        private CustomControl.GradientLabel gradientLabelResultRight2;
        private CustomControl.GradientLabel gradientLabel3;
        private CustomControl.GradientLabel gradientLabelResultRight1;
        private System.Windows.Forms.TextBox textBoxBarcode;
        private System.Windows.Forms.Label label1;
    }
}

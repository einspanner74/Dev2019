namespace KPVisionInspectionFramework
{
    partial class FolderPathWindow
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
            this.btnConfirm = new System.Windows.Forms.Button();
            this.textBoxInDataPath = new System.Windows.Forms.TextBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelTrainImage = new System.Windows.Forms.Label();
            this.btnIndataPath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfirm.Location = new System.Drawing.Point(587, 68);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(129, 33);
            this.btnConfirm.TabIndex = 23;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // textBoxInDataPath
            // 
            this.textBoxInDataPath.Enabled = false;
            this.textBoxInDataPath.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxInDataPath.Location = new System.Drawing.Point(119, 37);
            this.textBoxInDataPath.Name = "textBoxInDataPath";
            this.textBoxInDataPath.Size = new System.Drawing.Size(564, 26);
            this.textBoxInDataPath.TabIndex = 21;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.SlateGray;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(723, 30);
            this.labelTitle.TabIndex = 20;
            this.labelTitle.Text = " New Name";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseDown);
            this.labelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseMove);
            // 
            // labelTrainImage
            // 
            this.labelTrainImage.BackColor = System.Drawing.Color.Black;
            this.labelTrainImage.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTrainImage.ForeColor = System.Drawing.Color.White;
            this.labelTrainImage.Location = new System.Drawing.Point(6, 37);
            this.labelTrainImage.Name = "labelTrainImage";
            this.labelTrainImage.Size = new System.Drawing.Size(107, 26);
            this.labelTrainImage.TabIndex = 24;
            this.labelTrainImage.Text = "Folder Path";
            this.labelTrainImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnIndataPath
            // 
            this.btnIndataPath.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnIndataPath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIndataPath.Location = new System.Drawing.Point(685, 37);
            this.btnIndataPath.Name = "btnIndataPath";
            this.btnIndataPath.Size = new System.Drawing.Size(31, 24);
            this.btnIndataPath.TabIndex = 25;
            this.btnIndataPath.Tag = "0";
            this.btnIndataPath.Text = "...";
            this.btnIndataPath.UseVisualStyleBackColor = true;
            this.btnIndataPath.Click += new System.EventHandler(this.btnSearchDataPath_Click);
            // 
            // FolderPathWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.ClientSize = new System.Drawing.Size(723, 105);
            this.Controls.Add(this.btnIndataPath);
            this.Controls.Add(this.labelTrainImage);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.textBoxInDataPath);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FolderPathWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RecipeNewName";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TextBox textBoxInDataPath;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelTrainImage;
        private System.Windows.Forms.Button btnIndataPath;
    }
}
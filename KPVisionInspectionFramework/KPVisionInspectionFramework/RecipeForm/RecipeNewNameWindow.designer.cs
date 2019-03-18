namespace KPVisionInspectionFramework
{
    partial class RecipeNewNameWindow
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
            this.btnRecipeCancel = new System.Windows.Forms.Button();
            this.btnRecipeConfirm = new System.Windows.Forms.Button();
            this.textBoxCurrentRecipe = new System.Windows.Forms.TextBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxNewRecipe = new System.Windows.Forms.TextBox();
            this.labelTrainImage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNewRecipeSub = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnRecipeCancel
            // 
            this.btnRecipeCancel.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRecipeCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecipeCancel.Location = new System.Drawing.Point(218, 117);
            this.btnRecipeCancel.Name = "btnRecipeCancel";
            this.btnRecipeCancel.Size = new System.Drawing.Size(111, 36);
            this.btnRecipeCancel.TabIndex = 22;
            this.btnRecipeCancel.Text = "Cancel";
            this.btnRecipeCancel.UseVisualStyleBackColor = true;
            this.btnRecipeCancel.Click += new System.EventHandler(this.btnRecipeCancel_Click);
            // 
            // btnRecipeConfirm
            // 
            this.btnRecipeConfirm.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRecipeConfirm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecipeConfirm.Location = new System.Drawing.Point(101, 117);
            this.btnRecipeConfirm.Name = "btnRecipeConfirm";
            this.btnRecipeConfirm.Size = new System.Drawing.Size(111, 36);
            this.btnRecipeConfirm.TabIndex = 23;
            this.btnRecipeConfirm.Text = "Confirm";
            this.btnRecipeConfirm.UseVisualStyleBackColor = true;
            this.btnRecipeConfirm.Click += new System.EventHandler(this.btnRecipeConfirm_Click);
            // 
            // textBoxCurrentRecipe
            // 
            this.textBoxCurrentRecipe.Enabled = false;
            this.textBoxCurrentRecipe.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxCurrentRecipe.Location = new System.Drawing.Point(102, 40);
            this.textBoxCurrentRecipe.Name = "textBoxCurrentRecipe";
            this.textBoxCurrentRecipe.Size = new System.Drawing.Size(227, 26);
            this.textBoxCurrentRecipe.TabIndex = 21;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.SteelBlue;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(334, 33);
            this.labelTitle.TabIndex = 20;
            this.labelTitle.Text = " New Name";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseDown);
            this.labelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseMove);
            // 
            // textBoxNewRecipe
            // 
            this.textBoxNewRecipe.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxNewRecipe.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxNewRecipe.Location = new System.Drawing.Point(102, 77);
            this.textBoxNewRecipe.Name = "textBoxNewRecipe";
            this.textBoxNewRecipe.Size = new System.Drawing.Size(63, 26);
            this.textBoxNewRecipe.TabIndex = 21;
            this.textBoxNewRecipe.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNewRecipe_KeyPress);
            // 
            // labelTrainImage
            // 
            this.labelTrainImage.BackColor = System.Drawing.Color.Black;
            this.labelTrainImage.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTrainImage.ForeColor = System.Drawing.Color.White;
            this.labelTrainImage.Location = new System.Drawing.Point(5, 40);
            this.labelTrainImage.Name = "labelTrainImage";
            this.labelTrainImage.Size = new System.Drawing.Size(92, 28);
            this.labelTrainImage.TabIndex = 24;
            this.labelTrainImage.Text = "Selected Recipe";
            this.labelTrainImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 28);
            this.label1.TabIndex = 24;
            this.label1.Text = "New Recipe";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(166, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 19);
            this.label2.TabIndex = 25;
            this.label2.Text = "_";
            // 
            // textBoxNewRecipeSub
            // 
            this.textBoxNewRecipeSub.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNewRecipeSub.Location = new System.Drawing.Point(181, 77);
            this.textBoxNewRecipeSub.Name = "textBoxNewRecipeSub";
            this.textBoxNewRecipeSub.Size = new System.Drawing.Size(148, 26);
            this.textBoxNewRecipeSub.TabIndex = 26;
            // 
            // RecipeNewNameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(80)))), ((int)(((byte)(120)))));
            this.ClientSize = new System.Drawing.Size(334, 164);
            this.Controls.Add(this.textBoxNewRecipeSub);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTrainImage);
            this.Controls.Add(this.btnRecipeCancel);
            this.Controls.Add(this.btnRecipeConfirm);
            this.Controls.Add(this.textBoxNewRecipe);
            this.Controls.Add(this.textBoxCurrentRecipe);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RecipeNewNameWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RecipeNewName";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRecipeCancel;
        private System.Windows.Forms.Button btnRecipeConfirm;
        private System.Windows.Forms.TextBox textBoxCurrentRecipe;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxNewRecipe;
        private System.Windows.Forms.Label labelTrainImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxNewRecipeSub;
    }
}
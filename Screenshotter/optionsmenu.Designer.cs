namespace WindowsFormsApplication1
{
    partial class optionsmenu
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
            this.textBox_prefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_savefolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_viewfolder = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_subfolder = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox_prefix
            // 
            this.textBox_prefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_prefix.Location = new System.Drawing.Point(149, 13);
            this.textBox_prefix.Name = "textBox_prefix";
            this.textBox_prefix.Size = new System.Drawing.Size(350, 20);
            this.textBox_prefix.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Add filename prefix:";
            // 
            // textBox_savefolder
            // 
            this.textBox_savefolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_savefolder.Location = new System.Drawing.Point(149, 53);
            this.textBox_savefolder.Name = "textBox_savefolder";
            this.textBox_savefolder.Size = new System.Drawing.Size(295, 20);
            this.textBox_savefolder.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Save images to this folder:";
            // 
            // button_viewfolder
            // 
            this.button_viewfolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_viewfolder.Location = new System.Drawing.Point(450, 51);
            this.button_viewfolder.Name = "button_viewfolder";
            this.button_viewfolder.Size = new System.Drawing.Size(49, 23);
            this.button_viewfolder.TabIndex = 4;
            this.button_viewfolder.Text = "View";
            this.button_viewfolder.UseVisualStyleBackColor = true;
            this.button_viewfolder.Click += new System.EventHandler(this.button_viewfolder_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(12, 118);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(486, 46);
            this.button2.TabIndex = 5;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox_subfolder
            // 
            this.checkBox_subfolder.AutoSize = true;
            this.checkBox_subfolder.Location = new System.Drawing.Point(149, 80);
            this.checkBox_subfolder.Name = "checkBox_subfolder";
            this.checkBox_subfolder.Size = new System.Drawing.Size(212, 17);
            this.checkBox_subfolder.TabIndex = 6;
            this.checkBox_subfolder.Text = "Auto-create sub-folder with today\'s date";
            this.checkBox_subfolder.UseVisualStyleBackColor = true;
            // 
            // optionsmenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 176);
            this.Controls.Add(this.checkBox_subfolder);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_viewfolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_savefolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_prefix);
            this.Name = "optionsmenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_prefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_savefolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_viewfolder;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_subfolder;
    }
}
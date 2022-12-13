namespace Apollo_Soundboard.Forms
{
    partial class AudioConverter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioConverter));
            this.label1 = new System.Windows.Forms.Label();
            this.InputFileBox = new System.Windows.Forms.TextBox();
            this.BrowseConvertFile = new System.Windows.Forms.Button();
            this.SaveFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input File";
            // 
            // InputFileBox
            // 
            this.InputFileBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.InputFileBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputFileBox.ForeColor = System.Drawing.SystemColors.Control;
            this.InputFileBox.Location = new System.Drawing.Point(12, 36);
            this.InputFileBox.Name = "InputFileBox";
            this.InputFileBox.Size = new System.Drawing.Size(326, 27);
            this.InputFileBox.TabIndex = 1;
            // 
            // BrowseConvertFile
            // 
            this.BrowseConvertFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.BrowseConvertFile.FlatAppearance.BorderSize = 0;
            this.BrowseConvertFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseConvertFile.ForeColor = System.Drawing.SystemColors.Control;
            this.BrowseConvertFile.Location = new System.Drawing.Point(344, 36);
            this.BrowseConvertFile.Name = "BrowseConvertFile";
            this.BrowseConvertFile.Size = new System.Drawing.Size(112, 27);
            this.BrowseConvertFile.TabIndex = 2;
            this.BrowseConvertFile.Text = "Browse";
            this.BrowseConvertFile.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BrowseConvertFile.UseVisualStyleBackColor = false;
            this.BrowseConvertFile.Click += new System.EventHandler(this.BrowseConvertFile_Click);
            // 
            // SaveFile
            // 
            this.SaveFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.SaveFile.FlatAppearance.BorderSize = 0;
            this.SaveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveFile.ForeColor = System.Drawing.SystemColors.Control;
            this.SaveFile.Location = new System.Drawing.Point(12, 72);
            this.SaveFile.Name = "SaveFile";
            this.SaveFile.Size = new System.Drawing.Size(444, 60);
            this.SaveFile.TabIndex = 5;
            this.SaveFile.Text = "Export";
            this.SaveFile.UseVisualStyleBackColor = false;
            this.SaveFile.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // AudioConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(464, 144);
            this.Controls.Add(this.SaveFile);
            this.Controls.Add(this.BrowseConvertFile);
            this.Controls.Add(this.InputFileBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(482, 191);
            this.MinimumSize = new System.Drawing.Size(482, 191);
            this.Name = "AudioConverter";
            this.Text = "AudioConverter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox InputFileBox;
        private Button BrowseConvertFile;
        private Button SaveFile;
    }
}
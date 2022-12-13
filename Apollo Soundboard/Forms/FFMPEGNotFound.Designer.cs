namespace NUT_Soundboard.Forms
{
    partial class FFMPEGNotFound
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFMPEGNotFound));
            this.label1 = new System.Windows.Forms.Label();
            this.Yes = new System.Windows.Forms.Button();
            this.No = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(8, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(375, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "FFMPEG was not found! Would you like to download it?";
            // 
            // Yes
            // 
            this.Yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Yes.FlatAppearance.BorderSize = 0;
            this.Yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Yes.ForeColor = System.Drawing.SystemColors.Control;
            this.Yes.Location = new System.Drawing.Point(203, 62);
            this.Yes.Name = "Yes";
            this.Yes.Size = new System.Drawing.Size(175, 66);
            this.Yes.TabIndex = 1;
            this.Yes.Text = "Yes";
            this.Yes.UseVisualStyleBackColor = false;
            this.Yes.Click += new System.EventHandler(this.Yes_Click);
            // 
            // No
            // 
            this.No.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.No.FlatAppearance.BorderSize = 0;
            this.No.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.No.ForeColor = System.Drawing.SystemColors.Control;
            this.No.Location = new System.Drawing.Point(12, 62);
            this.No.Name = "No";
            this.No.Size = new System.Drawing.Size(175, 66);
            this.No.TabIndex = 2;
            this.No.Text = "No";
            this.No.UseVisualStyleBackColor = false;
            this.No.Click += new System.EventHandler(this.No_Click);
            // 
            // FFMPEGNotFound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(390, 147);
            this.Controls.Add(this.No);
            this.Controls.Add(this.Yes);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(408, 194);
            this.MinimumSize = new System.Drawing.Size(408, 194);
            this.Name = "FFMPEGNotFound";
            this.Text = "Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Button Yes;
        private Button No;
    }
}
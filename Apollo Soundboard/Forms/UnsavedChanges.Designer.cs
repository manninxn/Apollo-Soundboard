namespace Apollo_Soundboard
{
    partial class UnsavedChanges
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnsavedChanges));
            this.label1 = new System.Windows.Forms.Label();
            this.NoSave = new System.Windows.Forms.Button();
            this.YesSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Would you like to save?";
            // 
            // NoSave
            // 
            this.NoSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.NoSave.FlatAppearance.BorderSize = 0;
            this.NoSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NoSave.ForeColor = System.Drawing.SystemColors.Control;
            this.NoSave.Location = new System.Drawing.Point(12, 58);
            this.NoSave.Name = "NoSave";
            this.NoSave.Size = new System.Drawing.Size(263, 44);
            this.NoSave.TabIndex = 1;
            this.NoSave.Text = "No";
            this.NoSave.UseVisualStyleBackColor = false;
            this.NoSave.Click += new System.EventHandler(this.NoSave_Click);
            // 
            // YesSave
            // 
            this.YesSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.YesSave.FlatAppearance.BorderSize = 0;
            this.YesSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.YesSave.ForeColor = System.Drawing.SystemColors.Control;
            this.YesSave.Location = new System.Drawing.Point(297, 58);
            this.YesSave.Name = "YesSave";
            this.YesSave.Size = new System.Drawing.Size(250, 44);
            this.YesSave.TabIndex = 2;
            this.YesSave.Text = "Yes";
            this.YesSave.UseVisualStyleBackColor = false;
            this.YesSave.Click += new System.EventHandler(this.YesSave_Click);
            // 
            // UnsavedChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(559, 114);
            this.Controls.Add(this.YesSave);
            this.Controls.Add(this.NoSave);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UnsavedChanges";
            this.Text = "You have unsaved changes!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Button NoSave;
        private Button YesSave;
    }
}
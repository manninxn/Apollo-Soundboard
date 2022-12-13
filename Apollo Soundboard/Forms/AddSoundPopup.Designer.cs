namespace Apollo_Soundboard
{
    partial class AddSoundPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddSoundPopup));
            this.HotkeySelectorButton = new Apollo_Soundboard.HotkeySelector();
            this.ConfirmAdd = new System.Windows.Forms.Button();
            this.Browse = new System.Windows.Forms.Button();
            this.FilePathBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HotkeySelectorButton
            // 
            this.HotkeySelectorButton.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.HotkeySelectorButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.HotkeySelectorButton.InactiveColor = System.Drawing.SystemColors.ActiveCaption;
            this.HotkeySelectorButton.isActive = false;
            this.HotkeySelectorButton.Location = new System.Drawing.Point(14, 132);
            this.HotkeySelectorButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.HotkeySelectorButton.MultiKey = true;
            this.HotkeySelectorButton.Name = "HotkeySelectorButton";
            this.HotkeySelectorButton.Size = new System.Drawing.Size(527, 31);
            this.HotkeySelectorButton.TabIndex = 0;
            this.HotkeySelectorButton.Text = "Click to select hotkey";
            this.HotkeySelectorButton.UseVisualStyleBackColor = false;
            // 
            // ConfirmAdd
            // 
            this.ConfirmAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.ConfirmAdd.FlatAppearance.BorderSize = 0;
            this.ConfirmAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConfirmAdd.ForeColor = System.Drawing.SystemColors.Control;
            this.ConfirmAdd.Location = new System.Drawing.Point(455, 171);
            this.ConfirmAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ConfirmAdd.Name = "ConfirmAdd";
            this.ConfirmAdd.Size = new System.Drawing.Size(86, 31);
            this.ConfirmAdd.TabIndex = 1;
            this.ConfirmAdd.Text = "Done";
            this.ConfirmAdd.UseVisualStyleBackColor = false;
            this.ConfirmAdd.Click += new System.EventHandler(this.ConfirmAdd_Click);
            // 
            // Browse
            // 
            this.Browse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Browse.FlatAppearance.BorderSize = 0;
            this.Browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Browse.ForeColor = System.Drawing.SystemColors.Control;
            this.Browse.Location = new System.Drawing.Point(14, 79);
            this.Browse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(86, 31);
            this.Browse.TabIndex = 2;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = false;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // FilePathBox
            // 
            this.FilePathBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.FilePathBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilePathBox.ForeColor = System.Drawing.SystemColors.Control;
            this.FilePathBox.Location = new System.Drawing.Point(14, 40);
            this.FilePathBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FilePathBox.Name = "FilePathBox";
            this.FilePathBox.Size = new System.Drawing.Size(526, 27);
            this.FilePathBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "File Path";
            // 
            // AddSoundPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(554, 217);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FilePathBox);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.ConfirmAdd);
            this.Controls.Add(this.HotkeySelectorButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(572, 264);
            this.MinimumSize = new System.Drawing.Size(572, 264);
            this.Name = "AddSoundPopup";
            this.Text = "Add Sound";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HotkeySelector HotkeySelectorButton;
        private Button ConfirmAdd;
        private Button Browse;
        private TextBox FilePathBox;
        private Label label1;
    }
}
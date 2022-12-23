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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GainBar = new Apollo_Soundboard.NoFocusTrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.HotkeyOrderMattersCheckbox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.FileNameBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.GainBar)).BeginInit();
            this.SuspendLayout();
            // 
            // HotkeySelectorButton
            // 
            this.HotkeySelectorButton.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.HotkeySelectorButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.HotkeySelectorButton.InactiveColor = System.Drawing.SystemColors.ActiveCaption;
            this.HotkeySelectorButton.isActive = false;
            this.HotkeySelectorButton.Location = new System.Drawing.Point(106, 79);
            this.HotkeySelectorButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.HotkeySelectorButton.MultiKey = true;
            this.HotkeySelectorButton.Name = "HotkeySelectorButton";
            this.HotkeySelectorButton.Size = new System.Drawing.Size(434, 31);
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
            this.ConfirmAdd.Location = new System.Drawing.Point(456, 275);
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
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "File Path";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(497, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "100%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(269, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(14, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "-100%";
            // 
            // GainBar
            // 
            this.GainBar.LargeChange = 10;
            this.GainBar.Location = new System.Drawing.Point(14, 212);
            this.GainBar.Maximum = 100;
            this.GainBar.Minimum = -100;
            this.GainBar.Name = "GainBar";
            this.GainBar.Size = new System.Drawing.Size(526, 56);
            this.GainBar.TabIndex = 11;
            this.GainBar.TabStop = false;
            this.GainBar.TickFrequency = 10;
            this.GainBar.ValueChanged += new System.EventHandler(this.GainBar_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(14, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Gain";
            // 
            // HotkeyOrderMattersCheckbox
            // 
            this.HotkeyOrderMattersCheckbox.AutoSize = true;
            this.HotkeyOrderMattersCheckbox.ForeColor = System.Drawing.SystemColors.Control;
            this.HotkeyOrderMattersCheckbox.Location = new System.Drawing.Point(12, 275);
            this.HotkeyOrderMattersCheckbox.Name = "HotkeyOrderMattersCheckbox";
            this.HotkeyOrderMattersCheckbox.Size = new System.Drawing.Size(174, 24);
            this.HotkeyOrderMattersCheckbox.TabIndex = 15;
            this.HotkeyOrderMattersCheckbox.Text = "Hotkey Order Matters";
            this.HotkeyOrderMattersCheckbox.UseVisualStyleBackColor = true;
            this.HotkeyOrderMattersCheckbox.CheckedChanged += new System.EventHandler(this.HotkeyOrderMatters_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(14, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Sound Name";
            // 
            // FileNameBox
            // 
            this.FileNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.FileNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FileNameBox.ForeColor = System.Drawing.SystemColors.Control;
            this.FileNameBox.Location = new System.Drawing.Point(16, 148);
            this.FileNameBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FileNameBox.Name = "FileNameBox";
            this.FileNameBox.Size = new System.Drawing.Size(526, 27);
            this.FileNameBox.TabIndex = 17;
            // 
            // AddSoundPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(554, 319);
            this.Controls.Add(this.FileNameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.HotkeyOrderMattersCheckbox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.GainBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FilePathBox);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.ConfirmAdd);
            this.Controls.Add(this.HotkeySelectorButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(572, 366);
            this.MinimumSize = new System.Drawing.Size(572, 366);
            this.Name = "AddSoundPopup";
            this.Text = "Add Sound";
            ((System.ComponentModel.ISupportInitialize)(this.GainBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HotkeySelector HotkeySelectorButton;
        private Button ConfirmAdd;
        private Button Browse;
        private TextBox FilePathBox;
        private Label label1;
        private Label label6;
        private Label label5;
        private Label label4;
        private NoFocusTrackBar GainBar;
        private Label label2;
        private CheckBox HotkeyOrderMattersCheckbox;
        private Label label3;
        private TextBox FileNameBox;
    }
}
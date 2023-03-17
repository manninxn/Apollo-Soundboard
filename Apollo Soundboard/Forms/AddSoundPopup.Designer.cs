namespace Apollo.Forms
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
            this.HotkeySelectorButton = new Apollo.HotkeySelector();
            this.ConfirmAdd = new System.Windows.Forms.Button();
            this.Browse = new System.Windows.Forms.Button();
            this.FilePathBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GainBar = new Apollo.Forms.NoFocusTrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.HotkeyOrderMattersCheckbox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.FileNameBox = new System.Windows.Forms.TextBox();
            this.OverlapSelfCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.GainBar)).BeginInit();
            this.SuspendLayout();
            // 
            // HotkeySelectorButton
            // 
            this.HotkeySelectorButton.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.HotkeySelectorButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.HotkeySelectorButton.InactiveColor = System.Drawing.SystemColors.ActiveCaption;
            this.HotkeySelectorButton.isActive = false;
            this.HotkeySelectorButton.Location = new System.Drawing.Point(93, 59);
            this.HotkeySelectorButton.MultiKey = true;
            this.HotkeySelectorButton.Name = "HotkeySelectorButton";
            this.HotkeySelectorButton.SelectedHotkeys = ((System.Collections.Generic.List<System.Windows.Forms.Keys>)(resources.GetObject("HotkeySelectorButton.SelectedHotkeys")));
            this.HotkeySelectorButton.Size = new System.Drawing.Size(380, 23);
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
            this.ConfirmAdd.Location = new System.Drawing.Point(399, 206);
            this.ConfirmAdd.Name = "ConfirmAdd";
            this.ConfirmAdd.Size = new System.Drawing.Size(75, 23);
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
            this.Browse.Location = new System.Drawing.Point(12, 59);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
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
            this.FilePathBox.Location = new System.Drawing.Point(12, 30);
            this.FilePathBox.Name = "FilePathBox";
            this.FilePathBox.Size = new System.Drawing.Size(460, 23);
            this.FilePathBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "File Path";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(435, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "100%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(235, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(12, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "-100%";
            // 
            // GainBar
            // 
            this.GainBar.LargeChange = 10;
            this.GainBar.Location = new System.Drawing.Point(12, 159);
            this.GainBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GainBar.Maximum = 100;
            this.GainBar.Minimum = -100;
            this.GainBar.Name = "GainBar";
            this.GainBar.Size = new System.Drawing.Size(460, 45);
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
            this.label2.Location = new System.Drawing.Point(12, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Gain";
            // 
            // HotkeyOrderMattersCheckbox
            // 
            this.HotkeyOrderMattersCheckbox.AutoSize = true;
            this.HotkeyOrderMattersCheckbox.ForeColor = System.Drawing.SystemColors.Control;
            this.HotkeyOrderMattersCheckbox.Location = new System.Drawing.Point(10, 206);
            this.HotkeyOrderMattersCheckbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HotkeyOrderMattersCheckbox.Name = "HotkeyOrderMattersCheckbox";
            this.HotkeyOrderMattersCheckbox.Size = new System.Drawing.Size(140, 19);
            this.HotkeyOrderMattersCheckbox.TabIndex = 15;
            this.HotkeyOrderMattersCheckbox.Text = "Hotkey Order Matters";
            this.HotkeyOrderMattersCheckbox.UseVisualStyleBackColor = true;
            this.HotkeyOrderMattersCheckbox.CheckedChanged += new System.EventHandler(this.HotkeyOrderMatters_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Sound Name";
            // 
            // FileNameBox
            // 
            this.FileNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.FileNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FileNameBox.ForeColor = System.Drawing.SystemColors.Control;
            this.FileNameBox.Location = new System.Drawing.Point(14, 111);
            this.FileNameBox.Name = "FileNameBox";
            this.FileNameBox.Size = new System.Drawing.Size(460, 23);
            this.FileNameBox.TabIndex = 17;
            // 
            // OverlapSelfCheckbox
            // 
            this.OverlapSelfCheckbox.AutoSize = true;
            this.OverlapSelfCheckbox.Checked = true;
            this.OverlapSelfCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OverlapSelfCheckbox.ForeColor = System.Drawing.SystemColors.Control;
            this.OverlapSelfCheckbox.Location = new System.Drawing.Point(156, 206);
            this.OverlapSelfCheckbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OverlapSelfCheckbox.Name = "OverlapSelfCheckbox";
            this.OverlapSelfCheckbox.Size = new System.Drawing.Size(89, 19);
            this.OverlapSelfCheckbox.TabIndex = 18;
            this.OverlapSelfCheckbox.Text = "Overlap Self";
            this.OverlapSelfCheckbox.UseVisualStyleBackColor = true;
            this.OverlapSelfCheckbox.CheckedChanged += new System.EventHandler(this.OverlapSelfCheckbox_CheckedChanged);
            // 
            // AddSoundPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(486, 245);
            this.Controls.Add(this.OverlapSelfCheckbox);
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
            this.MaximumSize = new System.Drawing.Size(502, 284);
            this.MinimumSize = new System.Drawing.Size(502, 284);
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
        private CheckBox OverlapSelfCheckbox;
    }
}
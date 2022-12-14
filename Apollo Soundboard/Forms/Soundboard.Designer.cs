using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Apollo_Soundboard
{
    public class FlatCombo : ComboBox
    {
        
        private const int WM_PAINT = 0xF;
        private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
        Color borderColor = Color.Blue;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }
        public override Color ForeColor => SystemColors.Control;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT && DropDownStyle != ComboBoxStyle.Simple)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    using (var p = new Pen(BorderColor))
                    {
                        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);

                        var d = FlatStyle == FlatStyle.Popup ? 1 : 0;
                        g.DrawLine(p, Width - buttonWidth - d,
                            0, Width - buttonWidth - d, Height);
                    }
                }
            }
        }
    }


    public class CustomToolstrip : ToolStripProfessionalRenderer
    {
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var btn = e.Item as ToolStripButton;
            if (btn != null && btn.CheckOnClick && btn.Checked)
            {
                Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
                e.Graphics.FillRectangle(Brushes.Black, bounds);
            }
            else base.OnRenderButtonBackground(e);
        }
    }

    partial class Soundboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Soundboard));
            this.PrimaryOutputComboBox = new Apollo_Soundboard.FlatCombo();
            this.SoundGrid = new System.Windows.Forms.DataGridView();
            this.AddSoundButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SecondaryOutputComboBox = new Apollo_Soundboard.FlatCombo();
            this.MicInjectorToggle = new System.Windows.Forms.CheckBox();
            this.RemoveSound = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Import = new System.Windows.Forms.ToolStripMenuItem();
            this.eXPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeMixerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StopAll = new System.Windows.Forms.Button();
            this.StopAllHotkeySelector = new Apollo_Soundboard.HotkeySelector();
            this.EditButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MicrophoneSelectComboBox = new Apollo_Soundboard.FlatCombo();
            this.soundpadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.SoundGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PrimaryOutputComboBox
            // 
            this.PrimaryOutputComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.PrimaryOutputComboBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.PrimaryOutputComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrimaryOutputComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrimaryOutputComboBox.ForeColor = System.Drawing.SystemColors.Control;
            this.PrimaryOutputComboBox.FormattingEnabled = true;
            this.PrimaryOutputComboBox.Location = new System.Drawing.Point(7, 726);
            this.PrimaryOutputComboBox.Name = "PrimaryOutputComboBox";
            this.PrimaryOutputComboBox.Size = new System.Drawing.Size(601, 28);
            this.PrimaryOutputComboBox.TabIndex = 1;
            this.PrimaryOutputComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.PrimaryOutputComboBox_DrawItem);
            this.PrimaryOutputComboBox.SelectedIndexChanged += new System.EventHandler(this.PrimaryOutputComboBox_SelectedIndexChanged);
            // 
            // SoundGrid
            // 
            this.SoundGrid.AllowUserToAddRows = false;
            this.SoundGrid.AllowUserToDeleteRows = false;
            this.SoundGrid.AllowUserToResizeRows = false;
            this.SoundGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.SoundGrid.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.SoundGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SoundGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SoundGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.SoundGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SoundGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.SoundGrid.EnableHeadersVisualStyles = false;
            this.SoundGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.SoundGrid.Location = new System.Drawing.Point(7, 31);
            this.SoundGrid.MultiSelect = false;
            this.SoundGrid.Name = "SoundGrid";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SoundGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.SoundGrid.RowHeadersVisible = false;
            this.SoundGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.SoundGrid.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.SoundGrid.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.Control;
            this.SoundGrid.RowTemplate.Height = 29;
            this.SoundGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.SoundGrid.Size = new System.Drawing.Size(601, 591);
            this.SoundGrid.TabIndex = 2;
            this.SoundGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SoundGrid_CellDoubleClick);
            // 
            // AddSoundButton
            // 
            this.AddSoundButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.AddSoundButton.FlatAppearance.BorderSize = 0;
            this.AddSoundButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddSoundButton.ForeColor = System.Drawing.SystemColors.Control;
            this.AddSoundButton.Location = new System.Drawing.Point(7, 628);
            this.AddSoundButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddSoundButton.Name = "AddSoundButton";
            this.AddSoundButton.Size = new System.Drawing.Size(102, 62);
            this.AddSoundButton.TabIndex = 3;
            this.AddSoundButton.Text = "Add";
            this.AddSoundButton.UseVisualStyleBackColor = false;
            this.AddSoundButton.Click += new System.EventHandler(this.AddSoundButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(7, 694);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Main Output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(5, 757);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Virtual Audio Cable";
            // 
            // SecondaryOutputComboBox
            // 
            this.SecondaryOutputComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.SecondaryOutputComboBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.SecondaryOutputComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecondaryOutputComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SecondaryOutputComboBox.ForeColor = System.Drawing.SystemColors.Control;
            this.SecondaryOutputComboBox.FormattingEnabled = true;
            this.SecondaryOutputComboBox.Location = new System.Drawing.Point(6, 780);
            this.SecondaryOutputComboBox.Name = "SecondaryOutputComboBox";
            this.SecondaryOutputComboBox.Size = new System.Drawing.Size(601, 28);
            this.SecondaryOutputComboBox.TabIndex = 6;
            this.SecondaryOutputComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.SecondaryOutputComboBox_DrawItem);
            this.SecondaryOutputComboBox.SelectedIndexChanged += new System.EventHandler(this.SecondaryOutputComboBox_SelectedIndexChanged);
            // 
            // MicInjectorToggle
            // 
            this.MicInjectorToggle.AutoSize = true;
            this.MicInjectorToggle.ForeColor = System.Drawing.SystemColors.Control;
            this.MicInjectorToggle.Location = new System.Drawing.Point(499, 628);
            this.MicInjectorToggle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MicInjectorToggle.Name = "MicInjectorToggle";
            this.MicInjectorToggle.Size = new System.Drawing.Size(109, 24);
            this.MicInjectorToggle.TabIndex = 8;
            this.MicInjectorToggle.Text = "Mic Injector";
            this.MicInjectorToggle.UseVisualStyleBackColor = true;
            this.MicInjectorToggle.CheckedChanged += new System.EventHandler(this.MicInjectorToggle_CheckChanged);
            // 
            // RemoveSound
            // 
            this.RemoveSound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.RemoveSound.FlatAppearance.BorderSize = 0;
            this.RemoveSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveSound.ForeColor = System.Drawing.SystemColors.Control;
            this.RemoveSound.Location = new System.Drawing.Point(115, 628);
            this.RemoveSound.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RemoveSound.Name = "RemoveSound";
            this.RemoveSound.Size = new System.Drawing.Size(102, 62);
            this.RemoveSound.TabIndex = 9;
            this.RemoveSound.Text = "Remove";
            this.RemoveSound.UseVisualStyleBackColor = false;
            this.RemoveSound.Click += new System.EventHandler(this.RemoveSound_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.volumeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(619, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.Import,
            this.saveToolStripMenuItem,
            this.bToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.newToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.loadToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // Import
            // 
            this.Import.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Import.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eXPToolStripMenuItem,
            this.soundpadToolStripMenuItem});
            this.Import.ForeColor = System.Drawing.SystemColors.Control;
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(224, 26);
            this.Import.Text = "Import";
            // 
            // eXPToolStripMenuItem
            // 
            this.eXPToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.eXPToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.eXPToolStripMenuItem.Name = "eXPToolStripMenuItem";
            this.eXPToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.eXPToolStripMenuItem.Text = "EXP";
            this.eXPToolStripMenuItem.Click += new System.EventHandler(this.eXPToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.saveToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // bToolStripMenuItem
            // 
            this.bToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.bToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.bToolStripMenuItem.Name = "bToolStripMenuItem";
            this.bToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.bToolStripMenuItem.Text = "Save As";
            this.bToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.quitToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.volumeToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.volumeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.volumeMixerToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.volumeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.volumeToolStripMenuItem.Name = "volumeToolStripMenuItem";
            this.volumeToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.volumeToolStripMenuItem.Text = "Options";
            // 
            // volumeMixerToolStripMenuItem
            // 
            this.volumeMixerToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.volumeMixerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.volumeMixerToolStripMenuItem.Name = "volumeMixerToolStripMenuItem";
            this.volumeMixerToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.volumeMixerToolStripMenuItem.Text = "Volume Mixer";
            this.volumeMixerToolStripMenuItem.Click += new System.EventHandler(this.volumeMixerToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.optionsToolStripMenuItem.Text = "Audio Converter";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.audioConverterToolStripMenuItem_Click);
            // 
            // StopAll
            // 
            this.StopAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.StopAll.FlatAppearance.BorderSize = 0;
            this.StopAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopAll.ForeColor = System.Drawing.SystemColors.Control;
            this.StopAll.Location = new System.Drawing.Point(331, 628);
            this.StopAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StopAll.Name = "StopAll";
            this.StopAll.Size = new System.Drawing.Size(135, 31);
            this.StopAll.TabIndex = 11;
            this.StopAll.Text = "Stop All";
            this.StopAll.UseVisualStyleBackColor = false;
            this.StopAll.Click += new System.EventHandler(this.StopAll_Click);
            // 
            // StopAllHotkeySelector
            // 
            this.StopAllHotkeySelector.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.StopAllHotkeySelector.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.StopAllHotkeySelector.FlatAppearance.BorderSize = 0;
            this.StopAllHotkeySelector.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.StopAllHotkeySelector.ForeColor = System.Drawing.SystemColors.Control;
            this.StopAllHotkeySelector.InactiveColor = System.Drawing.SystemColors.ActiveCaption;
            this.StopAllHotkeySelector.isActive = false;
            this.StopAllHotkeySelector.Location = new System.Drawing.Point(331, 661);
            this.StopAllHotkeySelector.MultiKey = true;
            this.StopAllHotkeySelector.Name = "StopAllHotkeySelector";
            this.StopAllHotkeySelector.Size = new System.Drawing.Size(135, 29);
            this.StopAllHotkeySelector.TabIndex = 12;
            this.StopAllHotkeySelector.Text = "none";
            this.StopAllHotkeySelector.UseVisualStyleBackColor = false;
            this.StopAllHotkeySelector.HotkeyAssigned += new System.EventHandler(this.StopAllHotkeySelector_HotkeyAssigned);
            // 
            // EditButton
            // 
            this.EditButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.EditButton.FlatAppearance.BorderSize = 0;
            this.EditButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditButton.ForeColor = System.Drawing.SystemColors.Control;
            this.EditButton.Location = new System.Drawing.Point(223, 628);
            this.EditButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(102, 62);
            this.EditButton.TabIndex = 13;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = false;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(5, 811);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Microphone";
            // 
            // MicrophoneSelectComboBox
            // 
            this.MicrophoneSelectComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.MicrophoneSelectComboBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.MicrophoneSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MicrophoneSelectComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MicrophoneSelectComboBox.ForeColor = System.Drawing.SystemColors.Control;
            this.MicrophoneSelectComboBox.FormattingEnabled = true;
            this.MicrophoneSelectComboBox.Location = new System.Drawing.Point(5, 834);
            this.MicrophoneSelectComboBox.Name = "MicrophoneSelectComboBox";
            this.MicrophoneSelectComboBox.Size = new System.Drawing.Size(601, 28);
            this.MicrophoneSelectComboBox.TabIndex = 15;
            this.MicrophoneSelectComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.MicrophoneSelectComboBox_DrawItem);
            this.MicrophoneSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.MicrophoneSelectComboBox_SelectedIndexChanged);
            // 
            // soundpadToolStripMenuItem
            // 
            this.soundpadToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.soundpadToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.soundpadToolStripMenuItem.Name = "soundpadToolStripMenuItem";
            this.soundpadToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.soundpadToolStripMenuItem.Text = "Soundpad";
            this.soundpadToolStripMenuItem.Click += new System.EventHandler(this.soundpadToolStripMenuItem_Click);
            // 
            // Soundboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(619, 872);
            this.Controls.Add(this.MicrophoneSelectComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.StopAllHotkeySelector);
            this.Controls.Add(this.StopAll);
            this.Controls.Add(this.RemoveSound);
            this.Controls.Add(this.MicInjectorToggle);
            this.Controls.Add(this.SecondaryOutputComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AddSoundButton);
            this.Controls.Add(this.SoundGrid);
            this.Controls.Add(this.PrimaryOutputComboBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(637, 919);
            this.MinimumSize = new System.Drawing.Size(637, 919);
            this.Name = "Soundboard";
            this.Text = "Apollo Soundboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Soundboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SoundGrid)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FlatCombo PrimaryOutputComboBox;
        private DataGridView SoundGrid;
        private Button AddSoundButton;
        private Label label1;
        private Label label2;
        private FlatCombo SecondaryOutputComboBox;
        private CheckBox MicInjectorToggle;
        private Button RemoveSound;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem bToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private Button StopAll;
        private HotkeySelector StopAllHotkeySelector;
        private Button EditButton;
        private Label label3;
        private FlatCombo MicrophoneSelectComboBox;
        private ToolStripMenuItem volumeToolStripMenuItem;
        private ToolStripMenuItem volumeMixerToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem Import;
        private ToolStripMenuItem eXPToolStripMenuItem;
        private ToolStripMenuItem soundpadToolStripMenuItem;
    }

}
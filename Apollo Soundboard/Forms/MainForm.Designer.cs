﻿using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Apollo.Forms
{

    public static class CheckBoxExtension
    {
        public static void SetChecked(this CheckBox chBox, bool check)
        {
         var type = typeof(CheckBox);
            var field = type.GetField("checkState", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(chBox, check ? CheckState.Checked : CheckState.Unchecked);
            chBox.Invalidate();
        }
    }
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

    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.PrimaryOutputComboBox = new Apollo.Forms.FlatCombo();
            this.SoundGrid = new System.Windows.Forms.DataGridView();
            this.AddSoundButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SecondaryOutputComboBox = new Apollo.Forms.FlatCombo();
            this.MicInjectorToggle = new System.Windows.Forms.CheckBox();
            this.RemoveSound = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Import = new System.Windows.Forms.ToolStripMenuItem();
            this.eXPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundpadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeMixerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToTray = new System.Windows.Forms.ToolStripMenuItem();
            this.AlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.StopAll = new System.Windows.Forms.Button();
            this.StopAllHotkeySelector = new Apollo.HotkeySelector();
            this.EditButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MicrophoneSelectComboBox = new Apollo.Forms.FlatCombo();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyBar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MicInjectorToggleHotkey = new Apollo.HotkeySelector();
            this.PageSwitchGrid = new System.Windows.Forms.DataGridView();
            this.CycleLabel = new System.Windows.Forms.Label();
            this.CycleSelector = new Apollo.HotkeySelector();
            this.NewPage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SoundGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.NotifyBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PageSwitchGrid)).BeginInit();
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
            this.PrimaryOutputComboBox.Location = new System.Drawing.Point(201, 547);
            this.PrimaryOutputComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PrimaryOutputComboBox.Name = "PrimaryOutputComboBox";
            this.PrimaryOutputComboBox.Size = new System.Drawing.Size(526, 23);
            this.PrimaryOutputComboBox.TabIndex = 1;
            this.PrimaryOutputComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.PrimaryOutputComboBox_DrawItem);
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
            this.SoundGrid.Location = new System.Drawing.Point(201, 26);
            this.SoundGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.SoundGrid.Size = new System.Drawing.Size(526, 443);
            this.SoundGrid.TabIndex = 2;
            this.SoundGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SoundGrid_CellDoubleClick);
            this.SoundGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SoundGrid_ColumnHeaderMouseClick);
            // 
            // AddSoundButton
            // 
            this.AddSoundButton.AllowDrop = true;
            this.AddSoundButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.AddSoundButton.FlatAppearance.BorderSize = 0;
            this.AddSoundButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddSoundButton.ForeColor = System.Drawing.SystemColors.Control;
            this.AddSoundButton.Location = new System.Drawing.Point(201, 474);
            this.AddSoundButton.Name = "AddSoundButton";
            this.AddSoundButton.Size = new System.Drawing.Size(89, 46);
            this.AddSoundButton.TabIndex = 3;
            this.AddSoundButton.Text = "Add";
            this.AddSoundButton.UseVisualStyleBackColor = false;
            this.AddSoundButton.Click += new System.EventHandler(this.AddSoundButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(201, 523);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Main Output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(199, 571);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
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
            this.SecondaryOutputComboBox.Location = new System.Drawing.Point(200, 588);
            this.SecondaryOutputComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SecondaryOutputComboBox.Name = "SecondaryOutputComboBox";
            this.SecondaryOutputComboBox.Size = new System.Drawing.Size(527, 23);
            this.SecondaryOutputComboBox.TabIndex = 6;
            this.SecondaryOutputComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.SecondaryOutputComboBox_DrawItem);
            // 
            // MicInjectorToggle
            // 
            this.MicInjectorToggle.AutoSize = true;
            this.MicInjectorToggle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MicInjectorToggle.ForeColor = System.Drawing.SystemColors.Control;
            this.MicInjectorToggle.Location = new System.Drawing.Point(610, 474);
            this.MicInjectorToggle.Name = "MicInjectorToggle";
            this.MicInjectorToggle.Size = new System.Drawing.Size(106, 24);
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
            this.RemoveSound.Location = new System.Drawing.Point(296, 474);
            this.RemoveSound.Name = "RemoveSound";
            this.RemoveSound.Size = new System.Drawing.Size(89, 46);
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
            this.OptionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(743, 24);
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
            this.fileToolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.bToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.newToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.loadToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // Import
            // 
            this.Import.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Import.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eXPToolStripMenuItem,
            this.soundpadToolStripMenuItem,
            this.fromArchiveToolStripMenuItem});
            this.Import.ForeColor = System.Drawing.SystemColors.Control;
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(186, 22);
            this.Import.Text = "Import";
            // 
            // eXPToolStripMenuItem
            // 
            this.eXPToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.eXPToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.eXPToolStripMenuItem.Name = "eXPToolStripMenuItem";
            this.eXPToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.eXPToolStripMenuItem.Text = "EXP";
            this.eXPToolStripMenuItem.Click += new System.EventHandler(this.eXPToolStripMenuItem_Click);
            // 
            // soundpadToolStripMenuItem
            // 
            this.soundpadToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.soundpadToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.soundpadToolStripMenuItem.Name = "soundpadToolStripMenuItem";
            this.soundpadToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.soundpadToolStripMenuItem.Text = "Soundpad";
            this.soundpadToolStripMenuItem.Click += new System.EventHandler(this.soundpadToolStripMenuItem_Click);
            // 
            // fromArchiveToolStripMenuItem
            // 
            this.fromArchiveToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.fromArchiveToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.fromArchiveToolStripMenuItem.Name = "fromArchiveToolStripMenuItem";
            this.fromArchiveToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.fromArchiveToolStripMenuItem.Text = "From Archive";
            this.fromArchiveToolStripMenuItem.Click += new System.EventHandler(this.fromArchiveToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.fileToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.Control;
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(186, 22);
            this.fileToolStripMenuItem1.Text = "Export to Archive";
            this.fileToolStripMenuItem1.Click += new System.EventHandler(this.ExportToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.saveToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // bToolStripMenuItem
            // 
            this.bToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.bToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.bToolStripMenuItem.Name = "bToolStripMenuItem";
            this.bToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.bToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.bToolStripMenuItem.Text = "Save As";
            this.bToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.quitToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.X)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.OptionsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.volumeMixerToolStripMenuItem,
            this.ConverterToolStripMenuItem,
            this.ExitToTray,
            this.AlwaysOnTop});
            this.OptionsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.OptionsToolStripMenuItem.Text = "Options";
            // 
            // volumeMixerToolStripMenuItem
            // 
            this.volumeMixerToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.volumeMixerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.volumeMixerToolStripMenuItem.Name = "volumeMixerToolStripMenuItem";
            this.volumeMixerToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.volumeMixerToolStripMenuItem.Text = "Volume Mixer";
            this.volumeMixerToolStripMenuItem.Click += new System.EventHandler(this.volumeMixerToolStripMenuItem_Click);
            // 
            // ConverterToolStripMenuItem
            // 
            this.ConverterToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ConverterToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Control;
            this.ConverterToolStripMenuItem.Name = "ConverterToolStripMenuItem";
            this.ConverterToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.ConverterToolStripMenuItem.Text = "Audio Converter";
            this.ConverterToolStripMenuItem.Click += new System.EventHandler(this.audioConverterToolStripMenuItem_Click);
            // 
            // ExitToTray
            // 
            this.ExitToTray.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ExitToTray.Checked = true;
            this.ExitToTray.CheckOnClick = true;
            this.ExitToTray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExitToTray.ForeColor = System.Drawing.SystemColors.Control;
            this.ExitToTray.Name = "ExitToTray";
            this.ExitToTray.Size = new System.Drawing.Size(161, 22);
            this.ExitToTray.Text = "Exit to Tray";
            this.ExitToTray.CheckedChanged += new System.EventHandler(this.ExitToTray_CheckedChanged);
            // 
            // AlwaysOnTop
            // 
            this.AlwaysOnTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.AlwaysOnTop.Checked = true;
            this.AlwaysOnTop.CheckOnClick = true;
            this.AlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AlwaysOnTop.ForeColor = System.Drawing.SystemColors.Control;
            this.AlwaysOnTop.Name = "AlwaysOnTop";
            this.AlwaysOnTop.Size = new System.Drawing.Size(161, 22);
            this.AlwaysOnTop.Text = "Always on Top";
            this.AlwaysOnTop.CheckedChanged += new System.EventHandler(this.AlwaysOnTop_CheckedChanged);
            // 
            // StopAll
            // 
            this.StopAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.StopAll.FlatAppearance.BorderSize = 0;
            this.StopAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopAll.ForeColor = System.Drawing.SystemColors.Control;
            this.StopAll.Location = new System.Drawing.Point(485, 474);
            this.StopAll.Name = "StopAll";
            this.StopAll.Size = new System.Drawing.Size(118, 23);
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
            this.StopAllHotkeySelector.Location = new System.Drawing.Point(485, 499);
            this.StopAllHotkeySelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StopAllHotkeySelector.MultiKey = true;
            this.StopAllHotkeySelector.Name = "StopAllHotkeySelector";
            this.StopAllHotkeySelector.SelectedHotkeys = ((System.Collections.Generic.List<System.Windows.Forms.Keys>)(resources.GetObject("StopAllHotkeySelector.SelectedHotkeys")));
            this.StopAllHotkeySelector.Size = new System.Drawing.Size(118, 22);
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
            this.EditButton.Location = new System.Drawing.Point(390, 474);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(89, 46);
            this.EditButton.TabIndex = 13;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = false;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(199, 611);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
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
            this.MicrophoneSelectComboBox.Location = new System.Drawing.Point(201, 629);
            this.MicrophoneSelectComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MicrophoneSelectComboBox.Name = "MicrophoneSelectComboBox";
            this.MicrophoneSelectComboBox.Size = new System.Drawing.Size(526, 23);
            this.MicrophoneSelectComboBox.TabIndex = 15;
            this.MicrophoneSelectComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.MicrophoneSelectComboBox_DrawItem);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.NotifyBar;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "Apollo Soundboard";
            this.NotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseClick);
            // 
            // NotifyBar
            // 
            this.NotifyBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.NotifyBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem1});
            this.NotifyBar.Name = "NotifyBar";
            this.NotifyBar.Size = new System.Drawing.Size(98, 26);
            // 
            // quitToolStripMenuItem1
            // 
            this.quitToolStripMenuItem1.Name = "quitToolStripMenuItem1";
            this.quitToolStripMenuItem1.Size = new System.Drawing.Size(97, 22);
            this.quitToolStripMenuItem1.Text = "Quit";
            this.quitToolStripMenuItem1.Click += new System.EventHandler(this.quitToolStripMenuItem1_Click);
            // 
            // MicInjectorToggleHotkey
            // 
            this.MicInjectorToggleHotkey.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MicInjectorToggleHotkey.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.MicInjectorToggleHotkey.FlatAppearance.BorderSize = 0;
            this.MicInjectorToggleHotkey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MicInjectorToggleHotkey.ForeColor = System.Drawing.SystemColors.Control;
            this.MicInjectorToggleHotkey.InactiveColor = System.Drawing.SystemColors.ActiveCaption;
            this.MicInjectorToggleHotkey.isActive = false;
            this.MicInjectorToggleHotkey.Location = new System.Drawing.Point(609, 499);
            this.MicInjectorToggleHotkey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MicInjectorToggleHotkey.MultiKey = true;
            this.MicInjectorToggleHotkey.Name = "MicInjectorToggleHotkey";
            this.MicInjectorToggleHotkey.SelectedHotkeys = ((System.Collections.Generic.List<System.Windows.Forms.Keys>)(resources.GetObject("MicInjectorToggleHotkey.SelectedHotkeys")));
            this.MicInjectorToggleHotkey.Size = new System.Drawing.Size(118, 22);
            this.MicInjectorToggleHotkey.TabIndex = 16;
            this.MicInjectorToggleHotkey.Text = "none";
            this.MicInjectorToggleHotkey.UseVisualStyleBackColor = false;
            this.MicInjectorToggleHotkey.HotkeyAssigned += new System.EventHandler(this.MicInjectorToggleHotkey_HotkeyAssigned);
            // 
            // PageSwitchGrid
            // 
            this.PageSwitchGrid.AllowUserToAddRows = false;
            this.PageSwitchGrid.AllowUserToDeleteRows = false;
            this.PageSwitchGrid.AllowUserToResizeRows = false;
            this.PageSwitchGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PageSwitchGrid.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.PageSwitchGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PageSwitchGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PageSwitchGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.PageSwitchGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PageSwitchGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.PageSwitchGrid.EnableHeadersVisualStyles = false;
            this.PageSwitchGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.PageSwitchGrid.Location = new System.Drawing.Point(12, 26);
            this.PageSwitchGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PageSwitchGrid.MultiSelect = false;
            this.PageSwitchGrid.Name = "PageSwitchGrid";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PageSwitchGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.PageSwitchGrid.RowHeadersVisible = false;
            this.PageSwitchGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.PageSwitchGrid.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.PageSwitchGrid.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.Control;
            this.PageSwitchGrid.RowTemplate.Height = 29;
            this.PageSwitchGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PageSwitchGrid.Size = new System.Drawing.Size(183, 560);
            this.PageSwitchGrid.TabIndex = 17;
            this.PageSwitchGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PageSwitchGrid_CellClick);
            this.PageSwitchGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PageSwitchGrid_CellContentClick);
            this.PageSwitchGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PageSwitchGrid_CellContentDoubleClick);
            this.PageSwitchGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.PageSwitchGrid_CellEndEdit);
            // 
            // CycleLabel
            // 
            this.CycleLabel.AutoSize = true;
            this.CycleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CycleLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.CycleLabel.Location = new System.Drawing.Point(12, 629);
            this.CycleLabel.Name = "CycleLabel";
            this.CycleLabel.Size = new System.Drawing.Size(47, 21);
            this.CycleLabel.TabIndex = 18;
            this.CycleLabel.Text = "Cycle";
            // 
            // CycleSelector
            // 
            this.CycleSelector.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.CycleSelector.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.CycleSelector.FlatAppearance.BorderSize = 0;
            this.CycleSelector.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CycleSelector.ForeColor = System.Drawing.SystemColors.Control;
            this.CycleSelector.InactiveColor = System.Drawing.SystemColors.ActiveCaption;
            this.CycleSelector.isActive = false;
            this.CycleSelector.Location = new System.Drawing.Point(65, 629);
            this.CycleSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CycleSelector.MultiKey = true;
            this.CycleSelector.Name = "CycleSelector";
            this.CycleSelector.SelectedHotkeys = ((System.Collections.Generic.List<System.Windows.Forms.Keys>)(resources.GetObject("CycleSelector.SelectedHotkeys")));
            this.CycleSelector.Size = new System.Drawing.Size(130, 23);
            this.CycleSelector.TabIndex = 19;
            this.CycleSelector.Text = "none";
            this.CycleSelector.UseVisualStyleBackColor = false;
            this.CycleSelector.HotkeyAssigned += new System.EventHandler(this.CycleSelector_HotkeyAssigned);
            // 
            // NewPage
            // 
            this.NewPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.NewPage.FlatAppearance.BorderSize = 0;
            this.NewPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewPage.ForeColor = System.Drawing.SystemColors.Control;
            this.NewPage.Location = new System.Drawing.Point(12, 591);
            this.NewPage.Name = "NewPage";
            this.NewPage.Size = new System.Drawing.Size(183, 30);
            this.NewPage.TabIndex = 20;
            this.NewPage.Text = "New Page";
            this.NewPage.UseVisualStyleBackColor = false;
            this.NewPage.Click += new System.EventHandler(this.NewPage_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(743, 660);
            this.Controls.Add(this.NewPage);
            this.Controls.Add(this.CycleSelector);
            this.Controls.Add(this.CycleLabel);
            this.Controls.Add(this.PageSwitchGrid);
            this.Controls.Add(this.MicInjectorToggleHotkey);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(759, 699);
            this.MinimumSize = new System.Drawing.Size(759, 699);
            this.Name = "MainForm";
            this.Text = "Apollo Soundboard";
            this.Load += new System.EventHandler(this.Soundboard_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.File_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.File_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.SoundGrid)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.NotifyBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PageSwitchGrid)).EndInit();
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
        private ToolStripMenuItem OptionsToolStripMenuItem;
        private ToolStripMenuItem volumeMixerToolStripMenuItem;
        private ToolStripMenuItem ConverterToolStripMenuItem;
        private ToolStripMenuItem Import;
        private ToolStripMenuItem eXPToolStripMenuItem;
        private ToolStripMenuItem soundpadToolStripMenuItem;
        private ToolStripMenuItem fromArchiveToolStripMenuItem;
        private NotifyIcon NotifyIcon;
        private ContextMenuStrip NotifyBar;
        private ToolStripMenuItem quitToolStripMenuItem1;
        private HotkeySelector MicInjectorToggleHotkey;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem ExitToTray;
        private ToolStripMenuItem AlwaysOnTop;
        private DataGridView PageSwitchGrid;
        private Label CycleLabel;
        private HotkeySelector CycleSelector;
        private Button NewPage;
    }

}
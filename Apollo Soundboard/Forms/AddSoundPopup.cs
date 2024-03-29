﻿using Apollo.Properties;
using System.Data;

namespace Apollo.Forms
{
    public partial class AddSoundPopup : Form
    {
        public string FilePath = "";
        public string SoundName = "";
        public List<Keys> Hotkeys = new();
        public float Gain = 0;
        public bool HotkeyOrderMatters = false;
        public bool OverlapSelf = true;
        public AddSoundPopup()
        {
            InitializeComponent();

            Owner = MainForm.Instance;
            TopMost = Settings.Default.AlwaysOnTop;
        }

        public static float Remap(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        public AddSoundPopup(Sound sound)
        {
            InitializeComponent();
            Owner = MainForm.Instance;
            TopMost = Settings.Default.AlwaysOnTop;
            FilePathBox.Text = sound.FilePath;
            FileNameBox.Text = sound.SoundName;
            OverlapSelf = sound.OverlapSelf;
            FilePath = sound.FilePath;
            SoundName = sound.SoundName;
            Hotkeys = sound.GetHotkeys();
            Gain = sound.Gain;
            HotkeyOrderMatters = sound.HotkeyOrderMatters;
            GainBar.Value = (int)Remap(Gain, -1, 1, GainBar.Minimum, GainBar.Maximum);
            HotkeySelectorButton.SelectedHotkeys = Hotkeys;
            HotkeyOrderMattersCheckbox.Checked = HotkeyOrderMatters;
            OverlapSelfCheckbox.Checked = OverlapSelf;
        }

        public AddSoundPopup(string filePath, string soundName, List<Keys> _Hotkeys, float _Gain = 0, bool _HotkeyOrderMatters = false, bool overlapSelf = true)
        {
            InitializeComponent();
            FilePathBox.Text = filePath;
            FileNameBox.Text = soundName;
            FilePath = filePath;
            SoundName = soundName;
            Hotkeys = _Hotkeys;
            Gain = _Gain;
            OverlapSelf = overlapSelf;
            HotkeyOrderMatters = _HotkeyOrderMatters;
            GainBar.Value = (int)Remap(Gain, -1, 1, GainBar.Minimum, GainBar.Maximum);
            HotkeySelectorButton.SelectedHotkeys = Hotkeys;
            HotkeyOrderMattersCheckbox.Checked = HotkeyOrderMatters;
            OverlapSelfCheckbox.Checked = OverlapSelf;
        }


        private void Browse_Click(object sender, EventArgs e)
        {

            OpenFileDialog AudioFileSelector = new();
            //openfiledialog filter is only audio files
            string list = String.Join(";", MainForm.SupportedExtensions);
            string listWithStars = String.Join(";", MainForm.SupportedExtensions.Select(extension => "*" + extension));
            AudioFileSelector.Filter = $"Audio files ({list})|{listWithStars}";
            DialogResult result = AudioFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                FilePath = AudioFileSelector.FileName;
                FilePathBox.Text = FilePath;
                FileNameBox.Text = Path.GetFileName(FilePath);
            }
        }

        private void ConfirmAdd_Click(object sender, EventArgs e)
        {
            bool fileExists = File.Exists(FilePath);
            if (fileExists)
            {
                Hotkeys = HotkeySelectorButton.SelectedHotkeys;
                FilePath = FilePathBox.Text;
                SoundName = FileNameBox.Text;
                DialogResult = DialogResult.OK;

            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }



        private void GainBar_ValueChanged(object sender, EventArgs e)
        {
            Gain = Remap(GainBar.Value, GainBar.Minimum, GainBar.Maximum, -1, 1);
        }

        private void HotkeyOrderMatters_CheckedChanged(object sender, EventArgs e)
        {
            HotkeyOrderMatters = HotkeyOrderMattersCheckbox.Checked;
        }

        private void OverlapSelfCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            OverlapSelf = OverlapSelfCheckbox.Checked;
        }
    }
}

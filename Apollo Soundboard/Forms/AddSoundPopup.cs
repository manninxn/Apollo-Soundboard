using System.Data;

namespace Apollo_Soundboard
{
    public partial class AddSoundPopup : Form
    {
        public string FileName = "";
        public List<Keys> Hotkeys = new List<Keys>();
        public float Gain = 0;
        public bool HotkeyOrderMatters = false;
        public AddSoundPopup()
        {
            InitializeComponent();
        }

        float Remap(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        public AddSoundPopup(string fileName, List<Keys> _Hotkeys, float _Gain = 0, bool _HotkeyOrderMatters = false)
        {
            InitializeComponent();
            FilePathBox.Text = fileName;
            FileName = fileName;
            Hotkeys = _Hotkeys;
            Gain = _Gain;
            HotkeyOrderMatters = _HotkeyOrderMatters;
            GainBar.Value = (int)Remap(Gain, -1, 1, GainBar.Minimum, GainBar.Maximum);
            HotkeySelectorButton.Text = String.Join("+", Hotkeys.Select(i => i.ToString()).ToList());
            HotkeySelectorButton.SelectedHotkeys = Hotkeys;
            HotkeyOrderMattersCheckbox.Checked = HotkeyOrderMatters;
        }


        private void Browse_Click(object sender, EventArgs e)
        {

            OpenFileDialog AudioFileSelector = new OpenFileDialog();
            //openfiledialog filter is only audio files
            string list = String.Join(";", Soundboard.SupportedExtensions);
            string listWithStars = String.Join(";", Soundboard.SupportedExtensions.Select(extension => "*" + extension));
            AudioFileSelector.Filter = $"Audio files ({list})|{listWithStars}";
            DialogResult result = AudioFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                FileName = AudioFileSelector.FileName;
                FilePathBox.Text = FileName;
            }
        }

        private void ConfirmAdd_Click(object sender, EventArgs e)
        {
            bool fileExists = File.Exists(FileName);
            if (fileExists)
            {
                Hotkeys = HotkeySelectorButton.SelectedHotkeys;
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
    }
}

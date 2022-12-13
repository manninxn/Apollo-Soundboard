using System.Data;

namespace Apollo_Soundboard
{
    public partial class AddSoundPopup : Form
    {
        public string FileName = "";
        public List<Keys> Hotkeys = new List<Keys>();
        public AddSoundPopup()
        {
            InitializeComponent();
        }

        public AddSoundPopup(string fileName, List<Keys> Hotkeys)
        {
            InitializeComponent();
            FilePathBox.Text = fileName;
            FileName = fileName;
            HotkeySelectorButton.Text = String.Join("+", Hotkeys.Select(i => i.ToString()).ToList());
            HotkeySelectorButton.SelectedHotkeys = Hotkeys;
        }


        private void Browse_Click(object sender, EventArgs e)
        {

            OpenFileDialog AudioFileSelector = new OpenFileDialog();
            //openfiledialog filter is only audio files
            AudioFileSelector.Filter = "Audio files (*.WAV;*.MP3)|*.WAV;*.MP3";
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

    }
}

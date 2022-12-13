using Apollo_Soundboard.Forms;
using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace Apollo_Soundboard
{

    public partial class Soundboard : Form
    {
        #region Properties

        string _fileName = Settings.Default.FileName;
        string fileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                Settings.Default.FileName = value;
                Settings.Default.Save();
            }
        }

        bool _micInjector = Settings.Default.MicInjector;
        bool MicInjector
        {
            get { return _micInjector; }
            set
            {
                _micInjector = value;
                Settings.Default.MicInjector = value;
                Settings.Default.Save();
            }
        }

        int _primary = Settings.Default.PrimaryOutput;
        public int primaryOutput
        {
            get
            {
                return _primary;
            }
            set
            {
                _primary = value;
                Settings.Default.PrimaryOutput = value;
                Settings.Default.Save();

            }
        }

        int _secondary = Settings.Default.SecondaryOutput;
        public int secondaryOutput
        {
            get
            {
                return _secondary;
            }
            set
            {
                _secondary = value;
                Settings.Default.SecondaryOutput = value;
                Settings.Default.Save();
            }
        }

        int _microphone = Settings.Default.Microphone;
        int Microphone
        {
            get
            {
                return _microphone;
            }
            set
            {
                _microphone = value;
                Settings.Default.Microphone = value;
                Settings.Default.Save();
            }
        }

        bool saved = true;

        WaveIn? micStream; DirectSoundOut? virtualCable;

        #endregion


        public Soundboard()
        {

            Debug.WriteLine(primaryOutput);
            InitializeComponent();


            var Devices = new List<string>();
            var DevicesWithNone = new List<string>() { "None" };
            var Microphones = new List<string>();

            var enumerator = new MMDeviceEnumerator();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                Devices.Add(enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[i].ToString());
                DevicesWithNone.Add(enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[i].ToString());
            }

            for (int i = 0; i < WaveIn.DeviceCount; i++)
                Microphones.Add(enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[i].ToString());

            enumerator.Dispose();

            int secondaryTemp = secondaryOutput;
            int primaryTemp = primaryOutput;
            int micTemp = Microphone;

            PrimaryOutputComboBox.DataSource = Devices;
            SecondaryOutputComboBox.DataSource = DevicesWithNone;
            MicrophoneSelectComboBox.DataSource = Microphones;

            SecondaryOutputComboBox.SelectedIndex = secondaryTemp;
            PrimaryOutputComboBox.SelectedIndex = primaryTemp;
            MicrophoneSelectComboBox.SelectedIndex = micTemp;

            LoadFile();

            MicInjectorToggle.Checked = MicInjector;



            InputHandler.Subscribe();

            SoundItem.SetForm(this);

            StopAllHotkeySelector.Text = String.Join("+", SoundItem.ClearSounds.Select(i => i.ToString()).ToList());

            dataGridView1.DataSource = SoundItem.AllSounds;
        }

        #region Helper Methods
        private void InjectMicrophone()
        {
            if (secondaryOutput <= 0) return;

            micStream = new WaveIn();
            micStream.DeviceNumber = Microphone;

            micStream.BufferMilliseconds = 30;
            micStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(-1).Channels);

            WaveInProvider waveIn = new(micStream);
            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = Settings.Default.MicrophoneGain;

            Debug.WriteLine(DirectSoundOut.Devices.ElementAt(secondaryOutput).Description);
            virtualCable = new(DirectSoundOut.Devices.ElementAt(secondaryOutput).Guid);
            Debug.WriteLine(DirectSoundOut.Devices.ElementAt(secondaryOutput).Description);
            virtualCable.Init(volumeSampleProvider);

            micStream.StartRecording();
            virtualCable.Play();

        }

        public void RefreshGrid()
        {
            dataGridView1.DataSource = typeof(List<SoundItem>);
            dataGridView1.DataSource = SoundItem.AllSounds;
        }

        public void RefreshInjector()
        {
            if (MicInjector && micStream != null && virtualCable != null)
            {
                micStream.StopRecording();
                micStream.Dispose();
                virtualCable.Dispose();
                InjectMicrophone();
            }
        }
        private void LoadFile()
        {
            if (fileName != string.Empty)
            {
                SoundItem.AllSounds = Serializer.DeserializeFile(fileName);
                RefreshGrid();
            }
        }
        private void ExitApplication()
        {
            if (!saved)
            {
                UnsavedChanges prompt = new UnsavedChanges();
                var result = prompt.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    Save(false);
                }
            }
            Debug.WriteLine(Settings.Default.FileName);
            Settings.Default.Save();
            Application.Exit();
        }
        private void Save(bool newFile)
        {
            if (newFile || fileName == string.Empty)
            {
                SaveFileDialog saveFileSelector = new SaveFileDialog();
                //openfiledialog filter is only audio files
                saveFileSelector.Filter = "JSON files (*.JSON)|*.JSON";
                DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    fileName = saveFileSelector.FileName;
                    Serializer.SerializeToFile(SoundItem.AllSounds, fileName);
                    saved = true;
                }
            }
            else
            {
                Serializer.SerializeToFile(SoundItem.AllSounds, fileName);
                saved = true;
            }
        }
        #endregion

        #region Tool Strip
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(false);
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFile();
            OpenFileDialog saveFileSelector = new OpenFileDialog();
            //openfiledialog filter is only audio files
            saveFileSelector.Filter = "JSON files (*.JSON)|*.JSON";
            DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                fileName = saveFileSelector.FileName;
                LoadFile();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileName = string.Empty;
            SoundItem.AllSounds.Clear();
            RefreshGrid();
        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void volumeMixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VolumeMixer mixer = new VolumeMixer(this);
            mixer.ShowDialog();
        }

        private void audioConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AudioConverter converter = new AudioConverter();
            converter.ShowDialog();
        }

        #endregion

        #region Control Bar
        private void RemoveSound_Click(object sender, EventArgs e)
        {
            saved = false;
            if (dataGridView1.SelectedRows.Count > 0)
            {

                var row = dataGridView1.SelectedRows[0];
                SoundItem sound = SoundItem.AllSounds[row.Index];

                sound.Destroy();

            }
        }
        private void StopAll_Click(object sender, EventArgs e)
        {
            SoundItem.StopAllSounds();
        }

        private void StopAllHotkeySelector_HotkeyAssigned(object sender, EventArgs e)
        {
            SoundItem.ClearSounds = StopAllHotkeySelector.SelectedHotkeys;
        }
        private void MicInjectorToggle_CheckChanged(object sender, EventArgs e)
        {
            MicInjector = MicInjectorToggle.Checked;
            if (MicInjectorToggle.Checked)
            {
                InjectMicrophone();
            }
            else
            {
                if (micStream != null && virtualCable != null)
                {
                    micStream.StopRecording();
                    micStream.Dispose();
                    virtualCable.Dispose();
                }
            }
        }

        private void AddSoundButton_Click(object sender, EventArgs e)
        {
            AddSoundPopup popup = new AddSoundPopup();
            var result = popup.ShowDialog();
            Debug.WriteLine(result);
            if (result == DialogResult.OK)
            {
                new SoundItem(popup.Hotkeys, popup.FileName);
                RefreshGrid();
                saved = false;
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                var row = dataGridView1.SelectedRows[0];
                SoundItem sound = SoundItem.AllSounds[row.Index];

                AddSoundPopup popup = new AddSoundPopup(sound.FilePath, sound.GetHotkeys());
                var result = popup.ShowDialog();
                Debug.WriteLine(result);
                if (result == DialogResult.OK)
                {
                    sound.SetHotkeys(popup.Hotkeys);
                    sound.FilePath = popup.FileName;
                    RefreshGrid();
                    saved = false;
                }


            }

        }



#endregion

        #region Device Selectors
        private void MicrophoneSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Microphone = MicrophoneSelectComboBox.SelectedIndex;
            RefreshInjector();
        }

        private void PrimaryOutputComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            primaryOutput = PrimaryOutputComboBox.SelectedIndex;
            RefreshInjector();
        }

        private void SecondaryOutputComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            secondaryOutput = SecondaryOutputComboBox.SelectedIndex;
            RefreshInjector();
        }
        private void PrimaryOutputComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(PrimaryOutputComboBox.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void SecondaryOutputComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(SecondaryOutputComboBox.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void MicrophoneSelectComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(MicrophoneSelectComboBox.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }


        #endregion
        private void Soundboard_Load(object sender, EventArgs e)
        {
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new TestColorTable());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall) ExitApplication();
        }

    }

    //lol
    public class TestColorTable : ProfessionalColorTable
    {

        public override Color MenuBorder => Color.FromArgb(26, 26, 26);

        public override Color ToolStripDropDownBackground => Color.FromArgb(45, 45, 45);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(60, 60, 60);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(60, 60, 60);
        public override Color MenuItemSelected => Color.FromArgb(60, 60, 60);

        public override Color MenuItemPressedGradientBegin => Color.FromArgb(45, 45, 45);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(45, 45, 45);
        public override Color ImageMarginGradientBegin => Color.FromArgb(45, 45, 45);
        public override Color ImageMarginGradientEnd => Color.FromArgb(45, 45, 45);

    }
}
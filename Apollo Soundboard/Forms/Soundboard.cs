using Apollo_Soundboard.Forms;
using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Diagnostics;
using NUT_Soundboard.Importers;
using System.Windows.Forms;

namespace Apollo_Soundboard
{

    public partial class Soundboard : Form
    {
        #region Properties

        string _fileName = "";
        string fileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                Settings.Default.FileName = value;
                Settings.Default.Save();

                if(value != string.Empty)
                {
                    this.Text = $"Apollo Soundboard - {Path.GetFileName(value)}";
                } else
                {
                    this.Text = "Apollo Soundboard";
                }

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

        Guid _primary = Settings.Default.PrimaryOutput;
        public Guid primaryOutput
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

        Guid _secondary = Settings.Default.SecondaryOutput;
        public Guid secondaryOutput
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

        bool _saved = true;
        bool saved
        {
            get
            {
                return _saved;
            }
            set
            {
                if (value) this.Text = this.Text.TrimEnd('*');
                else
                {
                    if(!this.Text.EndsWith("*")) this.Text = this.Text + "*";

                }
            }
        }

        WaveIn? micStream; DirectSoundOut? virtualCable; BindingSource source;

        public static Guid NoDeviceGuid = new Guid("a8103378-999f-4e9c-a82f-12e2ff5395ed");

        #endregion

        public Soundboard(string? file)
        {
            
            Debug.WriteLine(primaryOutput);
            InitializeComponent();
            fileName = file ?? Settings.Default.FileName;
            var bindingList = new BindingList<SoundItem>(SoundItem.AllSounds);
            source = new BindingSource(bindingList, null);
            SoundGrid.DataSource = source;

            var Devices = new Dictionary<Guid, string>() { };
            var DevicesWithNone = new Dictionary<Guid, string>() { { NoDeviceGuid, "None" } };
            var Microphones = new List<string>() { "Default Device" };



            int secondaryTemp = 0;
            int primaryTemp = 0;
            int i = 0;
            foreach (var device in DirectSoundOut.Devices)
            {
                if (device.Guid == secondaryOutput) secondaryTemp = i + 1;
                else if(device.Guid == primaryOutput) primaryTemp = i;

                Devices.Add(device.Guid, device.Description);
                DevicesWithNone.Add(device.Guid, device.Description);
                i++;
            }

            var enumerator = new MMDeviceEnumerator();

            for (i = 0; i < WaveIn.DeviceCount; i++)
                Microphones.Add(enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[i].ToString());

            enumerator.Dispose();


            int micTemp = Microphone;

            PrimaryOutputComboBox.DataSource = new BindingSource(Devices, null);
            PrimaryOutputComboBox.DisplayMember = "Value";
            PrimaryOutputComboBox.ValueMember = "Key";

            SecondaryOutputComboBox.DataSource = new BindingSource(Devices, null);
            SecondaryOutputComboBox.DisplayMember = "Value";
            SecondaryOutputComboBox.ValueMember = "Key";


            MicrophoneSelectComboBox.DataSource = Microphones;

            try
            {
                SecondaryOutputComboBox.SelectedIndex = secondaryTemp;
                PrimaryOutputComboBox.SelectedIndex = primaryTemp;
                MicrophoneSelectComboBox.SelectedIndex = micTemp;
            } catch
            {
                Debug.WriteLine("Device index exceeded list");
            }
            LoadFile();

            MicInjectorToggle.Checked = MicInjector;



            InputHandler.Subscribe();

            SoundItem.SetForm(this);

            StopAllHotkeySelector.Text = String.Join("+", SoundItem.ClearSounds.Select(i => i.ToString()).ToList());


        }

        #region Helper Methods
        private void InjectMicrophone()
        {
            if (secondaryOutput == NoDeviceGuid) return;

            micStream = new WaveIn();
            
            micStream.BufferMilliseconds = 30;
            micStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Microphone).Channels);
            if(Microphone > 0) Debug.WriteLine(new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[Microphone - 1].ToString());
            micStream.DeviceNumber = Microphone;

            WaveInProvider waveIn = new(micStream);
            
            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = 1 + Settings.Default.MicrophoneGain;

            virtualCable = new(secondaryOutput);

            virtualCable.Init(volumeSampleProvider);

            micStream.StartRecording();
            virtualCable.Play();

        }

        public void RefreshGrid()
        {
            // SoundGrid.
            //SoundGrid.DataSource = null;
            // if (SoundItem.AllSounds.Count > 0)
            //      SoundGrid.DataSource = SoundItem.AllSounds;
            //SoundGrid.DataSource = SoundItem.AllSounds;
            source.DataSource = SoundItem.AllSounds;
            source.ResetBindings(false);
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
                saveFileSelector.Filter = "Apollo Soundboard files (*.asb)|*.asb";
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
            OpenFileDialog saveFileSelector = new OpenFileDialog();
            //openfiledialog filter is only audio files
            saveFileSelector.Filter = "Apollo Soundboard files (*.asb)|*.asb";
            DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                if (!saved)
                {
                    UnsavedChanges prompt = new UnsavedChanges();
                    if (prompt.ShowDialog() == DialogResult.Yes)
                    {
                        Save(false);
                    }
                }
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
            if (!saved)
            {
                UnsavedChanges prompt = new UnsavedChanges();
                var result = prompt.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    Save(false);
                }
            }
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
            if (SoundGrid.SelectedRows.Count > 0)
            {

                var row = SoundGrid.SelectedRows[0];
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
                new SoundItem(popup.Hotkeys, popup.FileName, popup.Gain, popup.HotkeyOrderMatters);
                RefreshGrid();
                saved = false;
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (SoundGrid.SelectedRows.Count > 0)
            {

                var row = SoundGrid.SelectedRows[0];
                SoundItem sound = SoundItem.AllSounds[row.Index];

                AddSoundPopup popup = new AddSoundPopup(sound.FilePath, sound.GetHotkeys(), sound.Gain, sound.HotkeyOrderMatters);
                var result = popup.ShowDialog();
                Debug.WriteLine(result);
                if (result == DialogResult.OK)
                {
                    sound.SetHotkeys(popup.Hotkeys);
                    sound.FilePath = popup.FileName;
                    sound.Gain = popup.Gain;
                    sound.HotkeyOrderMatters = popup.HotkeyOrderMatters;
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
            primaryOutput = ((KeyValuePair<Guid, string>)PrimaryOutputComboBox.SelectedItem).Key;
            RefreshInjector();
        }

        private void SecondaryOutputComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            secondaryOutput = ((KeyValuePair<Guid, string>)SecondaryOutputComboBox.SelectedItem).Key;
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

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            
            base.OnFormClosing(e);
            
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                Hide();
                NotifyIcon.Visible = true;
                NotifyIcon.BalloonTipText = "Your soundboard hotkeys will still work.";
                NotifyIcon.BalloonTipTitle = "Apollo is running in the background";
                NotifyIcon.ShowBalloonTip(500);
            }
        }

        private void SoundGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Debug.WriteLine(e.RowIndex);
            if (e.RowIndex < 0) return;
            SoundItem sound = SoundItem.AllSounds[e.RowIndex];

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

        private void eXPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog saveFileSelector = new OpenFileDialog();
            //openfiledialog filter is only audio files
            saveFileSelector.Filter = "JSON files (*.json)|*.json";
            DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                saved = false;
                fileName = string.Empty;
                SoundItem.AllSounds = EXPImporter.Import(saveFileSelector.FileName);
                RefreshGrid();
            }
        }

        private void soundpadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog saveFileSelector = new OpenFileDialog();
            //openfiledialog filter is only audio files
            saveFileSelector.Filter = "Soundpad files (*.spl)|*.spl";
            DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                saved = false;
                fileName = string.Empty;
                SoundItem.AllSounds = SoundpadImporter.Import(saveFileSelector.FileName);
                RefreshGrid();
            }
        }

        private void File_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                var ext = System.IO.Path.GetExtension(file);
                if (ext.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase) ||
                    ext.Equals(".wav", StringComparison.CurrentCultureIgnoreCase))
                {
                    AddSoundPopup popup = new AddSoundPopup(file, new List<Keys>());
                    var result = popup.ShowDialog();
                    Debug.WriteLine(result);
                    if (result == DialogResult.OK)
                    {
                        new SoundItem(popup.Hotkeys, popup.FileName);
                        RefreshGrid();
                        saved = false;
                    }
                    return;
                }
            }
        }

        private void File_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                var ext = System.IO.Path.GetExtension(file);
                if (ext.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase) ||
                    ext.Equals(".wav", StringComparison.CurrentCultureIgnoreCase))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                NotifyIcon.Visible = false;
            }
        }

        private void Soundboard_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                NotifyIcon.Visible = true;
                Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                NotifyIcon.Visible = false;
            }
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            Show();
            ExitApplication();
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
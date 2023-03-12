using Apollo.IO;
using Apollo.Properties;
using AutoUpdaterDotNET;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Apollo.Forms
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

                if (value != string.Empty)
                {
                    this.Text = $"Apollo Soundboard - {Path.GetFileName(value)}";
                }
                else
                {
                    this.Text = "Apollo Soundboard";
                }

            }
        }

        ListSortDirection sortDirection = ListSortDirection.Descending;



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
                    if (!this.Text.EndsWith("*")) this.Text += "*";

                }
                _saved = value;
            }
        }


        #endregion

        #region Managers

        public static Soundboard Instance { get; set; }

        private DeviceManager _Devices = new();

        public static DeviceManager Devices
        {
            get
            {
                return Instance._Devices;
            }
            set
            {
                Instance._Devices = value;
            }
        }

        private MicInjector _MicInjector = new();

        public static MicInjector MicInjector
        {
            get
            {
                return Instance._MicInjector;
            }
            set
            {
                Instance._MicInjector = value;
            }
        }



        #endregion

        public static string[] SupportedExtensions = new string[]
        {
            ".wav", ".ogg", ".mp3"
        };

        public static bool IsFileSupported(string path)
        {
            var ext = System.IO.Path.GetExtension(path);
            var supported = false;
            Array.ForEach(SupportedExtensions, extension => supported |= ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
            return supported;
        }

        public Soundboard(string? file)
        {

            AutoUpdater.Start("https://raw.githubusercontent.com/manninxn/Apollo-Soundboard/master/Apollo%20Soundboard/version.xml");

            if (Instance is not null) return;

            Instance = this;
            InitializeComponent();

            if (file != null && Path.GetExtension(file) == ".asba")
            {
                var result = Archiver.LoadFromArchive(file);

                if (result != null)
                {
                    SoundItem.AllSounds.Clear();
                    SoundItem.AllSounds.AddRange(result.Value.Sounds);
                    saved = true;
                    fileName = result.Value.SaveFile;
                }
            }
            else
            {
                fileName = file ?? Settings.Default.FileName;
            }
            SoundGrid.DataSource = SoundItem.AllSounds;

            SoundGrid.Columns[0].HeaderText = "Sound";

            SoundGrid.Columns[0].FillWeight = 60;
            SoundGrid.Columns[1].FillWeight = 25;
            SoundGrid.Columns[2].FillWeight = 15;


            PrimaryOutputComboBox.DisplayMember = "Name";
            PrimaryOutputComboBox.ValueMember = "DeviceNumber";
            PrimaryOutputComboBox.DataSource = Devices.PrimaryDevices;
            PrimaryOutputComboBox.SelectionChangeCommitted += Devices.PrimaryOutputSelect;

            SecondaryOutputComboBox.DisplayMember = "Name";
            SecondaryOutputComboBox.ValueMember = "DeviceNumber";
            SecondaryOutputComboBox.DataSource = Devices.SecondaryDevices;
            SecondaryOutputComboBox.SelectionChangeCommitted += Devices.SecondaryOutputSelect;

            MicrophoneSelectComboBox.DisplayMember = "Name";
            MicrophoneSelectComboBox.ValueMember = "DeviceNumber";
            MicrophoneSelectComboBox.DataSource = Devices.Microphones;
            MicrophoneSelectComboBox.SelectionChangeCommitted += Devices.MicrophoneSelect;




            Devices.Refresh();
            int primaryIndex = Devices.PrimaryOutput + 1, secondaryIndex = Devices.SecondaryOutput + 2, microphoneIndex = Devices.Microphone + 1;


            try
            {
                SecondaryOutputComboBox.SelectedIndex = secondaryIndex;
                PrimaryOutputComboBox.SelectedIndex = primaryIndex;
                MicrophoneSelectComboBox.SelectedIndex = microphoneIndex;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            LoadFile();




            InputHandler.Subscribe();

            StopAllHotkeySelector.Text = String.Join("+", SoundItem.ClearSounds.Select(i => KeyMap.KeyToChar(i)).ToList());
            MicInjectorToggleHotkey.Text = String.Join("+", MicInjector.ToggleInjector.Select(i => KeyMap.KeyToChar(i)).ToList());
            Devices.DevicesUpdated += UpdateDeviceSelectors;


            MicInjectorToggle.Checked = MicInjector.Initialize();

            ExitToTray.Checked = Settings.Default.ExitToTray;

            OptionsToolStripMenuItem.DropDown.Closing += DropDown_Closing;

        }

        #region Helper Methods


        private void UpdateDeviceSelectors(object? sender = null, EventArgs? e = null)
        {

            _ = BeginInvoke(delegate ()
            {

                int primaryIndex = Devices.PrimaryOutput + 1, secondaryIndex = Devices.SecondaryOutput + 2, microphoneIndex = Devices.Microphone + 1;
                Devices.Refresh();
                try
                {
                    SecondaryOutputComboBox.SelectedIndex = secondaryIndex;
                    PrimaryOutputComboBox.SelectedIndex = primaryIndex;
                    MicrophoneSelectComboBox.SelectedIndex = microphoneIndex;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
            Thread.Sleep(1000);
            MicInjector.Refresh();




        }


        private void LoadFile()
        {
            if (fileName != string.Empty)
            {
                SoundItem.AllSounds.Clear();
                SoundItem.AllSounds.AddRange(Serializer.DeserializeFile(fileName));

            }
        }
        private void ExitApplication()
        {
            if (!saved)
            {
                UnsavedChanges prompt = new();
                var result = prompt.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    Save(false);
                }
            }
            Debug.WriteLine(Settings.Default.FileName);
            Settings.Default.Save();
            MicInjector.Stop();
            System.Windows.Forms.Application.Exit();
        }
        private void Save(bool newFile)
        {
            if (newFile || fileName == string.Empty)
            {
                SaveFileDialog saveFileSelector = new();
                //openfiledialog filter is only audio files
                saveFileSelector.Filter = "Apollo Soundboard files (*.asb)|*.asb";
                DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    fileName = saveFileSelector.FileName;
                    Serializer.SerializeToFile(SoundItem.AllSounds.ToList(), fileName);
                    saved = true;
                }
            }
            else
            {
                Serializer.SerializeToFile(SoundItem.AllSounds.ToList(), fileName);
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
            OpenFileDialog saveFileSelector = new();
            //openfiledialog filter is only audio files
            saveFileSelector.Filter = "Apollo Soundboard files (*.asb)|*.asb";
            DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                if (!saved)
                {
                    UnsavedChanges prompt = new();
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
                UnsavedChanges prompt = new();
                var result = prompt.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    Save(false);
                }
            }
            fileName = string.Empty;
            SoundItem.AllSounds.Clear();

        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void volumeMixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VolumeMixer mixer = new(this);
            _ = mixer.ShowDialog();
        }

        private void audioConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AudioConverter converter = new();
            _ = converter.ShowDialog();
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
            if (MicInjector.Enabled == MicInjectorToggle.Checked) return;
            BeginInvoke(() => MicInjector.Enabled = MicInjectorToggle.Checked);

        }

        private void AddSoundButton_Click(object sender, EventArgs e)
        {
            AddSoundPopup popup = new();
            var result = popup.ShowDialog();
            Debug.WriteLine(result);
            if (result == DialogResult.OK)
            {
                _ = new SoundItem(popup.Hotkeys, popup.FilePath, popup.SoundName, popup.Gain, popup.HotkeyOrderMatters);

                saved = false;
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (SoundGrid.SelectedRows.Count > 0)
            {

                var row = SoundGrid.SelectedRows[0];
                SoundItem sound = SoundItem.AllSounds[row.Index];

                AddSoundPopup popup = new(sound);
                var result = popup.ShowDialog();
                Debug.WriteLine(result);
                if (result == DialogResult.OK)
                {
                    sound.SoundName = popup.SoundName;
                    sound.SetHotkeys(popup.Hotkeys);
                    sound.FilePath = popup.FilePath;
                    sound.Gain = popup.Gain;
                    sound.HotkeyOrderMatters = popup.HotkeyOrderMatters;

                    saved = false;
                }


            }

        }



        #endregion

        #region Device Selectors

        private void PrimaryOutputComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Font is null) return;
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(PrimaryOutputComboBox.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void SecondaryOutputComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Font is null) return;
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(SecondaryOutputComboBox.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void MicrophoneSelectComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Font is null) return;
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(MicrophoneSelectComboBox.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }


        #endregion
        private void Soundboard_Load(object sender, EventArgs e)
        {
            menuStrip1.Renderer = new CustomRenderer(new TestColorTable());
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;


            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                if (Settings.Default.ExitToTray)
                {
                    e.Cancel = true;
                    Hide();
                    NotifyIcon.Visible = true;
                    NotifyIcon.BalloonTipText = "Your soundboard hotkeys will still work.";
                    NotifyIcon.BalloonTipTitle = "Apollo is running in the background";
                    NotifyIcon.ShowBalloonTip(500);
                }
            }
        }

        private void SoundGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Debug.WriteLine(e.RowIndex);
            if (e.RowIndex < 0) return;
            SoundItem sound = SoundItem.AllSounds[e.RowIndex];

            AddSoundPopup popup = new(sound);
            var result = popup.ShowDialog();
            Debug.WriteLine(result);
            if (result == DialogResult.OK)
            {
                sound.SetHotkeys(popup.Hotkeys);
                sound.FilePath = popup.FilePath;

                saved = false;
            }
        }

        private void eXPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                UnsavedChanges prompt = new();
                DialogResult response = prompt.ShowDialog();
                if (response == DialogResult.OK)
                {
                    Save(false);
                }
                if (response == DialogResult.No | response == DialogResult.OK)
                {
                    OpenFileDialog saveFileSelector = new();
                    //openfiledialog filter is only audio files
                    saveFileSelector.Filter = "JSON files (*.json)|*.json";
                    DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                    if (result == DialogResult.OK) // Test result.
                    {

                        saved = false;
                        fileName = string.Empty;
                        SoundItem.AllSounds.Clear();
                        SoundItem.AllSounds.AddRange(EXPImporter.Import(saveFileSelector.FileName));

                    }
                }

            } else
            {
                OpenFileDialog saveFileSelector = new();
                //openfiledialog filter is only audio files
                saveFileSelector.Filter = "JSON files (*.json)|*.json";
                DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {

                    saved = false;
                    fileName = string.Empty;
                    SoundItem.AllSounds.Clear();
                    SoundItem.AllSounds.AddRange(EXPImporter.Import(saveFileSelector.FileName));

                }
            }

        }

        private void soundpadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                UnsavedChanges prompt = new();
                DialogResult response = prompt.ShowDialog();
                if (response == DialogResult.OK) {
                    Save(false);
                }
                if (response == DialogResult.No | response == DialogResult.OK)
                {
                    OpenFileDialog saveFileSelector = new();
                    //openfiledialog filter is only audio files
                    saveFileSelector.Filter = "Soundpad files (*.spl)|*.spl";
                    DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                    if (result == DialogResult.OK) // Test result.
                    {

                        saved = false;
                        fileName = string.Empty;
                        SoundItem.AllSounds.Clear();
                        SoundItem.AllSounds.AddRange(SoundpadImporter.Import(saveFileSelector.FileName));

                    }
                }
            
            } else
            {
                OpenFileDialog saveFileSelector = new();
                //openfiledialog filter is only audio files
                saveFileSelector.Filter = "Soundpad files (*.spl)|*.spl";
                DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {

                    saved = false;
                    fileName = string.Empty;
                    SoundItem.AllSounds.Clear();
                    SoundItem.AllSounds.AddRange(SoundpadImporter.Import(saveFileSelector.FileName));

                }
            }

        }

        private void fromArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                UnsavedChanges prompt = new();
                DialogResult response = prompt.ShowDialog();
                if (response == DialogResult.OK)
                {
                    Save(false);
                }
                if (response == DialogResult.No | response == DialogResult.OK)
                {
                    OpenFileDialog saveFileSelector = new();
                    
                    //openfiledialog filter is only audio files
                    saveFileSelector.Filter = "Apollo Soundboard Archive files (*.asba)|*.asba";
                    DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                    if (result == DialogResult.OK) // Test result.
                    {
                        var archiveResult = Archiver.LoadFromArchive(saveFileSelector.FileName);

                        if (archiveResult != null)
                        {
                            SoundItem.AllSounds.Clear();
                            SoundItem.AllSounds.AddRange(archiveResult.Value.Sounds);
                            saved = true;
                            fileName = archiveResult.Value.SaveFile;
                        }
                    }
                }

            } else
            {
                OpenFileDialog saveFileSelector = new();
                //openfiledialog filter is only audio files
                saveFileSelector.Filter = "Apollo Soundboard Archive files (*.asba)|*.asba";
                DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    var archiveResult = Archiver.LoadFromArchive(saveFileSelector.FileName);

                    if (archiveResult != null)
                    {
                        SoundItem.AllSounds.Clear();
                        SoundItem.AllSounds.AddRange(archiveResult.Value.Sounds);
                        saved = true;
                        fileName = archiveResult.Value.SaveFile;
                    }


                }
            }

        }

        private void File_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                if (IsFileSupported(file))
                {
                    AddSoundPopup popup = new(file, Path.GetFileName(file), new List<Keys>());
                    var result = popup.ShowDialog();
                    Debug.WriteLine(result);
                    if (result == DialogResult.OK)
                    {
                        _ = new SoundItem(popup.Hotkeys, popup.SoundName, popup.FilePath, popup.Gain, popup.HotkeyOrderMatters);

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

                if (IsFileSupported(file))
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

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            Show();
            ExitApplication();
        }

        private void SoundGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            sortDirection = sortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

        }

        private void MicInjectorToggleHotkey_HotkeyAssigned(object sender, EventArgs e)
        {
            MicInjector.ToggleInjector = MicInjectorToggleHotkey.SelectedHotkeys;
        }

        public void ToggleMicInjector()
        {
            MicInjectorToggle.Checked = !MicInjectorToggle.Checked;
            
        }

        private void ExportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileSelector = new();
            //openfiledialog filter is only audio files
            saveFileSelector.Filter = "Apollo Soundboard Archive files (*.asba)|*.asba";
            DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                fileName = saveFileSelector.FileName;
                Archiver.Export(SoundItem.AllSounds.ToArray(), fileName);
            }
        }

        private void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            Debug.WriteLine(e.CloseReason);
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
            }
        }

        private void ExitToTray_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ExitToTray = ExitToTray.Checked;
            Settings.Default.Save();
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

    public class CustomRenderer : ToolStripProfessionalRenderer
    {
        public CustomRenderer(ProfessionalColorTable ColorTable)
            : base(ColorTable)
        {
        }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(e.ArrowRectangle.Location, e.ArrowRectangle.Size);
            r.Inflate(-2, -6);
            e.Graphics.DrawLines(Pens.White, new Point[]{
        new Point(r.Left, r.Top),
        new Point(r.Right, r.Top + r.Height /2),
        new Point(r.Left, r.Top+ r.Height)});
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
           // e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
            r.Inflate(-4, -6);
            e.Graphics.DrawLines(Pens.White, new Point[]{
        new Point(r.Left, r.Bottom - r.Height /2),
        new Point(r.Left + r.Width /3,  r.Bottom),
        new Point(r.Right, r.Top)});
        }
    }
}
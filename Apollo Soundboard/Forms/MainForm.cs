using Apollo.IO;
using Apollo.Properties;
using AutoUpdaterDotNET;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Forms;

namespace Apollo.Forms
{
    
    public partial class MainForm : Form
    {
        #region Properties

        private static readonly OptimizedBindingList<Sound> empty = new();

        private Soundboard _activeSoundboard;
        Soundboard ActiveSoundboard {
            set
            {
                _activeSoundboard?.Dispose();
                _activeSoundboard = value;
                var ActivePage = _activeSoundboard.ActivePage;
                if (ActivePage != null)
                {
                    SoundGrid.DataSource = ActivePage.Sounds;
                } else
                {
                    SoundGrid.DataSource = empty;
                }
                PageSwitchGrid.DataSource = _activeSoundboard.Pages;
                _activeSoundboard.PageChanged += (s,e) => SoundGrid.DataSource = _activeSoundboard.ActivePage?.Sounds ?? empty;
            }
            get => _activeSoundboard;
        }

        string _fileName = "";
        string FileName
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
        bool Saved
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

        private static string _Cycle = Settings.Default.CycleHotkey;

        public static List<Keys> CycleHotkeys
        {
            get
            {

                var list = new List<Keys>();
                if (_Cycle == string.Empty) return list;
                int[] keycodes = Array.ConvertAll(_Cycle.Split(","), int.Parse);
                foreach (int i in keycodes)
                {
                    list.Add((Keys)i);
                }
                return list;
            }
            set
            {
                _Cycle = String.Join(",", value.ConvertAll(i => (int)i));
                Settings.Default.CycleHotkey = _Cycle;
                Settings.Default.Save();
            }
        }



        #endregion

        #region Managers

        public static MainForm Instance { get; set; }

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


        public MainForm(string? file)
        {

            AutoUpdater.Synchronous = true;
            AutoUpdater.Start("https://raw.githubusercontent.com/manninxn/Apollo-Soundboard/master/Apollo%20Soundboard/version.xml");
            

            if (Instance is not null) return;

            Instance = this;
            InitializeComponent();

            if (file != null && Path.GetExtension(file) == ".asba")
            {
                var result = Archiver.LoadFromArchive(file);

                if (result != null)
                {
                    ActiveSoundboard = result.Value.soundboard;
                    Saved = true;
                    FileName = result.Value.SaveFile;
                }
            }
            else
            {
                FileName = file ?? Settings.Default.FileName;
            }
            LoadFile();
            //ActiveSoundboard = ActiveSoundboard ?? new();

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

            PageSwitchGrid.Columns[0].FillWeight = 60;
            PageSwitchGrid.Columns[1].FillWeight = 25;

            PageSwitchGrid.Columns[0].HeaderText = "Page";
            PageSwitchGrid.Columns[1].HeaderText = "#";

            PageSwitchGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SetDoubleBuffer(SoundGrid, true);

            DataGridViewButtonColumn RemoveColumn = new();
            RemoveColumn.FlatStyle = FlatStyle.Flat;
            RemoveColumn.DefaultCellStyle.NullValue = "X";
            RemoveColumn.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            RemoveColumn.Name = "";
            RemoveColumn.CellTemplate.Style.ForeColor = Color.FromArgb(200,12,12);
            RemoveColumn.FillWeight = 15;
            int columnIndex = 2;
            PageSwitchGrid.Columns.Insert(columnIndex, RemoveColumn);


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

            InputHandler.Subscribe();

            StopAllHotkeySelector.Text = String.Join("+", Sound.ClearSounds.Select(i => KeyMap.KeyToChar(i)).ToList());
            MicInjectorToggleHotkey.Text = String.Join("+", MicInjector.ToggleInjector.Select(i => KeyMap.KeyToChar(i)).ToList());
            CycleSelector.Text = String.Join("+", CycleHotkeys.Select(i => KeyMap.KeyToChar(i)).ToList());
            Devices.DevicesUpdated += UpdateDeviceSelectors;


            MicInjectorToggle.Checked = MicInjector.Initialize();

            ExitToTray.Checked = Settings.Default.ExitToTray;
            AlwaysOnTop.Checked = Settings.Default.AlwaysOnTop;

            TopMost = Settings.Default.AlwaysOnTop;


            OptionsToolStripMenuItem.DropDown.Closing += DropDown_Closing;

            InputHandler.PressedKeysChanged += (s, e) =>
            {
                if (InputHandler.CheckHotkeys(MicInjector.ToggleInjector) && e.KeyDown) ToggleMicInjector();
                if (InputHandler.CheckHotkeys(CycleHotkeys) && e.KeyDown) CycleSoundboard();
            };


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
            if (FileName != string.Empty)
            {
                ActiveSoundboard = Serializer.DeserializeFile(FileName);

            }
        }
        private void ExitApplication()
        {
            if (!Saved)
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
            if (newFile || FileName == string.Empty)
            {
                SaveFileDialog saveFileSelector = new();
                //openfiledialog filter is only audio files
                saveFileSelector.Filter = "Apollo Soundboard files (*.asb)|*.asb";
                DialogResult result = saveFileSelector.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    FileName = saveFileSelector.FileName;
                    Serializer.SerializeToFile(ActiveSoundboard, FileName);
                    Saved = true;
                }
            }
            else
            {
                Serializer.SerializeToFile(ActiveSoundboard, FileName);
                Saved = true;
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
                if (!Saved)
                {
                    UnsavedChanges prompt = new();
                    if (prompt.ShowDialog() == DialogResult.Yes)
                    {
                        Save(false);
                    }
                }
                FileName = saveFileSelector.FileName;
                LoadFile();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Saved)
            {
                UnsavedChanges prompt = new();
                var result = prompt.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    Save(false);
                }
            }
            FileName = string.Empty;
            ActiveSoundboard = new Soundboard();

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
            Saved = false;
            if (SoundGrid.SelectedRows.Count > 0)
            {

                var row = SoundGrid.SelectedRows[0];
                var ActivePage = ActiveSoundboard.ActivePage;
                if (ActivePage == null) return;
                Sound sound = ActivePage.Sounds[row.Index];
                ActivePage.Sounds.RemoveAt(row.Index);
                sound.Dispose();

                

            }
        }
        private void StopAll_Click(object sender, EventArgs e)
        {
            Sound.StopAllSounds();
        }

        private void StopAllHotkeySelector_HotkeyAssigned(object sender, EventArgs e)
        {
            Sound.ClearSounds = StopAllHotkeySelector.SelectedHotkeys;
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
                var Sound = new Sound(popup.Hotkeys, popup.FilePath, popup.SoundName, popup.Gain, popup.HotkeyOrderMatters, 0, popup.OverlapSelf);
                if (ActiveSoundboard.ActivePage == null) ActiveSoundboard.NewPage();
                ActiveSoundboard.ActivePage?.AddSound(Sound);
                Sound.SetOwner(ActiveSoundboard);
                Saved = false;
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (SoundGrid.SelectedRows.Count > 0)
            {

                var row = SoundGrid.SelectedRows[0];
                var ActivePage = ActiveSoundboard.ActivePage;
                if (ActivePage == null) return;
                Sound sound = ActivePage.Sounds[row.Index];

                AddSoundPopup popup = new(sound);
                var result = popup.ShowDialog();
                Debug.WriteLine(result);
                if (result == DialogResult.OK)
                {
                    if(!popup.FilePath.Equals(sound.FilePath))
                    {
                        sound.TimesPlayed = 0;
                    }
                    sound.SoundName = popup.SoundName;
                    sound.SetHotkeys(popup.Hotkeys);
                    sound.FilePath = popup.FilePath;
                    sound.Gain = popup.Gain;
                    sound.HotkeyOrderMatters = popup.HotkeyOrderMatters;
                    sound.OverlapSelf = popup.OverlapSelf;

                    Saved = false;
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

            if (PageSwitchGrid.RowCount > 0)
            {
                PageSwitchGrid.Rows[0].Selected = true;
            }
            SoundGrid.ClearSelection();

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
                } else
                {
                    ExitApplication();
                }
            }
        }

        private void SoundGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Debug.WriteLine(e.RowIndex);
            if (e.RowIndex < 0) return;
            var ActivePage = ActiveSoundboard.ActivePage;
            if (ActivePage == null) return;
            Sound sound = ActivePage.Sounds[e.RowIndex];

            AddSoundPopup popup = new(sound);
            var result = popup.ShowDialog();
            Debug.WriteLine(result);
            if (result == DialogResult.OK)
            {
                sound.SetHotkeys(popup.Hotkeys);
                sound.FilePath = popup.FilePath;

                Saved = false;
            }
        }

        private void eXPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Saved)
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

                        Saved = false;
                        FileName = string.Empty;
                        ActiveSoundboard = Soundboard.FromExp(saveFileSelector.FileName);
                        

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

                    Saved = false;
                    FileName = string.Empty;
                    ActiveSoundboard = Soundboard.FromExp(saveFileSelector.FileName);

                }
            }

        }

        private void soundpadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Saved)
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

                        Saved = false;
                        FileName = string.Empty;
                        ActiveSoundboard = Soundboard.FromSoundpad(saveFileSelector.FileName);

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

                    Saved = false;
                    FileName = string.Empty;
                    ActiveSoundboard = Soundboard.FromSoundpad(saveFileSelector.FileName);

                }
            }

        }

        private void fromArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Saved)
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
                            ActiveSoundboard = archiveResult.Value.soundboard;
                            Saved = true;
                            FileName = archiveResult.Value.SaveFile;
                            Debug.WriteLine(FileName);
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
                        ActiveSoundboard = archiveResult.Value.soundboard;
                        Saved = true;
                        FileName = archiveResult.Value.SaveFile;
                    }


                }
            }

        }

        private void File_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data;
            if (data == null) return;
            if (!data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[])data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                if (IsFileSupported(file))
                {
                    AddSoundPopup popup = new(file, Path.GetFileName(file), new List<Keys>());
                    var result = popup.ShowDialog();
                    Debug.WriteLine(result);
                    if (result == DialogResult.OK)
                    {
                        var Sound = new Sound(popup.Hotkeys, popup.SoundName, popup.FilePath, popup.Gain, popup.HotkeyOrderMatters);
                        if (ActiveSoundboard.ActivePage == null) ActiveSoundboard.NewPage();
                        ActiveSoundboard.ActivePage?.AddSound(Sound);
                        Sound.SetOwner(ActiveSoundboard);
                        Saved = false;
                    }
                    return;
                }
            }
        }

        private void File_DragEnter(object sender, DragEventArgs e)
        {
            var data = e.Data;
            if (data == null) return;
            if (data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[]) data.GetData(DataFormats.FileDrop);
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
                FileName = saveFileSelector.FileName;
                Archiver.Export(ActiveSoundboard, FileName);
            }
        }

        private void DropDown_Closing(object? sender, ToolStripDropDownClosingEventArgs e)
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

        private void AlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AlwaysOnTop = AlwaysOnTop.Checked;
            TopMost = AlwaysOnTop.Checked;
            Settings.Default.Save();
        }

        private void PageSwitchGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {



            if (e.RowIndex < 0) return;
            if (e.ColumnIndex > 0)
            {
                ActiveSoundboard.ActivePageNumber = e.RowIndex;
                
            }

        }

        private void PageSwitchGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 0)
            {
                if (e.RowIndex == ActiveSoundboard.ActivePageNumber) CycleSoundboard();
                ActiveSoundboard.Pages.RemoveAt(e.RowIndex);
                
                
            }
            
        }

        public void CycleSoundboard()
        {
            ActiveSoundboard.NextPage();
            PageSwitchGrid.Rows[ActiveSoundboard.ActivePageNumber].Selected = true;
        }

        private void CycleSelector_HotkeyAssigned(object sender, EventArgs e)
        {
            CycleHotkeys = CycleSelector.SelectedHotkeys;
        }

        static void SetDoubleBuffer(Control dgv, bool DoubleBuffered)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgv, new object[] { DoubleBuffered });
        }

        private void PageSwitchGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 1)
            {
                PageSwitchGrid.BeginEdit(false);


            }
        }

        private void PageSwitchGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Saved = false;
        }

        private void NewPage_Click(object sender, EventArgs e)
        {
            ActiveSoundboard.NewPage();
            PageSwitchGrid.Rows[ActiveSoundboard.ActivePageNumber].Selected = true;

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
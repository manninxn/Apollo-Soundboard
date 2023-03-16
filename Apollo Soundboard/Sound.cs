using Apollo.Forms;
using Apollo.IO;
using Apollo.Properties;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using static Apollo.IO.Archiver;

namespace Apollo
{
    public class Sound : IDisposable
    {

        private static string _ClearSounds = Settings.Default.StopAllSoundsHotkey;

        public int TimesPlayed = 0;
        public static List<Keys> ClearSounds
        {
            get
            {

                var list = new List<Keys>();
                if (_ClearSounds == string.Empty) return list;
                int[] keycodes = Array.ConvertAll(_ClearSounds.Split(","), int.Parse);
                foreach (int i in keycodes)
                {
                    list.Add((Keys)i);
                }
                return list;
            }
            set
            {
                _ClearSounds = String.Join(",", value.ConvertAll(i => (int)i));
                Settings.Default.StopAllSoundsHotkey = _ClearSounds;
                Settings.Default.Save();
            }
        }

        private static List<WaveOut> PlayingSounds = new();

        private string _path = string.Empty;
        [Browsable(false)]
        public string FilePath
        {
            get => _path;
            set  {
                Debug.WriteLine("Path: " + value);
                _path = value;

                    var ext = System.IO.Path.GetExtension(value);
                Debug.WriteLine("Extension: " + ext);
                   WaveStream reader = ext switch
                   {
                      ".ogg" => new VorbisWaveReader(value),
                        _ => new AudioFileReader(value)
                    };
                    _length = TimeSpan.FromSeconds(Math.Ceiling(reader.TotalTime.TotalSeconds));
                    reader.Dispose();
                

            }
        }

        private string _soundName;
        public string SoundName
        {
            get => string.IsNullOrEmpty(_soundName) ? Path.GetFileName(FilePath) : _soundName;
            set => _soundName = value;
        }
        public string Hotkey
        {
            get
            {
                return String.Join("+", Hotkeys.Select(i => KeyMap.KeyToChar(i)).ToList());
            }
        }

        private TimeSpan _length;
        public string Length
        {
            get
            {
                return String.Format("{0:mm\\:ss}", _length);
                //return _length.ToString();
            }
        }

        public float Gain = 0;

        public bool HotkeyOrderMatters = false;

        private List<Keys> Hotkeys { get; set; } = new List<Keys>();

        private Soundboard Owner;

        private bool Active
        {
            get
            {
                if (Owner == null) return false;

                if (!Owner.Active) return false;

                if(Owner.ActivePage?.Sounds.Contains(this) ?? false) return true;

                return false;

            }
        }
        public Sound() { }

        public Sound(List<Keys> _KeyCodes, string _FilePath, string _soundName, float _Gain = 0, bool _HotkeyOrderMatters = false, int timesPlayed = 0)
        {
            Hotkeys = _KeyCodes;
            FilePath = _FilePath;
            SoundName = _soundName;
            Gain = _Gain;
            HotkeyOrderMatters = _HotkeyOrderMatters;
            TimesPlayed = timesPlayed;
            InputHandler.PressedKeysChanged += OnPressedKeysChanged;
        }
        public Sound(SoundData data)
        {
            Hotkeys = data.Hotkeys.Select(i => (Keys)i).ToList();
            FilePath = data.FilePath;
            SoundName = data.SoundName;
            Gain = data.Gain;
            HotkeyOrderMatters = data.HotkeyOrderMatters;
            TimesPlayed = data.TimesPlayed;
            InputHandler.PressedKeysChanged += OnPressedKeysChanged;

        }

        public Sound(SoundMetadata data)
        {
            Hotkeys = data.Hotkeys.Select(i => (Keys)i).ToList();
            SoundName = data.SoundName;
            Gain = data.Gain;
            HotkeyOrderMatters = data.HotkeyOrderMatters;
            TimesPlayed = data.TimesPlayed;
            InputHandler.PressedKeysChanged += OnPressedKeysChanged;

        }

        ~Sound() {
            Dispose();
        }

        public void SetOwner(Soundboard soundboard)
        {
            Owner = soundboard;
        }

        public void OnPressedKeysChanged(object? sender, PressedKeysEventArgs e)
        {
            var PressedKeys = e.PressedKeys;

            if (GetHotkeys().Count == 0) return;

            var lastN = PressedKeys.TakeLast(GetHotkeys().Count());

            if (HotkeyOrderMatters ? lastN.SequenceEqual(GetHotkeys()) : Enumerable.SequenceEqual(lastN.OrderBy(e => e), GetHotkeys().OrderBy(e => e)))
            {
                Play();
            }
        }


        public void Dispose()
        {
            InputHandler.PressedKeysChanged -= OnPressedKeysChanged;
        }



        private void PlayThroughDevice(string filePath, int Device, float gain)
        {
            WaveOut output = new() { DeviceNumber = Device };

            PlayingSounds.Add(output);
            var ext = Path.GetExtension(filePath);
            WaveStream reader = ext switch
            {
                ".ogg" => new VorbisWaveReader(FilePath),
                _ => new AudioFileReader(FilePath)
            };




            try
            {

                var volumeSampleProvider = new VolumeSampleProvider(reader.ToSampleProvider());
                volumeSampleProvider.Volume = gain;

                output.PlaybackStopped += (object? o, StoppedEventArgs a) =>
                {
                    

                    _ = PlayingSounds.Remove(output);
                    reader.Dispose();
                    output.Dispose();
                };


                output.Init(volumeSampleProvider);

                output.Play();



            }
            catch
            {
                //I'm honestly not sure if this is needed but it seems like it is
                output.Dispose();
                if (reader != null) { reader.Dispose(); }

                Debug.WriteLine("Can't keep up! Too many sounds playing at once probably.");
            }

        }

        public void Play(bool overrideActive = false)
        {
            if(!Active && !overrideActive) return;
            try
            {

                if (MainForm.Devices.SecondaryOutput != -2)
                    PlayThroughDevice(FilePath, MainForm.Devices.SecondaryOutput, (1 + Settings.Default.SecondaryGain) * (1 + Gain));

                PlayThroughDevice(FilePath, MainForm.Devices.PrimaryOutput, (1 + Settings.Default.PrimaryGain) * (1 + Gain));

                TimesPlayed++;

            } catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public List<Keys> GetHotkeys()
        {
            return Hotkeys;
        }
        public void SetHotkeys(List<Keys> hotkeys)
        {
            Hotkeys = hotkeys;
        }

        public static void StopAllSounds()
        {
            foreach (WaveOut sound in PlayingSounds)
            {
                sound.Stop();
            }
        }


    }

}

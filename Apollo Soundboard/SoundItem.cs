using Apollo_Soundboard.Properties;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace Apollo_Soundboard
{
    public class SoundItem
    {
        public static string _ClearSounds = Settings.Default.StopAllSoundsHotkey;

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



        public static List<SoundItem> AllSounds = new List<SoundItem>();

        private static List<WaveOut> PlayingSounds = new List<WaveOut>();

        private static Soundboard form;

        public string FilePath;
        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }
        public string Hotkey
        {
            get
            {
                return String.Join("+", Hotkeys.Select(i => KeyMap.KeyToChar(i)).ToList());
            }
        }
        public float Gain = 0;

        public bool HotkeyOrderMatters = false;

        private List<Keys> Hotkeys { get; set; }

        public static void SetForm(Soundboard _form)
        {
            form = _form;
        }

        public SoundItem() { }

        public SoundItem(List<Keys> _KeyCodes, string _FilePath, float _Gain = 0, bool _HotkeyOrderMatters = false)
        {
            Hotkeys = _KeyCodes;
            FilePath = _FilePath;
            Gain = _Gain;
            HotkeyOrderMatters = _HotkeyOrderMatters;
            AllSounds.Add(this);
        }

        public void Destroy()
        {
            AllSounds.Remove(this);
            form.RefreshGrid();
        }


        private void PlayThroughDevice(string filePath, int Device, float gain)
        {
            Debug.WriteLine(Device);
            WaveOut output = new() { DeviceNumber = Device };

            PlayingSounds.Add(output);

            AudioFileReader? reader = null;

            try
            {

                reader = new AudioFileReader(FilePath);

                var volumeSampleProvider = new VolumeSampleProvider(reader.ToSampleProvider());
                volumeSampleProvider.Volume = gain;

                output.PlaybackStopped += (object? o, StoppedEventArgs a) =>
                {

                    PlayingSounds.Remove(output);
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

        public void Play()
        {
            Debug.WriteLine($"Gain: {Gain}");
            if (Devices.SecondaryOutput != -2)
                PlayThroughDevice(FilePath, Devices.SecondaryOutput, (1 + Settings.Default.SecondaryGain) * (1 + Gain));

            PlayThroughDevice(FilePath, Devices.PrimaryOutput, (1 + Settings.Default.PrimaryGain) * (1 + Gain));

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

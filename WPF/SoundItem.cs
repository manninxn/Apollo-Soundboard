using Apollo.Forms;
using Apollo.Properties;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;
namespace Apollo
{
    public class SoundItem
    {

        private static string _ClearSounds = Settings.Default.StopAllSoundsHotkey;

        public static List<Key> ClearSounds
        {
            get
            {

                var list = new List<Key>();
                if (_ClearSounds == string.Empty) return list;
                int[] keycodes = Array.ConvertAll(_ClearSounds.Split(","), int.Parse);
                foreach (int i in keycodes)
                {
                    list.Add((Key)i);
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


        public static OptimizedBindingList<SoundItem> AllSounds = new();

        private static List<WaveOut> PlayingSounds = new();

        public string FilePath = string.Empty;

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
        public float Gain = 0;

        public bool HotkeyOrderMatters = false;

        private List<Key> Hotkeys { get; set; } = new List<Key>();


        public SoundItem() { }

        public SoundItem(List<Key> _KeyCodes, string _FilePath, string _soundName, float _Gain = 0, bool _HotkeyOrderMatters = false)
        {
            Hotkeys = _KeyCodes;
            FilePath = _FilePath;
            SoundName = _soundName;
            Gain = _Gain;
            HotkeyOrderMatters = _HotkeyOrderMatters;
            AllSounds.Add(this);
        }

        public void Destroy()
        {
            _ = AllSounds.Remove(this);
        }



        private void PlayThroughDevice(string filePath, int Device, float gain)
        {
            Debug.WriteLine(Device);
            WaveOut output = new() { DeviceNumber = Device };

            PlayingSounds.Add(output);
            var ext = System.IO.Path.GetExtension(filePath);
            WaveStream reader = ext switch
            {
                ".ogg" => new VorbisWaveReader(FilePath),
                _ => new AudioFileReader(FilePath)
            };




            try
            {

                Debug.WriteLine(FilePath.TakeLast(3));
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

        public void Play()
        {
            Debug.WriteLine($"Gain: {Gain}");
            if (Soundboard.Devices.SecondaryOutput != -2)
                PlayThroughDevice(FilePath, Soundboard.Devices.SecondaryOutput, (1 + Settings.Default.SecondaryGain) * (1 + Gain));

            PlayThroughDevice(FilePath, Soundboard.Devices.PrimaryOutput, (1 + Settings.Default.PrimaryGain) * (1 + Gain));

        }

        public List<Key> GetHotkeys()
        {
            return Hotkeys;
        }
        public void SetHotkeys(List<Key> hotkeys)
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

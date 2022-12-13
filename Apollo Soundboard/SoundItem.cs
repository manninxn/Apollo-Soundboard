using Apollo_Soundboard.Properties;
using NAudio.Wave;
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
        public string FilePath { get; set; }
        public string Hotkey { 
            get {
                return String.Join("+", Hotkeys.Select(i => i.ToString()).ToList()); 
            } 
        }
        private List<Keys> Hotkeys { get; set; }

        public static void SetForm(Soundboard _form)
        {
            form = _form;
        }

        public SoundItem() { }

        public SoundItem(List<Keys> _KeyCodes, string _FilePath)
        {
            Hotkeys = _KeyCodes;
            FilePath = _FilePath;
            AllSounds.Add(this);
        }

        public void Destroy()
        {
            AllSounds.Remove(this);
            form.RefreshGrid();
        }

        private void PlayThroughDevice(string filePath, int deviceId, float gain)
        {
            try
            {

                var reader = new AudioFileReader(FilePath);
                reader.Volume = gain;
                var waveOut = new WaveOut();
                waveOut.DeviceNumber = deviceId;
                waveOut.Init(reader);
                waveOut.Play();
                PlayingSounds.Add(waveOut);
                waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(delegate (object o, StoppedEventArgs a)
                {
                    PlayingSounds.Remove(waveOut);
                    reader.Dispose();
                    waveOut.Dispose();
                });
            }
            catch
            {
                Debug.WriteLine("Can't keep up! Too many sounds playing at once probably.");
            }

        }

        public void Play()
        {
            PlayThroughDevice(FilePath, form.primaryOutput, Settings.Default.PrimaryGain);
            if (form.secondaryOutput > 0)
                PlayThroughDevice(FilePath, form.secondaryOutput - 1, Settings.Default.SecondaryGain);
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
            PlayingSounds.Clear();
        }


    }
}

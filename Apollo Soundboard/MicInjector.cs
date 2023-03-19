using Apollo.Forms;
using Apollo.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
namespace Apollo
{

    public class MicInjector
    {

        private static string _ToggleInjector = Settings.Default.MicInjectorHotkey;

        public static List<Keys> ToggleInjector
        {
            get
            {

                var list = new List<Keys>();
                if (_ToggleInjector == string.Empty) return list;
                int[] keycodes = Array.ConvertAll(_ToggleInjector.Split(","), int.Parse);
                foreach (int i in keycodes)
                {
                    list.Add((Keys)i);
                }
                return list;
            }
            set
            {
                _ToggleInjector = String.Join(",", value.ConvertAll(i => (int)i));
                Settings.Default.MicInjectorHotkey = _ToggleInjector;
                Settings.Default.Save();
            }
        }

        WasapiCapture? micStream; WasapiOut? virtualCable;

        private bool _Enabled;

        private bool _running = false;

        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                Settings.Default.MicInjector = value;
                Settings.Default.Save();
                if (value)
                {
                    if (!_Enabled)
                    {
                        _Enabled = true;
                        
                        Start();
                    }

                }
                else
                {
                    _Enabled = false;
                    Stop();
                }
            }
        }


        public bool Initialize()
        {
            Enabled = Settings.Default.MicInjector;

            return Enabled;
        }

        private void Start()
        {

            if (MainForm.Devices.SecondaryOutput == -2 | _running) return;
            _running = true;
            micStream = new(MainForm.Devices.MicrophoneDevice?.MMDevice, true, 50)
            {
                WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(MainForm.Devices.Microphone).Channels)
            };

            WaveInProvider waveIn = new(micStream);


            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider())
            {
                Volume = 1 + Settings.Default.MicrophoneGain
            };

            virtualCable = new(MainForm.Devices.SecondaryDevice?.MMDevice, AudioClientShareMode.Shared, true, 50);
            virtualCable.Init(volumeSampleProvider);

            micStream.StartRecording();
            virtualCable.Play();

        }
        public void Stop()
        {
            _running = false;

            virtualCable?.Stop();
            micStream?.StopRecording();

            virtualCable?.Dispose();
            micStream?.Dispose();

            micStream = null;
            virtualCable = null;
        }
        public void Refresh()
        {
            var enabled = Enabled;
            Enabled = false;
            Enabled = enabled;
        }


    }
}

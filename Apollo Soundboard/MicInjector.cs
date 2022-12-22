using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Apollo_Soundboard
{

    public class MicInjector
    {
        WasapiCapture? micStream; WasapiOut? virtualCable;
        Thread? micInjectorThread;
        private bool _Enabled;

        private bool _running = false;

        public bool Enabled
        {
            get { return _Enabled; }
            set
            {

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

        public MicInjector() { }
        public bool Initialize()
        {
            Enabled = Settings.Default.MicInjector;
            return Enabled;
        }

        private void Start()
        {

            if (Soundboard.Devices.SecondaryOutput == -2 | _running) return;
            _running = true;
            micStream = new(Soundboard.Devices.MicrophoneDevice.MMDevice, true, 50);

            micStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Soundboard.Devices.Microphone).Channels);

            WaveInProvider waveIn = new(micStream);


            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = 1 + Settings.Default.MicrophoneGain;

            virtualCable = new(Soundboard.Devices.SecondaryDevice.MMDevice, AudioClientShareMode.Shared, true, 50);
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

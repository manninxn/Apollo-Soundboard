using Apollo_Soundboard.Properties;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace Apollo_Soundboard
{

    public class MicInjector
    {
        WaveInEvent? micStream; WaveOutEvent? virtualCable;

        private bool _Enabled = Settings.Default.MicInjector;


        public bool Enabled
        {
            get { return _Enabled; }
            set
            {

                if (value)
                {
                    if (!_Enabled)
                        Start();
                }
                else
                {
                    Stop();
                }
                _Enabled = value;
            }
        }

        public MicInjector() { }
        public void Initialize()
        {
            if (_Enabled) Start();
            // Soundboard.Devices.DevicesUpdated += Devices_DevicesUpdated;
        }

        private void Devices_DevicesUpdated(object? sender, EventArgs e)
        {

            Debug.WriteLine("Updated");
        }

        public void Start()
        {

            if (Soundboard.Devices.SecondaryOutput == -2) return;

            micStream = new();



            micStream.BufferMilliseconds = 100;
            micStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Soundboard.Devices.Microphone).Channels);

            micStream.DeviceNumber = Soundboard.Devices.Microphone;

            WaveInProvider waveIn = new(micStream);


            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = 1 + Settings.Default.MicrophoneGain;


            virtualCable = new();
            virtualCable.DesiredLatency = 100;
            virtualCable.DeviceNumber = Soundboard.Devices.SecondaryOutput;
            virtualCable.Init(volumeSampleProvider);

            micStream.StartRecording();
            virtualCable.Play();

        }
        public void Stop()
        {

            try
            {
                if (micStream != null && virtualCable != null)
                {


                    micStream.Dispose();
                    micStream = null;

                    virtualCable.Dispose();
                    virtualCable = null;

                }
            }
            catch
            {
                micStream = null;

                virtualCable = null;
            }
        }
        public void Refresh()
        {
            bool temp = Enabled;
            Enabled = false;
            Enabled = temp;
        }

    }
}

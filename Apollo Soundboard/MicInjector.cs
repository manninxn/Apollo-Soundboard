using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Apollo_Soundboard
{
    
    public static class MicInjector
    {
        static WaveInEvent? micStream; static WaveOutEvent? virtualCable;

        private static bool _Enabled = Settings.Default.MicInjector;


        public static bool Enabled
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

        public static void Initialize()
        {
            if(_Enabled) Start();
            Devices.DevicesUpdated += Devices_DevicesUpdated;
        }

        private static void Devices_DevicesUpdated(object? sender, EventArgs e)
        {

            Debug.WriteLine("Updated");
        }

        public static void Start()
        {
            
            if (Devices.SecondaryOutput == -2) return;

            micStream = new();

            

            micStream.BufferMilliseconds = 50;
            micStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Devices.Microphone).Channels);

            micStream.DeviceNumber = Devices.Microphone;

            WaveInProvider waveIn = new(micStream);

            
            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = 1 + Settings.Default.MicrophoneGain;

            virtualCable = new();
            virtualCable.DesiredLatency = 150;
            virtualCable.DeviceNumber = Devices.SecondaryOutput;
            virtualCable.Init(volumeSampleProvider);

            micStream.StartRecording();
            virtualCable.Play();

        }
        public static void Stop()
        {
            try
            {
                if (micStream != null && virtualCable != null)
                {


                    micStream.Dispose();

                    virtualCable.Dispose();

                    micStream = null;

                    virtualCable = null;

                }
            } catch
            {
                micStream = null;

                virtualCable = null;
            }
        }
        public static void Refresh()
        {
            Stop();
            if (Enabled) Start();
        }

    }
}

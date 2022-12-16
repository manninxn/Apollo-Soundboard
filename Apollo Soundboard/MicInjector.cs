using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace Apollo_Soundboard
{
    public static class MicInjector
    {
        static WaveInEvent? micStream; static DirectSoundOut? virtualCable;

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

        static MicInjector()
        {
            Enabled = _Enabled;
        }


        public static void Start()
        {
            if (Soundboard.secondaryOutput == -2) return;

            micStream = new WaveInEvent();

            

            micStream.BufferMilliseconds = 50;
            micStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Soundboard.Microphone).Channels);

            var enumerator = new MMDeviceEnumerator();
            if(Soundboard.Microphone > -1) Debug.WriteLine($"{Soundboard.Microphone}: {WaveIn.GetCapabilities(Soundboard.Microphone).ProductName} || {enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[Soundboard.Microphone].FriendlyName}");

            enumerator.Dispose();


            micStream.DeviceNumber = Soundboard.Microphone;

            WaveInProvider waveIn = new(micStream);


            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = 1 + Settings.Default.MicrophoneGain;


            virtualCable = new(DirectSoundOut.Devices.ElementAt(Soundboard.secondaryOutput + 1).Guid);

            virtualCable.Init(volumeSampleProvider);

            micStream.StartRecording();
            virtualCable.Play();

        }
        public static void Stop()
        {

            if (micStream != null && virtualCable != null)
            {
                micStream.StopRecording();
                virtualCable.Stop();
                micStream.Dispose();
                virtualCable.Dispose();
                Debug.WriteLine("Stopped");
            }
        }
        public static void Refresh()
        {
            Stop();
            if (Enabled) Start();
        }

    }
}

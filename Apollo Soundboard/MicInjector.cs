using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Diagnostics;

namespace Apollo_Soundboard
{
    public static class MicInjector
    {
        static WasapiCapture? micStream; static WasapiOut? virtualCable;

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
            if (Soundboard.secondaryOutput == null) return;

            micStream = new(Soundboard.Microphone);
                
            WaveInProvider waveIn = new(micStream);
            

            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = 1 + Settings.Default.MicrophoneGain;


            virtualCable = new(Soundboard.secondaryOutput, AudioClientShareMode.Shared, true, 30);

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

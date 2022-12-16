using Microsoft.VisualBasic;
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Data;
using System.Diagnostics;

namespace Error
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MicInjector.Enabled = checkBox1.Checked;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MicInjector.Enabled = checkBox1.Checked;
        }
    }

    public static class MicInjector
    {
        static WaveInEvent? micStream; static WaveOutEvent? virtualCable;

        private static bool _Enabled = false;


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
            micStream = new();



            micStream.BufferMilliseconds = 50;
            micStream.DeviceNumber = -1;

            WaveInProvider waveIn = new(micStream);


            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());


            virtualCable = new();
            virtualCable.DeviceNumber = -1;
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

            }
        }
        public static void Refresh()
        {
            Stop();
            if (Enabled) Start();
        }

    }
}
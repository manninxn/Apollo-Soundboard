using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using ReproducingError.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReproducingError
{
    public partial class Form1 : Form
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

        public static void Start()
        {

            micStream = new WaveInEvent()
            {
                BufferMilliseconds = 50,
                WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Devices.Microphone).Channels)
            };

            var enumerator = new MMDeviceEnumerator();
            if (Devices.Microphone > -1) Debug.WriteLine($"{Devices.Microphone}: {WaveIn.GetCapabilities(Devices.Microphone).ProductName} || {enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[Devices.Microphone].FriendlyName}");

            enumerator.Dispose();


            micStream.DeviceNumber = Devices.Microphone;

            WaveInProvider waveIn = new(micStream);


            var volumeSampleProvider = new VolumeSampleProvider(waveIn.ToSampleProvider());
            volumeSampleProvider.Volume = 1 + Settings.Default.MicrophoneGain;


            virtualCable = new();
            virtualCable.DeviceNumber = Devices.SecondaryOutput;
            virtualCable.Init(volumeSampleProvider);

            micStream.StartRecording();
            virtualCable.Play();

        }
        public static void Stop()
        {

            if (micStream != null && virtualCable != null)
            {

                micStream.StopRecording();
                Debug.WriteLine("A");
                virtualCable.Stop();
                Debug.WriteLine("A");
                // micStream.Dispose();
                Debug.WriteLine("A");
                //virtualCable.Dispose();
                Debug.WriteLine("A");
                Debug.WriteLine("Stopped");
                Debug.WriteLine("A");
            }
        }
        public static void Refresh()
        {
            Stop();
            if (Enabled) Start();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}

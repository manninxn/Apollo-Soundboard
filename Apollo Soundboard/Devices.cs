using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Apollo_Soundboard
{
    public static class Devices
    {
        static int _primary = Settings.Default.PrimaryOutput;
        public static int PrimaryOutput
        {
            get
            {
                return _primary;
            }
            set
            {
                _primary = value;
                Settings.Default.PrimaryOutput = value;
                Settings.Default.Save();

            }
        }

        static int _secondary = Settings.Default.SecondaryOutput;
        public static int SecondaryOutput
        {
            get
            {
                return _secondary;
            }
            set
            {
                _secondary = value;
                Settings.Default.SecondaryOutput = value;
                Settings.Default.Save();
            }
        }


        static int _microphone = Settings.Default.Microphone;
        public static int Microphone
        {
            get
            {
                return _microphone;
            }
            set
            {
                _microphone = value;
                Settings.Default.Microphone = value;
                Settings.Default.Save();
            }
        }

        public static SortedDictionary<int, string> PrimaryDevices = new();
        public static SortedDictionary<int, string> SecondaryDevices = new() { { -2, "None" } };
        public static SortedDictionary<int, string> Microphones = new();

        public static BindingSource PrimaryDeviceBindingSource = new(PrimaryDevices, null);
        public static BindingSource SecondaryDeviceBindingSource = new(SecondaryDevices, null);
        public static BindingSource MicrophoneBindingSource = new(Microphones, null);

        private static readonly MMDeviceEnumerator enumerator = new();

        public static event EventHandler<EventArgs>? DevicesUpdated;
        static Devices()
        {
            enumerator.RegisterEndpointNotificationCallback(new NotificationClientImplementation());
        }
        public static void Initialize()
        {
            
            PrimaryDevices.Clear();
            SecondaryDevices.Clear();
            SecondaryDevices.Add(-2, "None");
            Microphones.Clear();

            //fix mapping
            var outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
            var defaultOutputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            int defaultOutputIndex = outputDevices.FindIndex(delegate (MMDevice dev)
            {
                return dev.ID == defaultOutputDevice.ID;
            });
            outputDevices.RemoveAt(defaultOutputIndex);
            outputDevices.Insert(0, defaultOutputDevice);

            for (int i = -1; i < WaveOut.DeviceCount; i++)
            {
                string name = "Primary Audio Driver";
                if (i >= 0) name = outputDevices[i].FriendlyName;
                PrimaryDevices.Add(i, name);
                SecondaryDevices.Add(i, name);
                Debug.WriteLine($"Added {name}");
            }

            var inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            var defaultInputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            int defaultInputIndex = inputDevices.FindIndex(delegate(MMDevice dev)
            {
                return dev.ID == defaultInputDevice.ID;
            });
            inputDevices.RemoveAt(defaultInputIndex);
            inputDevices.Insert(0, defaultInputDevice);

            for (int i = -1; i < WaveIn.DeviceCount; i++)
            {
                string name = "Primary Audio Driver";
                var caps = WaveIn.GetCapabilities(i);
                
                if (i >= 0) name = inputDevices[i].FriendlyName;
                Microphones.Add(i, name);
                Debug.WriteLine($"Added {name}");

            }


        PrimaryDeviceBindingSource = new(PrimaryDevices, null);
        SecondaryDeviceBindingSource = new(SecondaryDevices, null);
        MicrophoneBindingSource = new(Microphones, null);
            
        }

        public static void UpdateDevices()
        {
            DevicesUpdated?.Invoke(null, new());
        }


        #region Selection
        public static void MicrophoneSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem == null) return;
           Microphone = ((KeyValuePair<int, string>)(box.SelectedItem)).Key;
            //MicInjector.Refresh();
        }

        public static void PrimaryOutputSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if(box.SelectedItem == null) return;
            PrimaryOutput = ((KeyValuePair<int, string>)(box.SelectedItem)).Key;
            //MicInjector.Refresh();
        }

        public static void SecondaryOutputSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem == null) return;
            SecondaryOutput = ((KeyValuePair<int, string>)(box.SelectedItem)).Key;
            //MicInjector.Refresh();
        }
        #endregion


    }

    //https://stackoverflow.com/questions/6163119/handling-changed-audio-device-event-in-c-sharp
    class NotificationClientImplementation : NAudio.CoreAudioApi.Interfaces.IMMNotificationClient
    {
        string defaultDevice = "";

        public void OnDefaultDeviceChanged(DataFlow dataFlow, Role deviceRole, string defaultDeviceId)
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
            if (defaultDeviceId != defaultDevice)
                Devices.UpdateDevices();
            defaultDevice = defaultDeviceId;
        }

        public void OnDeviceAdded(string deviceId)
        {
            //Do some Work
            Debug.WriteLine("OnDeviceAdded -->");
        }

        public void OnDeviceRemoved(string deviceId)
        {

            Debug.WriteLine("OnDeviceRemoved -->");
            //Do some Work
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            Debug.WriteLine("OnDeviceStateChanged\n Device Id -->{0} : Device State {1}", deviceId, newState);
            //Do some Work
        }

        public NotificationClientImplementation()
        {
            //_realEnumerator.RegisterEndpointNotificationCallback();
            if (System.Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
            }
        }

        public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
        {
            //Do some Work
            //fmtid & pid are changed to formatId and propertyId in the latest version NAudio
            Debug.WriteLine("OnPropertyValueChanged: formatId --> {0}  propertyId --> {1}", propertyKey.formatId.ToString(), propertyKey.propertyId.ToString());
        }

    }
}

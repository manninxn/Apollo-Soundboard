using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Diagnostics;

namespace Apollo_Soundboard
{
    public class DeviceManager
    {
        int _primary = Settings.Default.PrimaryOutput;
        public int PrimaryOutput
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

        int _secondary = Settings.Default.SecondaryOutput;
        public int SecondaryOutput
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


        int _microphone = Settings.Default.Microphone;
        public int Microphone
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

        public OptimizedBindingList<KeyValuePair<int, string>> PrimaryDevices = new();
        public OptimizedBindingList<KeyValuePair<int, string>> SecondaryDevices = new();
        public OptimizedBindingList<KeyValuePair<int, string>> Microphones = new();

        private MMDeviceEnumerator enumerator = new();

        public event EventHandler<EventArgs>? DevicesUpdated;

        public DeviceManager()
        {
            enumerator.RegisterEndpointNotificationCallback(new NotificationClientImplementation());
        }
        public void Refresh()
        {
            var tempPrimary = new OptimizedBindingList<KeyValuePair<int, string>>();
            var tempSecondary = new OptimizedBindingList<KeyValuePair<int, string>>();
            var tempMicrophones = new OptimizedBindingList<KeyValuePair<int, string>>();

            PrimaryDevices.Clear();
            SecondaryDevices.Clear();
            tempSecondary.Add(new(-2, "None"));
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

            var inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            var defaultInputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            int defaultInputIndex = inputDevices.FindIndex(delegate (MMDevice dev)
            {
                return dev.ID == defaultInputDevice.ID;
            });
            inputDevices.RemoveAt(defaultInputIndex);
            inputDevices.Insert(0, defaultInputDevice);


            for (int i = -1; i < WaveOut.DeviceCount; i++)
            {
                string name = "Primary Audio Driver";

                if (i >= 0) name = outputDevices[i].FriendlyName;
                tempPrimary.Add(new(i, name));
                tempSecondary.Add(new(i, name));

                Debug.WriteLine($"Added {name}");
            }

            for (int i = -1; i < WaveIn.DeviceCount; i++)
            {
                string name = "Primary Audio Driver";
                var caps = WaveIn.GetCapabilities(i);

                if (i >= 0) name = inputDevices[i].FriendlyName;
                tempMicrophones.Add(new(i, name));

                Debug.WriteLine($"Added {name}");

            }

            PrimaryDevices.AddRange(tempPrimary);
            SecondaryDevices.AddRange(tempSecondary);
            Microphones.AddRange(tempMicrophones);

        }

        public void OnDevicesUpdated()
        {
            DevicesUpdated?.Invoke(this, new());
        }


        #region Selection
        public void MicrophoneSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem == null) return;
            Microphone = ((KeyValuePair<int, string>)(box.SelectedItem)).Key;
            Soundboard.MicInjector.Refresh();
        }

        public void PrimaryOutputSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem == null) return;
            PrimaryOutput = ((KeyValuePair<int, string>)(box.SelectedItem)).Key;
            Soundboard.MicInjector.Refresh();
        }

        public void SecondaryOutputSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem == null) return;
            SecondaryOutput = ((KeyValuePair<int, string>)(box.SelectedItem)).Key;
            Soundboard.MicInjector.Refresh();
        }
        #endregion


    }

    //https://stackoverflow.com/questions/6163119/handling-changed-audio-device-event-in-c-sharp
    class NotificationClientImplementation : NAudio.CoreAudioApi.Interfaces.IMMNotificationClient
    {
        string deviceId = string.Empty;
        public void OnDefaultDeviceChanged(DataFlow dataFlow, Role deviceRole, string defaultDeviceId)
        {
            if (deviceId == defaultDeviceId)
                return;
            deviceId = defaultDeviceId;

          //  Thread.Sleep(1000);
            Soundboard.Devices.OnDevicesUpdated();
        }

        public void OnDeviceAdded(string deviceId)
        {
        }

        public void OnDeviceRemoved(string deviceId)
        {

        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {

        }

        public NotificationClientImplementation()
        {

        }

        public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
        {

        }

    }

}

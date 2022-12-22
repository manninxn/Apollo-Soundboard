using Apollo_Soundboard.Properties;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;

namespace Apollo_Soundboard
{
    public class Device
    {
        public int DeviceNumber;
        public MMDevice? MMDevice;
        public string Name
        {
            get
            {
                return DeviceNumber switch
                {
                    -2 => "None",
                    -1 => "Primary Sound Driver",
                    _ => MMDevice?.FriendlyName ?? "Unknown"
                };
            }
        }

        public Device(int deviceNumber, MMDevice? device)
        {
            DeviceNumber = deviceNumber;
            MMDevice = device;
        }
    }
    public class DeviceManager : IMMNotificationClient
    {
        int _primary;
        public Device PrimaryDevice { get; private set; }
        public Device SecondaryDevice { get; private set; }
        public Device MicrophoneDevice { get; private set; }
        public int PrimaryOutput
        {
            get
            {
                return _primary;
            }
            set
            {
                PrimaryDevice = PrimaryDevices.First(x => x.DeviceNumber == value);
                _primary = value;
                Settings.Default.PrimaryOutput = value;
                Settings.Default.Save();

            }
        }

        int _secondary;
        public int SecondaryOutput
        {
            get
            {
                return _secondary;
            }
            set
            {
                SecondaryDevice = SecondaryDevices.First(x => x.DeviceNumber == value);
                _secondary = value;
                Settings.Default.SecondaryOutput = value;
                Settings.Default.Save();
            }
        }


        int _microphone;
        public int Microphone
        {
            get
            {
                return _microphone;
            }
            set
            {
                _microphone = value;
                MicrophoneDevice = Microphones.First(x => x.DeviceNumber == value);
                Settings.Default.Microphone = value;
                Settings.Default.Save();
            }
        }

        public OptimizedBindingList<Device> PrimaryDevices = new();
        public OptimizedBindingList<Device> SecondaryDevices = new();
        public OptimizedBindingList<Device> Microphones = new();

        private MMDeviceEnumerator enumerator = new();

        List<MMDevice> outputDevices = new();
        List<MMDevice> inputDevices = new();

        public event EventHandler<EventArgs>? DevicesUpdated;

        public DeviceManager()
        {
            outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
            inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            _ = enumerator.RegisterEndpointNotificationCallback(this);
        }
        public void Refresh()
        {
            var tempPrimary = new OptimizedBindingList<Device>();
            var tempSecondary = new OptimizedBindingList<Device>();
            var tempMicrophones = new OptimizedBindingList<Device>();

            PrimaryDevices.Clear();
            SecondaryDevices.Clear();
            tempSecondary.Add(new(-2, null));
            Microphones.Clear();

            //fix mapping
            //this is ridiculous

            //output
            var defaultOutputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            var newOutputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

            _ = outputDevices.RemoveAll(item => 0 > newOutputDevices.FindIndex(dev => dev.ID == item.ID));
            foreach (var dev in newOutputDevices)
            {
                int contains = outputDevices.FindIndex(item => dev.ID == item.ID);
                if (-1 < contains) continue;
                outputDevices.Add(dev);
            }

            int defaultOutputIndex = outputDevices.FindIndex(dev => dev.ID == defaultOutputDevice.ID);
            outputDevices.MoveItemAtIndexToFront(defaultOutputIndex);


            //input
            var defaultInputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            var newInputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            _ = inputDevices.RemoveAll(item => 0 > newInputDevices.FindIndex(dev => dev.ID == item.ID));
            foreach (var dev in newInputDevices)
            {
                if (-1 < inputDevices.FindIndex(old => old.ID == dev.ID)) continue;
                inputDevices.Add(dev);
            }

            int defaultInputIndex = inputDevices.FindIndex(dev => dev.ID == defaultInputDevice.ID);
            inputDevices.MoveItemAtIndexToFront(defaultInputIndex);



            for (int i = -1; i < WaveOut.DeviceCount; i++)
            {
                Device device = i switch
                {
                    -1 => new(i, defaultOutputDevice),
                    _ => new(i, outputDevices[i]),
                };
                tempPrimary.Add(device);
                tempSecondary.Add(device);

            }

            for (int i = -1; i < WaveIn.DeviceCount; i++)
            {
                Device device = i switch
                {
                    -1 => new(i, defaultInputDevice),
                    _ => new(i, inputDevices[i]),
                };
                tempMicrophones.Add(device);

            }

            PrimaryDevices.AddRange(tempPrimary);
            SecondaryDevices.AddRange(tempSecondary);
            Microphones.AddRange(tempMicrophones);

            PrimaryOutput = Settings.Default.PrimaryOutput;
            SecondaryOutput = Settings.Default.SecondaryOutput;
            Microphone = Settings.Default.Microphone;

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
            Microphone = ((Device)(box.SelectedItem)).DeviceNumber;
            Soundboard.MicInjector.Refresh();
        }

        public void PrimaryOutputSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem == null) return;
            PrimaryOutput = ((Device)(box.SelectedItem)).DeviceNumber;
            Soundboard.MicInjector.Refresh();
        }

        public void SecondaryOutputSelect(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem == null) return;
            SecondaryOutput = ((Device)(box.SelectedItem)).DeviceNumber;
            Soundboard.MicInjector.Refresh();
        }
        #endregion

        #region notification callback
        //https://stackoverflow.com/questions/6163119/handling-changed-audio-device-event-in-c-sharp
        string inputDeviceId = string.Empty;
        string outputDeviceId = string.Empty;
        string lastDeviceAddedId = string.Empty;
        string lastDeviceRemovedId = string.Empty;
        string lastDeviceChangedId = string.Empty;
        DeviceState lastDeviceState;
        public void OnDefaultDeviceChanged(DataFlow dataFlow, Role deviceRole, string defaultDeviceId)
        {
            if (deviceRole != Role.Console) return;

            if (dataFlow == DataFlow.Capture)
            {
                if (inputDeviceId == defaultDeviceId) return;
                inputDeviceId = defaultDeviceId;
            }
            else
            {
                if (outputDeviceId == defaultDeviceId) return;
                outputDeviceId = defaultDeviceId;
            }

            _ = Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
        }

        public void OnDeviceAdded(string deviceId)
        {
            if (lastDeviceAddedId != deviceId)
            {
                deviceId = lastDeviceAddedId;
                _ = Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
            }
        }

        public void OnDeviceRemoved(string deviceId)
        {
            if (lastDeviceRemovedId != deviceId)
            {
                lastDeviceRemovedId = deviceId;
                _ = Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
            }
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            if (lastDeviceChangedId != deviceId && lastDeviceState != newState)
            {
                lastDeviceChangedId = deviceId;
                lastDeviceState = newState;
                _ = Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
            }

        }

        public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
        {

        }
        #endregion
    }


}

using Apollo_Soundboard.Properties;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using System.Diagnostics;
using System.Xml.Linq;

namespace Apollo_Soundboard
{
    public class DeviceManager : IMMNotificationClient
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

        List<MMDevice> outputDevices = new();
        List<MMDevice> inputDevices = new();

        public event EventHandler<EventArgs>? DevicesUpdated;

        public DeviceManager()
        {
            outputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
            inputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            enumerator.RegisterEndpointNotificationCallback(this);
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
            //this is ridiculous

            //output
            var defaultOutputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            var newOutputDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

            outputDevices.RemoveAll(item => 0 > newOutputDevices.FindIndex(dev => dev.ID == item.ID));
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
            inputDevices.RemoveAll(item => 0 > newInputDevices.FindIndex(dev => dev.ID == item.ID));
            foreach (var dev in newInputDevices)
            {
                if (-1 < inputDevices.FindIndex(old => old.ID == dev.ID)) continue;
                inputDevices.Add(dev);
            }
            
            int defaultInputIndex = inputDevices.FindIndex(dev => dev.ID == defaultInputDevice.ID);
            inputDevices.MoveItemAtIndexToFront(defaultInputIndex);



            for (int i = -1; i < WaveOut.DeviceCount; i++)
            {
                string name = "Primary Audio Driver";

                if (i >= 0) name = outputDevices[i].FriendlyName;
                tempPrimary.Add(new(i, name));
                tempSecondary.Add(new(i, name));

            }

            for (int i = -1; i < WaveIn.DeviceCount; i++)
            {
                string name = "Primary Audio Driver";
                if (i >= 0) name = inputDevices[i].FriendlyName;
                tempMicrophones.Add(new(i, name));

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

            Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
        }

        public void OnDeviceAdded(string deviceId)
        {
            if (lastDeviceAddedId != deviceId)
            {
                deviceId= lastDeviceAddedId;
                Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
            }
        }

        public void OnDeviceRemoved(string deviceId)
        {
            if (lastDeviceRemovedId != deviceId)
            {
                lastDeviceRemovedId = deviceId;
                Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
            }
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            if(lastDeviceChangedId!= deviceId && lastDeviceState != newState)
            {
                lastDeviceChangedId= deviceId;
                lastDeviceState = newState;
                Soundboard.Instance.BeginInvoke(Soundboard.Devices.OnDevicesUpdated);
            }
            
        }

        public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
        {
           
        }
        #endregion
    }


}

using Apollo_Soundboard.Properties;

namespace Apollo_Soundboard
{
    public partial class VolumeMixer : Form
    {
        public Soundboard form;
        public VolumeMixer(Soundboard soundboard)
        {
            form = soundboard;
            InitializeComponent();
            trackBar1.Value = (int)Remap(Settings.Default.PrimaryGain - 1, -1, 1, -20, 20);
            trackBar2.Value = (int)Remap(Settings.Default.SecondaryGain - 1, -1, 1, -20, 20);
            trackBar3.Value = (int)Remap(Settings.Default.MicrophoneGain - 1, -1, 1, -20, 20);
        }
        float Remap(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            Settings.Default.PrimaryGain = 1 + Remap(trackBar1.Value, -20, 20, -1, 1);
            Settings.Default.Save();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

            Settings.Default.SecondaryGain = 1 + Remap(trackBar2.Value, -20, 20, -1, 1);
            Settings.Default.Save();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Settings.Default.MicrophoneGain = 1 + Remap(trackBar3.Value, -20, 20, -1, 1);
            Settings.Default.Save();
            form.RefreshInjector();
        }
    }
}

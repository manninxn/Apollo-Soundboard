using Apollo.Properties;

namespace Apollo.Forms
{
    public partial class VolumeMixer : Form
    {
        public Soundboard form;
        public VolumeMixer(Soundboard soundboard)
        {
            form = soundboard;
            InitializeComponent();
            trackBar1.Value = (int)Remap(Settings.Default.PrimaryGain, -1, 1, trackBar1.Minimum, trackBar1.Maximum);
            trackBar2.Value = (int)Remap(Settings.Default.SecondaryGain, -1, 1, trackBar2.Minimum, trackBar2.Maximum);
            trackBar3.Value = (int)Remap(Settings.Default.MicrophoneGain, -1, 1, trackBar3.Minimum, trackBar3.Maximum);
        }
        float Remap(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            Settings.Default.PrimaryGain = Remap(trackBar1.Value, trackBar1.Minimum, trackBar1.Maximum, -1, 1);
            Settings.Default.Save();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

            Settings.Default.SecondaryGain = Remap(trackBar2.Value, trackBar2.Minimum, trackBar2.Maximum, -1, 1);
            Settings.Default.Save();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Settings.Default.MicrophoneGain = Remap(trackBar3.Value, trackBar3.Minimum, trackBar3.Maximum, -1, 1);
            Settings.Default.Save();
            Soundboard.MicInjector.Refresh();
        }
    }
}

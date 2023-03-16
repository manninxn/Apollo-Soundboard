using Apollo.Properties;

namespace Apollo.Forms
{
    public partial class FFMPEGNotFound : Form
    {
        public FFMPEGNotFound()
        {
            InitializeComponent();
           // TopMost = Settings.Default.AlwaysOnTop;
        }

        private void Yes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void No_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}

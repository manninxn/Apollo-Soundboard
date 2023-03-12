using Apollo.Properties;

namespace Apollo.Forms
{
    public partial class UnsavedChanges : Form
    {
        public UnsavedChanges()
        {
            InitializeComponent();
            Owner = Soundboard.Instance;
            TopMost = Settings.Default.AlwaysOnTop;
        }

        private void YesSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void NoSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}

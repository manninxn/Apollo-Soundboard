namespace Apollo.Forms
{
    public partial class UnsavedChanges : Form
    {
        public UnsavedChanges()
        {
            InitializeComponent();
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

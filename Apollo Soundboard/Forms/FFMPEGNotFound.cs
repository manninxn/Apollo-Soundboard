namespace NUT_Soundboard.Forms
{
    public partial class FFMPEGNotFound : Form
    {
        public FFMPEGNotFound()
        {
            InitializeComponent();
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

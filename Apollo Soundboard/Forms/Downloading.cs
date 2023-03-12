using Apollo.Properties;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Apollo.Forms
{
    public partial class Downloading : Form
    {

        public Downloading()
        {
            InitializeComponent();
            TopMost = Settings.Default.AlwaysOnTop;
        }

        private async void Downloading_VisibleChanged(object sender, EventArgs e)
        {
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official,
                new Progress<ProgressInfo>(percent => progressBar1.Value = (int)(100 * (percent.DownloadedBytes / (float)percent.TotalBytes))
                ));
            Close();
        }

    }
}

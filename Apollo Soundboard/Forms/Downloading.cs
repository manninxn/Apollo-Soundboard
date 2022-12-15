using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Apollo_Soundboard.Forms
{
    public partial class Downloading : Form
    {

        public Downloading()
        {
            InitializeComponent();
        }

        private async void Downloading_VisibleChanged(object sender, EventArgs e)
        {
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official,
                new Progress<ProgressInfo>(percent => progressBar1.Value = (int)(100 * ((float)percent.DownloadedBytes / (float)percent.TotalBytes))
                ));
            Close();
        }

    }
}

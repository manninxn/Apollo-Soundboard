using Apollo.Properties;
using System.Diagnostics;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Exceptions;

namespace Apollo.Forms
{
    public partial class AudioConverter : Form
    {
        public AudioConverter()
        {
            InitializeComponent();
            Owner = MainForm.Instance;
            TopMost = Settings.Default.AlwaysOnTop;
        }

        private void BrowseConvertFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog AudioFileSelector = new();
            //openfiledialog filter is only audio files
            AudioFileSelector.Filter = "Any file (*.*)|*.*";
            DialogResult result = AudioFileSelector.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                InputFileBox.Text = AudioFileSelector.FileName;
            }
        }


        private async Task Convert(string input, string output)
        {
            try
            {

                var mediaInfo = await FFmpeg.GetMediaInfo(input);
                var streams = mediaInfo.AudioStreams;
                foreach (var stream in streams)
                {

                    stream.SetCodec(output.ToLower().TakeLast(3).ToString() switch
                    {
                        "mp3" => AudioCodec.mp3,
                        "ogg" => AudioCodec.libvorbis,
                        "wav" => AudioCodec.wavpack,
                        _ => AudioCodec.mp3
                    });
                }
                string codec = output.ToLower().TakeLast(3).ToString() switch { 
                "wav" =>"pcm_s16le",
                "mp3" => "libmp3lame",
                _ => "libvorbis"
                };
                Debug.Write(codec);
                _ = await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{input}\"")
                .AddParameter($"-y -c:a {codec}")
                .SetOutput(output)
                .SetOverwriteOutput(true)
                .Start();

            }
            catch (FFmpegNotFoundException ex)
            {
                Debug.WriteLine(ex.Message);
                FFMPEGNotFound error = new();
                if (error.ShowDialog() == DialogResult.Yes)
                {
                    Downloading wait = new();
                    wait.Owner = error;
                    wait.TopMost = Settings.Default.AlwaysOnTop;
                    wait.ShowDialog();
                    await Convert(input, output);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private void SaveFile_Click(object sender, EventArgs e)
        {
            FFmpeg.SetExecutablesPath(Directory.GetCurrentDirectory());
            SaveFileDialog ExportFile = new();
            ExportFile.Filter = "MP3 files (*.mp3)|*.mp3|WAV files (*.wav)|*.wav|OGG files (*.ogg)|*.ogg";
            DialogResult result = ExportFile.ShowDialog();
            if (result == DialogResult.OK)
            {

                Task.Run(() => Convert(InputFileBox.Text, ExportFile.FileName));

            }
        }

    }
}

﻿using System.Diagnostics;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Exceptions;

namespace Apollo_Soundboard.Forms
{
    public partial class AudioConverter : Form
    {
        public AudioConverter()
        {
            InitializeComponent();
        }

        private void BrowseConvertFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog AudioFileSelector = new OpenFileDialog();
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

                    stream.SetCodec(output.ToLower().EndsWith(".mp3") ? AudioCodec.mp3 : AudioCodec.wavpack);
                }
                string codec = output.EndsWith(".wav", StringComparison.OrdinalIgnoreCase) ? "pcm_s16le" : "libmp3lame";
                Debug.Write(codec);
                await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{input}\"")
                .AddParameter($"-y -c:a {codec}")
                .SetOutput(output)
                .SetOverwriteOutput(true)
                .Start();

            }
            catch (FFmpegNotFoundException ex)
            {
                Debug.WriteLine(ex.Message);
                FFMPEGNotFound error = new FFMPEGNotFound();
                if (error.ShowDialog() == DialogResult.Yes)
                {
                    Downloading wait = new Downloading();
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
            ExportFile.Filter = "MP3 files (*.mp3)|*.mp3|WAV files (*.wav)|*.wav";
            DialogResult result = ExportFile.ShowDialog();
            if (result == DialogResult.OK)
            {

                Task.Run(() => Convert(InputFileBox.Text, ExportFile.FileName));

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Text.Json;
using System.Diagnostics;
using Gma.System.MouseKeyHook.HotKeys;
using System.Reflection;

namespace Apollo.IO
{


    public static class Archiver
    {

        public struct SoundMetadata
        {
            public string SoundName { get; set; }
            public string FileName { get; set; }
            public string SoundId { get; set; }
            public int[] Hotkeys { get; set; }
            public int TimesPlayed { get; set; }

            public bool HotkeyOrderMatters { get; set; }
            public float Gain { get; set; }
            public bool OverlapSelf { get; set; }

        }

        private struct SoundPageMetadata
        {
            public List<SoundMetadata> Sounds { get; set; }
            public string Name { get; set; }
            public int PageNumber { get; set; }
        }
        private struct SoundboardMetadata
        {
            public List<SoundPageMetadata> Pages { get; set; }
            public string SoundboardVersion { get; set; }
            public string Name { get; set; }
        }



        public static void Export(Soundboard soundboard, string FileName)
        {
            SoundboardMetadata metadata = new();
            metadata.Pages = new List<SoundPageMetadata>();
            metadata.SoundboardVersion = Program.Version;
            metadata.Name = soundboard.Name;

            using (FileStream ExportedArchive = new FileStream(FileName, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(ExportedArchive, ZipArchiveMode.Update))
            {
                for (int i = 0; i < soundboard.NumPages; i++)
                {
                    SoundPage page = soundboard.Pages[i];
                    SoundPageMetadata soundPageMetadata = new SoundPageMetadata();
                    soundPageMetadata.Sounds = new();
                    soundPageMetadata.PageNumber = i;
                    soundPageMetadata.Name = page.Name;
                    
                    for (int j = 0; j < page.NumSounds; j++)
                    {
                        Sound sound = page.Sounds[j];
                        string soundId = String.Format("{0}.{1}", i, j);
                        ZipArchiveEntry SoundFileEntry = archive.CreateEntry(soundId);

                        using (BinaryWriter SoundWriter = new BinaryWriter(SoundFileEntry.Open()))
                        using (FileStream SoundReader = new FileStream(sound.FilePath, FileMode.Open))
                        {
                            SoundReader.CopyTo(SoundWriter.BaseStream);
                        }

                        soundPageMetadata.Sounds.Add(new()
                        {
                            SoundName = sound.SoundName,
                            FileName = System.IO.Path.GetFileName(sound.FilePath),
                            Hotkeys = sound.GetHotkeys().Select(i => (int)i).ToArray(),
                            HotkeyOrderMatters = sound.HotkeyOrderMatters,
                            Gain = sound.Gain,
                            SoundId = soundId,
                            OverlapSelf = sound.OverlapSelf
                        });
                    }
                    metadata.Pages.Add(soundPageMetadata);
                }

                string jsonString = JsonSerializer.Serialize(metadata, new JsonSerializerOptions() { WriteIndented = true });
                ZipArchiveEntry MetaEntry = archive.CreateEntry("meta");

                using (StreamWriter SoundWriter = new StreamWriter(MetaEntry.Open()))
                {
                    SoundWriter.Write(jsonString);
                }
            }
        }

        public static (string SaveFile, Soundboard soundboard)? LoadFromArchive(string FileName)
        {
            Soundboard soundboard = new(true);
            
            FolderBrowserDialog SaveDirectory = new FolderBrowserDialog();
            SaveDirectory.Description = "Select Extraction Directory";
            SaveDirectory.UseDescriptionForTitle = true;
            string Directory;

            if (SaveDirectory.ShowDialog() == DialogResult.Cancel)
                return null;
            else
            {
                Directory = SaveDirectory.SelectedPath;

            }

            string SaveFileName = Path.Combine(Directory, Path.GetFileNameWithoutExtension(FileName) + ".asb");

            using (FileStream ExportedArchive = new FileStream(FileName, FileMode.Open))
            using (ZipArchive archive = new ZipArchive(ExportedArchive, ZipArchiveMode.Read))
            {
                ZipArchiveEntry MetaEntry = archive.Entries.Where(entry => entry.Name == "meta").First();
                
                SoundboardMetadata? metadata = null;

                using (StreamReader SoundReader = new StreamReader(MetaEntry.Open()))
                {
                   string json = SoundReader.ReadToEnd();
                    Debug.WriteLine(json);
                   metadata = JsonSerializer.Deserialize<SoundboardMetadata>(json);



                }

                if(metadata != null)
                {
                    foreach (SoundPageMetadata pageMetadata in metadata.Value.Pages)
                    {
                        SoundPage page = new();
                        
                        foreach (SoundMetadata soundMetadata in pageMetadata.Sounds)
                        {

                            ZipArchiveEntry SoundArchiveEntry = archive.Entries.Where(entry => entry.Name == soundMetadata.SoundId).First();

                            Sound sound = new(soundMetadata);
                            
                            string filePath = Path.Combine(Directory, soundMetadata.SoundId + "_" + soundMetadata.FileName);

                            sound.SetOwner(soundboard);
                            using (BinaryReader SoundReader = new BinaryReader(SoundArchiveEntry.Open()))
                            using (FileStream SoundWriter = new FileStream(filePath, FileMode.OpenOrCreate))
                            {
                                SoundReader.BaseStream.CopyTo(SoundWriter);
                                SoundReader.Close();
                                SoundWriter.Close();
                            }
                            Debug.WriteLine("Path: " + filePath);
                            sound.FilePath = filePath;
                            page.AddSound(sound);
                        }
                        page.Name = pageMetadata.Name;
                        soundboard.Pages.Add(page);
                    }
                    soundboard.Name = metadata.Value.Name;
                }

                Serializer.SerializeToFile(soundboard, SaveFileName);

            }

            return (SaveFileName, soundboard);
        }

    }
}

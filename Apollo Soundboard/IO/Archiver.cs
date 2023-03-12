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

        private struct ArchiveMetadata
        {
            public string SoundName { get; set; }
            public string FileName { get; set; }
            public int SoundIndex { get; set; }
            public int[] Hotkeys { get; set; }

            public bool HotkeyOrderMatters { get; set; }
            public float Gain { get; set; }

        }



        public static void Export(SoundItem[] Sounds, string FileName)
        {
            List<ArchiveMetadata> SoundMetas = new();

            using (FileStream ExportedArchive = new FileStream(FileName, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(ExportedArchive, ZipArchiveMode.Update))
            {
                for (int i = 0; i < Sounds.Length; i++)
                {
                    SoundItem Sound = Sounds[i];
                    ZipArchiveEntry SoundFileEntry = archive.CreateEntry(i.ToString());

                    using (BinaryWriter SoundWriter = new BinaryWriter(SoundFileEntry.Open()))
                    using (FileStream SoundReader = new FileStream(Sound.FilePath, FileMode.Open))
                    {
                        SoundReader.CopyTo(SoundWriter.BaseStream);
                    }

                    SoundMetas.Add(new()
                    {
                        SoundName = Sound.SoundName,
                        FileName = System.IO.Path.GetFileName(Sound.FilePath),
                        Hotkeys = Sound.GetHotkeys().Select(i => (int)i).ToArray(),
                        HotkeyOrderMatters = Sound.HotkeyOrderMatters,
                        Gain = Sound.Gain,
                        SoundIndex = i
                    });
                }
                string jsonString = JsonSerializer.Serialize(SoundMetas, new JsonSerializerOptions() { WriteIndented = true });
                ZipArchiveEntry MetaEntry = archive.CreateEntry("meta");

                using (StreamWriter SoundWriter = new StreamWriter(MetaEntry.Open()))
                {
                    SoundWriter.Write(jsonString);
                }
            }
        }

        public static (string SaveFile, List<SoundItem> Sounds)? LoadFromArchive(string FileName)
        {
            List<SoundItem> Sounds = new List<SoundItem>();
            
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
                
                List<ArchiveMetadata>? metadata = null;

                using (StreamReader SoundReader = new StreamReader(MetaEntry.Open()))
                {
                   string json = SoundReader.ReadToEnd();
                    Debug.WriteLine(json);
                   metadata = JsonSerializer.Deserialize<List<ArchiveMetadata>>(json);



                }

                if(metadata != null)
                {
                    foreach (ArchiveMetadata MetadataEntry in metadata)
                    {
                        SoundItem Sound = new();
                        ZipArchiveEntry SoundArchiveEntry = archive.Entries.Where(entry => entry.Name == MetadataEntry.SoundIndex.ToString()).First();
                        
                        
                        Sound.SoundName = MetadataEntry.SoundName;
                        Sound.SetHotkeys(MetadataEntry.Hotkeys.Select(i => (Keys)i).ToList());
                        Sound.Gain = MetadataEntry.Gain;
                        Sound.HotkeyOrderMatters = MetadataEntry.HotkeyOrderMatters;
                        string filePath = Path.Combine(Directory, MetadataEntry.SoundIndex.ToString() + "_" + MetadataEntry.FileName); ;
                        using (BinaryReader SoundReader = new BinaryReader(SoundArchiveEntry.Open()))
                        using (FileStream SoundWriter = new FileStream(filePath, FileMode.OpenOrCreate))
                        {
                            SoundReader.BaseStream.CopyTo(SoundWriter);
                            SoundReader.Close();
                            SoundWriter.Close();
                        }
                        Debug.WriteLine("Path: " + filePath);
                        Sound.FilePath = filePath;
                        Sounds.Add(Sound);
                    }
                }

                Serializer.SerializeToFile(Sounds.ToList(), SaveFileName);

            }

            return (SaveFileName, Sounds);
        }

    }
}

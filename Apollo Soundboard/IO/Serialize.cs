using System.Text.Json;

namespace Apollo.IO
{
    public struct SoundboardData
    {
        public string Name { get; set; }
        public List<SoundboardPageData> Pages { get; set; }
        public string SoundboardVersion { get; set; }
    }
    public struct SoundboardPageData
    {
        public List<SoundData> Sounds { get; set; }
        public string Name { get; set; }
    }
    public struct SoundData
    {
        public int TimesPlayed { get; set; }
        public string FilePath { get; set; }
        public float Gain { get; set; }
        public int[] Hotkeys { get; set; }
        public bool HotkeyOrderMatters { get; set; }
        public string SoundName { get; set; }
        public bool OverlapSelf { get; set; }
    }
    public static class Serializer
    {
        public static string Serialize(Soundboard soundboard)
        {
            SoundboardData soundboardData = new();
            soundboardData.Name = soundboard.Name;
            soundboardData.SoundboardVersion = Program.Version;
            soundboardData.Pages = new List<SoundboardPageData>();

            foreach (SoundPage page in soundboard.Pages)
            {
                List<SoundData> entries = new();

                foreach (Sound sound in page.Sounds)
                {
                    SoundData data = new();
                    data.Gain = sound.Gain;
                    data.FilePath = sound.FilePath;
                    data.SoundName = sound.SoundName;
                    data.Hotkeys = sound.GetHotkeys().Select(i => (int)i).ToArray();
                    data.HotkeyOrderMatters = sound.HotkeyOrderMatters;
                    data.TimesPlayed = sound.TimesPlayed;
                    entries.Add(data);
                }
                soundboardData.Pages.Add(new() { Sounds = entries, Name = page.Name });
            }

            string jsonString = JsonSerializer.Serialize(soundboardData, new JsonSerializerOptions() { WriteIndented = true });

            return jsonString;
        }

        public static void SerializeToFile(Soundboard soundboard, string fileName)
        {
            string jsons = Serialize(soundboard);
            File.WriteAllText(fileName, jsons);
        }

        public static Soundboard Deserialize(string json)
        {

            SoundboardData? soundboardData = JsonSerializer.Deserialize<SoundboardData>(json);

            if(soundboardData == null)
            {
                return new();
            }
            OptimizedBindingList<SoundPage> soundPages= new OptimizedBindingList<SoundPage>();
            
            

            foreach (SoundboardPageData item in soundboardData.Value.Pages)
            {
                SoundPage page = new SoundPage();

                foreach(SoundData soundData in item.Sounds)
                {
                    Sound sound = new(soundData);
                    page.Sounds.Add(sound);
                    
                }
                page.Name = item.Name;

                soundPages.Add(page);

            }

            return new(soundboardData.Value.Name, soundPages);


        }

        public static Soundboard DeserializeFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return Deserialize(json);
            }
            catch { return new(); }
        }


    }
}

using System.Text.Json;

namespace Apollo.Importers
{
    public struct SoundData
    {
        public string FilePath { get; set; }
        public float Gain { get; set; }
        public int[] Hotkeys { get; set; }
        public bool HotkeyOrderMatters { get; set; }
        public string SoundName { get; set; }
    }
    public static class Serializer
    {
        public static string Serialize(List<SoundItem> sounds)
        {
            List<SoundData> entries = new();

            foreach (SoundItem sound in sounds)
            {
                SoundData data = new();
                data.Gain = sound.Gain;
                data.FilePath = sound.FilePath;
                data.SoundName = sound.SoundName;
                data.Hotkeys = sound.GetHotkeys().Select(i => (int)i).ToArray();
                data.HotkeyOrderMatters = sound.HotkeyOrderMatters;
                entries.Add(data);
            }

            string jsonString = JsonSerializer.Serialize(entries, new JsonSerializerOptions() { WriteIndented = true });

            return jsonString;
        }

        public static void SerializeToFile(List<SoundItem> sounds, string fileName)
        {
            string jsons = Serialize(sounds);
            File.WriteAllText(fileName, jsons);
        }

        public static List<SoundItem> Deserialize(string json)
        {

            List<SoundData>? entries = JsonSerializer.Deserialize<List<SoundData>>(json);

            var sounds = new List<SoundItem>();
            if (entries == null) return sounds;

            foreach (SoundData item in entries)
            {
                SoundItem sound = new();
                var keys = new List<Keys>();

                sound.SetHotkeys(Array.ConvertAll(item.Hotkeys, (i) => { return (Keys)i; }).ToList());
                sound.FilePath = item.FilePath;
                sound.SoundName = item.SoundName;
                sound.Gain = item.Gain;
                sound.HotkeyOrderMatters = item.HotkeyOrderMatters;
                sounds.Add(sound);
            }

            return sounds;


        }

        public static List<SoundItem> DeserializeFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return Deserialize(json);
            }
            catch { return new List<SoundItem>(); }
        }


    }
}

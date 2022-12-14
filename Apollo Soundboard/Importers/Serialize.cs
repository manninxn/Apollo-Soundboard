using System.Text.Json;
using Apollo_Soundboard;

namespace NUT_Soundboard.Importers
{

    public static class Serializer
    {
        public static string Serialize(List<SoundItem> sounds)
        {
            Dictionary<string, int[]> entries = new();

            foreach (SoundItem sound in sounds)
            {
                entries.Add(sound.FilePath, sound.GetHotkeys().Select(i => (int)i).ToArray());
            }

            string jsonString = JsonSerializer.Serialize(entries);

            return jsonString;
        }

        public static void SerializeToFile(List<SoundItem> sounds, string fileName)
        {
            string jsons = Serialize(sounds);
            File.WriteAllText(fileName, jsons);
        }

        public static List<SoundItem> Deserialize(string json)
        {
            Dictionary<string, int[]> entries = JsonSerializer.Deserialize<Dictionary<string, int[]>>(json);

            var sounds = new List<SoundItem>();
            if (entries == null) return sounds;

            foreach (KeyValuePair<string, int[]> item in entries)
            {
                SoundItem sound = new SoundItem();
                var keys = new List<Keys>();

                sound.SetHotkeys(Array.ConvertAll(item.Value, (i) => { return (Keys)i; }).ToList());
                sound.FilePath = item.Key;

                sounds.Add(sound);
            }

            return sounds;

        }

        public static List<SoundItem> DeserializeFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return Deserialize(json);
        }

    }
}

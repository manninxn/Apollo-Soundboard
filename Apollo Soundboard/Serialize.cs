using System.Text.Json;

namespace Apollo_Soundboard
{
    public class ItemEntry
    {
        public string file { get; set; }
        public int[] activationKeysNumbers { get; set; }
    }
    public class ItemList
    {
        public ItemEntry[] soundboardEntries { get; set; }
    }
    public static class Serializer
    {
        public static string Serialize(List<SoundItem> sounds)
        {
            ItemList itemList = new ItemList();
            List<ItemEntry> entries = new List<ItemEntry>();
            foreach (SoundItem sound in sounds)
            {
                ItemEntry entry = new ItemEntry();
                entry.file = sound.FilePath;
                entry.activationKeysNumbers = sound.GetHotkeys().Select(i => (int)i).ToArray();
                entries.Add(entry);
            }
            itemList.soundboardEntries = entries.ToArray();
            string jsonString = JsonSerializer.Serialize(itemList);
            return jsonString;
        }

        public static void SerializeToFile(List<SoundItem> sounds, string fileName)
        {
            string jsons = Serialize(sounds);
            File.WriteAllText(fileName, jsons);
        }

        public static List<SoundItem> Deserialize(string json)
        {
            ItemList? itemList = JsonSerializer.Deserialize<ItemList>(json);
            var sounds = new List<SoundItem>();
            if (itemList == null) return sounds;

            foreach (ItemEntry item in itemList.soundboardEntries)
            {
                SoundItem sound = new SoundItem();
                var keys = new List<Keys>();
                foreach (int i in item.activationKeysNumbers)
                {
                    keys.Add((Keys)i);
                }
                sound.FilePath = item.file;
                sound.SetHotkeys(keys);
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

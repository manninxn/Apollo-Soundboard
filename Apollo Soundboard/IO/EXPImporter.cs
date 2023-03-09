using System.Text.Json;

namespace Apollo.IO
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
    internal class EXPImporter
    {

        public static List<SoundItem> Import(string filePath)
        {
            string json = File.ReadAllText(filePath);
            ItemList? itemList = JsonSerializer.Deserialize<ItemList>(json);
            var sounds = new List<SoundItem>();

            if (itemList == null) return sounds;

            foreach (ItemEntry item in itemList.soundboardEntries)
            {
                SoundItem sound = new();
                var keys = new List<Keys>();
                foreach (int i in item.activationKeysNumbers)
                {
                    keys.Add(KeyMap.JavaKeyCodeTranslate(i));
                }
                sound.FilePath = item.file;
                sound.SetHotkeys(keys);
                sounds.Add(sound);
            }
            return sounds;
        }



    }
}

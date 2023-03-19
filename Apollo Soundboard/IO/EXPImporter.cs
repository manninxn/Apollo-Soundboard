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

        public static Soundboard Import(string filePath)
        {
            string json = File.ReadAllText(filePath);
            ItemList? itemList = JsonSerializer.Deserialize<ItemList>(json);
            var sounds = new OptimizedBindingList<Sound>();

            if (itemList == null) return new();

            foreach (ItemEntry item in itemList.soundboardEntries)
            {
                Sound sound = new();
                var keys = new List<Keys>();
                foreach (int i in item.activationKeysNumbers)
                {
                    keys.Add(KeyMap.JavaKeyCodeTranslate(i));
                }
                sound.FilePath = item.file;
                sound.SetHotkeys(keys);
                sounds.Add(sound);
            }

            return new(Path.GetFileNameWithoutExtension(filePath), sounds);
        }



    }
}

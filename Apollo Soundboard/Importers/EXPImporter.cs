using System.Text.Json;

namespace Apollo.Importers
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
                    keys.Add(JavaKeyCodeTranslate(i));
                }
                sound.FilePath = item.file;
                sound.SetHotkeys(keys);
                sounds.Add(sound);
            }
            return sounds;
        }

        //please make issues on github if theres more to add im too tired to do this all, theres so many
        public static Keys JavaKeyCodeTranslate(int keyCode)
        {
            return (Keys)(keyCode switch
            {
                10 => 13,
                44 => 188,
                45 => 189,
                46 => 190,
                57 => 191,
                59 => 186,
                61 => 187,
                91 => 219,
                92 => 226,
                93 => 221,
                127 => 46,
                129 => 191,
                _ => keyCode

            });
        }

    }
}

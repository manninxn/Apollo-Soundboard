using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace Apollo.Importers
{
    [XmlRoot(ElementName = "Sound")]
    public class Sound
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
        [XmlAttribute(AttributeName = "keyModifiers")]
        public string KeyModifiers { get; set; }
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

    }

    [XmlRoot(ElementName = "Soundlist")]
    public class Soundlist
    {
        [XmlElement(ElementName = "Sound")]
        public List<Sound> Sound { get; set; }


    }
    public static class SoundpadImporter
    {
        public static List<SoundItem> Import(string path)
        {

            var sounds = new List<SoundItem>();
            _ = XmlReader.Create(path);
            var serializer = new XmlSerializer(typeof(Soundlist));
            using (FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var obj = (Soundlist?)serializer.Deserialize(fs);
                if (obj is null) return sounds;
                Debug.WriteLine(obj.Sound[6].Key);
                foreach (Sound sound in obj.Sound)
                {
                    var soundItem = new SoundItem();
                    var keys = new List<Keys>();
                    soundItem.FilePath = sound.Url;

                    if (int.TryParse(sound.Key, out int keyCode))
                    {
                        keys.Add((Keys)keyCode);
                    }

                    if (int.TryParse(sound.KeyModifiers, out int modifiers))
                    {
                        switch (modifiers)
                        {
                            case 1:
                                keys.Add(Keys.Alt); break;
                            case 2:
                                keys.Add(Keys.ControlKey); break;
                            case 3:
                                keys.Add(Keys.Alt);
                                keys.Add(Keys.ControlKey);
                                break;
                            case 4:
                                keys.Add(Keys.Shift); break;
                            case 5:
                                keys.Add(Keys.Alt);
                                keys.Add(Keys.ShiftKey);
                                break;
                            case 6:
                                keys.Add(Keys.ShiftKey);
                                keys.Add(Keys.ControlKey);
                                break;
                            default:
                                break;
                        }
                    }

                    soundItem.SetHotkeys(keys);
                    sounds.Add(soundItem);

                }
            }

            return sounds;
        }
    }
}

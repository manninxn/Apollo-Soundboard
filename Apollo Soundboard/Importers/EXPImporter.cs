﻿using Apollo_Soundboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Apollo_Soundboard.Importers
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

        }
    }
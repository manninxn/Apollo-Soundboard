using Apollo.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Apollo.InputHandler;

namespace Apollo
{
    public class SoundPage
    {

        public OptimizedBindingList<Sound> Sounds;

        public string Name { get; set; } = "Page";

        public int NumSounds
        {
            get => Sounds.Count;
        }

        public SoundPage(OptimizedBindingList<Sound> Sounds)
        {
            this.Sounds = Sounds;
        }

        public SoundPage(string name, OptimizedBindingList<Sound> Sounds)
        {
            this.Name = name;
            this.Sounds = Sounds;
        }

        public SoundPage(string name)
        {
            Name = name;
            Sounds = new OptimizedBindingList<Sound>();
        }

        public SoundPage()
        {
            Sounds = new OptimizedBindingList<Sound>();
        }

        public void AddSound(Sound sound)
        {
            Sounds.Add(sound);
        }


    }

    public class Soundboard : IDisposable
    {
        public OptimizedBindingList<SoundPage> Pages = new OptimizedBindingList<SoundPage>();
        public string Name = "New Soundboard";

        public bool Active = true;

        public int NumPages { get => Pages.Count; }
        public int NumSounds {
            get {
                int i = 0;
                foreach(SoundPage page in Pages)
                {
                    i += page.NumSounds;
                }
                return i;
            }
        }

        public List<Sound> AllSounds
        {
            get
            {
                return Pages.SelectMany(p => p.Sounds).ToList();
            }
        }


        public event EventHandler<EventArgs> PageChanged;

        private int _pageNum = 0;
        public int ActivePageNumber
        {
            get { return _pageNum; }
            set
            {
                _pageNum = value;
                EventHandler<EventArgs> raiseEvent = PageChanged;
                if (raiseEvent != null)
                {
                    raiseEvent(this, new());
                }
            }
         }


        public SoundPage? ActivePage { get => Pages.ElementAtOrDefault(ActivePageNumber); }

        public Soundboard(bool doNotAddPage = false) {
            Pages = new OptimizedBindingList<SoundPage>();
            if(!doNotAddPage)
            {
                Pages.Add(new());
            }
        }

        public Soundboard(string name) { 
            Name = name;
        }

        public Soundboard(string name, OptimizedBindingList<SoundPage> pages) { 
            Name = name;
            Pages = pages;
            foreach (Sound sound in AllSounds)
            {
                sound.SetOwner(this);
            }
        }

        public Soundboard(string name, OptimizedBindingList<Sound> sounds)
        {
            Name = name;
            Pages.Add(new(sounds));
            foreach (Sound sound in AllSounds)
            {
                sound.SetOwner(this);
            }
        }

        public static Soundboard FromExp(string ExpFilePath)
        {
            return EXPImporter.Import(ExpFilePath);
        }
        public static Soundboard FromSoundpad(string SoundpadFilePath)
        {
            return SoundpadImporter.Import(SoundpadFilePath);
        }

        public static (string SaveFile, Soundboard soundboard)? FromArchive(string ArchiveFilePath)
        {
            return Archiver.LoadFromArchive(ArchiveFilePath);
        }

        ~Soundboard()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach(Sound sound in AllSounds)
            {
                sound.Dispose();
            }
        }

        public void NextPage()
        {
            ActivePageNumber = (ActivePageNumber + 1) % NumPages;

        }

        public void NewPage()
        {
            Pages.Add(new());
            ActivePageNumber = NumPages - 1;
        }


    }
}

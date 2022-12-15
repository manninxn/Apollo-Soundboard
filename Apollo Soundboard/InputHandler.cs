using Gma.System.MouseKeyHook;
using Newtonsoft.Json.Linq;
using NUT_Soundboard.Importers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Apollo_Soundboard
{
    public static class InputHandler
    {


        public static IKeyboardMouseEvents _GlobalHook = Hook.GlobalEvents();
        public static void Subscribe()
        {
            _GlobalHook.KeyDown += KeyboardListener;
            _GlobalHook.KeyUp += KeyUpListener;
        }

        static List<Keys> PressedKeys = new();
        private static void KeyboardListener(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode switch
            {
                Keys.LShiftKey => Keys.ShiftKey,
                Keys.LControlKey => Keys.ControlKey,
                Keys.RShiftKey => Keys.ShiftKey,
                Keys.RControlKey=> Keys.ControlKey,
                Keys.LMenu => Keys.Alt,
                Keys.RMenu => Keys.Alt,
                _ => e.KeyCode
            };

            if (!PressedKeys.Contains(keyCode))
            {
                PressedKeys.Add(keyCode);
            }

            foreach (SoundItem sound in SoundItem.AllSounds)
            {
                var lastN = PressedKeys.TakeLast(sound.GetHotkeys().Count());
                var b = sound.GetHotkeys();
                if (sound.HotkeyOrderMatters ? lastN.SequenceEqual(sound.GetHotkeys()) : Enumerable.SequenceEqual(lastN.OrderBy(e => e), sound.GetHotkeys().OrderBy(e => e)))
                {
                    sound.Play();
                }
            }
            if (PressedKeys.TakeLast(SoundItem.ClearSounds.Count).SequenceEqual(SoundItem.ClearSounds))
            {
                SoundItem.StopAllSounds();
            }
        }
        private static void KeyUpListener(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode switch
            {
                Keys.LShiftKey => Keys.ShiftKey,
                Keys.LControlKey => Keys.ControlKey,
                Keys.RShiftKey => Keys.ShiftKey,
                Keys.RControlKey => Keys.ControlKey,
                Keys.LMenu => Keys.Alt,
                Keys.RMenu => Keys.Alt,
                _ => e.KeyCode
            };
            PressedKeys.Remove(keyCode);

        }

    }
}

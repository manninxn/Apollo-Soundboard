using Gma.System.MouseKeyHook;
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

        static Dictionary<Keys, bool> PressedKeys = new Dictionary<Keys, bool>();

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
            
            PressedKeys[keyCode] = true;
            foreach (SoundItem sound in SoundItem.AllSounds)
            {
                if (sound.GetHotkeys().Count > 0 && sound.GetHotkeys().All(x =>
                {
                    bool result;
                    bool found = PressedKeys.TryGetValue(x, out result);
                    return result && found;
                }))
                {
                    sound.Play();
                }
            }
            if (SoundItem.ClearSounds.Count > 0 && SoundItem.ClearSounds.All(x =>
            {
                bool result;
                bool found = PressedKeys.TryGetValue(x, out result);
                return result && found;
            }))
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
            PressedKeys[keyCode] = false;

        }

    }
}

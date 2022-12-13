using Gma.System.MouseKeyHook;

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
            PressedKeys[e.KeyCode] = true;
            foreach (SoundItem sound in SoundItem.AllSounds)
            {
                if (SoundItem.ClearSounds.Count > 0 && sound.GetHotkeys().All(x =>
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
            PressedKeys[e.KeyCode] = false;

        }

    }
}

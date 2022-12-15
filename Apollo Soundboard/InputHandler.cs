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

        static List<Keys> PressedKeys = new();
        private static void KeyboardListener(object sender, KeyEventArgs e)
        {
            Keys keyCode = KeyMap.ParseModifierKey(e.KeyCode);

            if (!PressedKeys.Contains(keyCode))
            {
                PressedKeys.Add(keyCode);
            }

            for (int i = 0; i < SoundItem.AllSounds.Count; i++)
            {
                SoundItem sound = SoundItem.AllSounds[i];
                if (sound.GetHotkeys().Count == 0) continue;
                var lastN = PressedKeys.TakeLast(sound.GetHotkeys().Count());

                if (sound.HotkeyOrderMatters ? lastN.SequenceEqual(sound.GetHotkeys()) : Enumerable.SequenceEqual(lastN.OrderBy(e => e), sound.GetHotkeys().OrderBy(e => e)))
                {
                    sound.Play();
                }
            }

            if (SoundItem.ClearSounds.Count == 0) return;

            if (PressedKeys.TakeLast(SoundItem.ClearSounds.Count).SequenceEqual(SoundItem.ClearSounds))
            {
                SoundItem.StopAllSounds();
            }
        }
        private static void KeyUpListener(object sender, KeyEventArgs e)
        {
            PressedKeys.Remove(KeyMap.ParseModifierKey(e.KeyCode));

        }

    }

    public static class KeyMap
    {
        public static Keys ParseModifierKey(Keys key)
        {
            return key switch
            {
                Keys.LShiftKey => Keys.ShiftKey,
                Keys.LControlKey => Keys.ControlKey,
                Keys.RShiftKey => Keys.ShiftKey,
                Keys.RControlKey => Keys.ControlKey,
                Keys.LMenu => Keys.Alt,
                Keys.RMenu => Keys.Alt,
                _ => key
            };
        }
        public static string KeyToChar(Keys key)
        {
            return key switch
            {
                Keys.Enter => "Enter",
                Keys.A => "A",
                Keys.B => "B",
                Keys.C => "C",
                Keys.D => "D",
                Keys.E => "E",
                Keys.F => "F",
                Keys.G => "G",
                Keys.H => "H",
                Keys.I => "I",
                Keys.J => "J",
                Keys.K => "K",
                Keys.L => "L",
                Keys.M => "M",
                Keys.N => "N",
                Keys.O => "O",
                Keys.P => "P",
                Keys.Q => "Q",
                Keys.R => "R",
                Keys.S => "S",
                Keys.T => "T",
                Keys.U => "U",
                Keys.V => "V",
                Keys.W => "W",
                Keys.X => "X",
                Keys.Y => "Y",
                Keys.Z => "Z",
                Keys.D0 => "0",
                Keys.D1 => "1",
                Keys.D2 => "2",
                Keys.D3 => "3",
                Keys.D4 => "4",
                Keys.D5 => "5",
                Keys.D6 => "6",
                Keys.D7 => "7",
                Keys.D8 => "8",
                Keys.D9 => "9",
                Keys.Oemplus => "Plus",
                Keys.OemMinus => "Minus",
                Keys.OemQuestion => "Forward Slash",
                Keys.Oemcomma => "Comma",
                Keys.OemPeriod => "Period",
                Keys.OemOpenBrackets => "Open Bracket",
                Keys.OemQuotes => "Quote",
                Keys.Oem1 => "Semicolon",
                Keys.Oem3 => "Tilde",
                Keys.Oem5 => "Back Slash",
                Keys.Oem6 => "Close Bracket",
                Keys.Tab => "Tab",
                Keys.Space => "Spacebar",
                Keys.PageUp => "Page Up",
                Keys.PageDown => "Page Down",

                // Number Pad
                Keys.NumPad0 => "Numpad 0",
                Keys.NumPad1 => "Numpad 1",
                Keys.NumPad2 => "Numpad 2",
                Keys.NumPad3 => "Numpad 3",
                Keys.NumPad4 => "Numpad 4",
                Keys.NumPad5 => "Numpad 5",
                Keys.NumPad6 => "Numpad 6",
                Keys.NumPad7 => "Numpad 7",
                Keys.NumPad8 => "Numpad 8",
                Keys.NumPad9 => "Numpad 9",
                Keys.Subtract => "Numpad -",
                Keys.Add => "Numpad +",
                Keys.Decimal => "Numpad .",
                Keys.Divide => "Numpad /",
                Keys.Multiply => "Numpad *",
                Keys.LShiftKey => "Shift",
                Keys.LControlKey => "Control",
                Keys.RShiftKey => "Shift",
                Keys.RControlKey => "Control",
                Keys.ShiftKey => "Shift",
                Keys.ControlKey => "Control",
                Keys.Alt => "Alt",
                Keys.LMenu => "Alt",
                Keys.RMenu => "Alt",
                _ => key.ToString()

            };
        }
    }
}

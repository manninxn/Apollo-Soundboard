﻿using Apollo.Forms;
using Gma.System.MouseKeyHook;
using System.Runtime.CompilerServices;

namespace Apollo
{
    public class PressedKeysEventArgs : EventArgs
    {
        public List<Keys> PressedKeys { get; set; }
        public bool KeyDown { get; set; }
        public PressedKeysEventArgs(List<Keys> PressedKeys, bool keyDown = false)
        {
            this.PressedKeys = PressedKeys;
            KeyDown = keyDown;
        }
    }
    public static class InputHandler
    {


        public static IKeyboardMouseEvents _GlobalHook = Hook.GlobalEvents();

        public delegate void PressedKeysEventHandler(object? sender, PressedKeysEventArgs args);
        public static event PressedKeysEventHandler PressedKeysChanged;
        public static void Subscribe()
        {
            _GlobalHook.KeyDown += KeyboardListener;
            _GlobalHook.KeyUp += KeyUpListener;
        }

        static List<Keys> PressedKeys = new();
        private static void KeyboardListener(object? sender, KeyEventArgs e)
        {
            Keys keyCode = KeyMap.ParseModifierKey(e.KeyCode);

            if (!PressedKeys.Contains(keyCode))
            {
                PressedKeys.Add(keyCode);
                PressedKeysEventHandler raiseEvent = PressedKeysChanged;
                if(raiseEvent != null )
                {
                    raiseEvent(null, new(PressedKeys, true));
                }
            }
        }
        private static void KeyUpListener(object? sender, KeyEventArgs e)
        {
            PressedKeys.Remove(KeyMap.ParseModifierKey(e.KeyCode));
            PressedKeysEventHandler raiseEvent = PressedKeysChanged;
            if (raiseEvent != null)
            {
                raiseEvent(null, new(PressedKeys, false));
            }

        }

        public static bool CheckHotkeys(List<Keys> hotkeysToCheck, bool orderMatters = true)
        {
            var lastN = PressedKeys.TakeLast(hotkeysToCheck.Count());

            return orderMatters ? lastN.SequenceEqual(hotkeysToCheck) : Enumerable.SequenceEqual(lastN.OrderBy(e => e), hotkeysToCheck.OrderBy(e => e));

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
                155 => 45,
                _ => keyCode

            });
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
                Keys.Oemplus => "Equals",
                Keys.OemMinus => "Minus",
                Keys.OemQuestion => "Forward Slash",
                Keys.Oemcomma => "Comma",
                Keys.OemPeriod => "Period",
                Keys.OemOpenBrackets => "Open Bracket",
                Keys.OemBackslash => "Back Slash",
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

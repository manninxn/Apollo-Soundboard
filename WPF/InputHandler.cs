using Apollo.Forms;
using Gma.System.MouseKeyHook;
using System.Runtime.InteropServices;
using System;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;

namespace Apollo
{
    public static class InputHandler
    {

        public static KeyboardListener listener = new KeyboardListener();

        public static IKeyboardMouseEvents _GlobalHook = Hook.GlobalEvents();
        public static void Subscribe()
        {
            listener.KeyDown += KeyboardListener;
        }

        static List<Key> PressedKeys = new();
        private static void KeyboardListener(object sender, RawKeyEventArgs e)
        {
            Key keyCode = KeyMap.ParseModifierKey(e.Key);

            if (!PressedKeys.Contains(keyCode))
            {
                PressedKeys.Add(keyCode);
            }

            for (int i = 0; i < SoundItem.AllSounds.Count; i++)
            {
                SoundItem sound = SoundItem.AllSounds[i];
                if (sound.GetHotkeys().Count() == 0) continue;
                var lastN = PressedKeys.TakeLast(sound.GetHotkeys().Count());

                if (sound.HotkeyOrderMatters ? lastN.SequenceEqual(sound.GetHotkeys()) : lastN.OrderBy(e => e).ToList().SequenceEqual<Key>(sound.GetHotkeys().OrderBy(e => e).ToList()))
                {
                    sound.Play();
                }
            }

            if (SoundItem.ClearSounds.Count() > 0 && PressedKeys.TakeLast(SoundItem.ClearSounds.Count()).SequenceEqual(SoundItem.ClearSounds))
            {
                SoundItem.StopAllSounds();
            }

            if(MicInjector.ToggleInjector.Count > 0 && PressedKeys.TakeLast(MicInjector.ToggleInjector.Count).SequenceEqual(MicInjector.ToggleInjector))
            {
                Soundboard.Instance.ToggleMicInjector();
            }
        }
        private static void KeyUpListener(object sender, KeyEventArgs e)
        {
            _ = PressedKeys.Remove(KeyMap.ParseModifierKey(e.Key));

        }

    }




    public static class KeyMap
    {
        public static Key ParseModifierKey(Key key)
        {
            //force all to be left bc there is no general shift, control, or alt key
            return key switch
            {
                Key.RightShift => Key.LeftShift,
                Key.RightCtrl => Key.LeftCtrl,
                Key.RightAlt => Key.LeftAlt,
                _ => key
            };
        }

        //please make issues on github if theres more to add im too tired to do this all, theres so many
        public static Key JavaKeyCodeTranslate(int keyCode)
        {
            return (Key)(keyCode switch
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

        public static string KeyToChar(Key key)
        {
            return key switch
            {
                Key.Enter => "Enter",
                Key.A => "A",
                Key.B => "B",
                Key.C => "C",
                Key.D => "D",
                Key.E => "E",
                Key.F => "F",
                Key.G => "G",
                Key.H => "H",
                Key.I => "I",
                Key.J => "J",
                Key.K => "K",
                Key.L => "L",
                Key.M => "M",
                Key.N => "N",
                Key.O => "O",
                Key.P => "P",
                Key.Q => "Q",
                Key.R => "R",
                Key.S => "S",
                Key.T => "T",
                Key.U => "U",
                Key.V => "V",
                Key.W => "W",
                Key.X => "X",
                Key.Y => "Y",
                Key.Z => "Z",
                Key.D0 => "0",
                Key.D1 => "1",
                Key.D2 => "2",
                Key.D3 => "3",
                Key.D4 => "4",
                Key.D5 => "5",
                Key.D6 => "6",
                Key.D7 => "7",
                Key.D8 => "8",
                Key.D9 => "9",
                Key.OemPlus => "Equals",
                Key.OemMinus => "Minus",
                Key.OemQuestion => "Forward Slash",
                Key.OemComma => "Comma",
                Key.OemPeriod => "Period",
                Key.OemOpenBrackets => "Open Bracket",
                Key.OemBackslash => "Back Slash",
                Key.OemQuotes => "Quote",
                Key.Oem1 => "Semicolon",
                Key.Oem3 => "Tilde",
                Key.Oem5 => "Back Slash",
                Key.Oem6 => "Close Bracket",
                Key.Tab => "Tab",
                Key.Space => "Spacebar",
                Key.PageUp => "Page Up",
                Key.PageDown => "Page Down",

                // Number Pad
                Key.NumPad0 => "Numpad 0",
                Key.NumPad1 => "Numpad 1",
                Key.NumPad2 => "Numpad 2",
                Key.NumPad3 => "Numpad 3",
                Key.NumPad4 => "Numpad 4",
                Key.NumPad5 => "Numpad 5",
                Key.NumPad6 => "Numpad 6",
                Key.NumPad7 => "Numpad 7",
                Key.NumPad8 => "Numpad 8",
                Key.NumPad9 => "Numpad 9",
                Key.Subtract => "Numpad -",
                Key.Add => "Numpad +",
                Key.Decimal => "Numpad .",
                Key.Divide => "Numpad /",
                Key.Multiply => "Numpad *",
                Key.LeftShift => "Shift",
                Key.LeftCtrl => "Control",
                Key.RightShift => "Shift",
                Key.RightCtrl => "Control",
                Key.LeftAlt => "Alt",
                Key.RightAlt => "Alt",
                _ => key.ToString()

            };
        }
    }

    public class KeyboardListener : IDisposable
    {
        private static IntPtr hookId = IntPtr.Zero;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                return HookCallbackInner(nCode, wParam, lParam);
            }
            catch
            {
                Console.WriteLine("There was some error somewhere...");
            }
            return InterceptKeys.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private IntPtr HookCallbackInner(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)InterceptKeys.WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (KeyDown != null)
                        KeyDown(this, new RawKeyEventArgs(vkCode, false));
                }
                else if (wParam == (IntPtr)InterceptKeys.WM_KEYUP)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (KeyUp != null)
                        KeyUp(this, new RawKeyEventArgs(vkCode, false));
                }
            }
            return InterceptKeys.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        public event RawKeyEventHandler KeyDown;
        public event RawKeyEventHandler KeyUp;

        public KeyboardListener()
        {
            hookId = InterceptKeys.SetHook(HookCallback);
        }

        ~KeyboardListener()
        {
            Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            InterceptKeys.UnhookWindowsHookEx(hookId);
        }

        #endregion
    }

    internal static class InterceptKeys
    {
        public delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        public static int WH_KEYBOARD_LL = 13;
        public static int WM_KEYDOWN = 0x0100;
        public static int WM_KEYUP = 0x0101;

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }

    public class RawKeyEventArgs : EventArgs
    {
        public int VKCode;
        public Key Key;
        public bool IsSysKey;

        public RawKeyEventArgs(int VKCode, bool isSysKey)
        {
            this.VKCode = VKCode;
            this.IsSysKey = isSysKey;
            this.Key = System.Windows.Input.KeyInterop.KeyFromVirtualKey(VKCode);
        }
    }

    public delegate void RawKeyEventHandler(object sender, RawKeyEventArgs args);

}

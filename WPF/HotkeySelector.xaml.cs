using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Apollo
{
    /// <summary>
    /// Interaction logic for HotkeySelector.xaml
    /// </summary>
    public partial class HotkeySelector : UserControl
    {
        public string ButtonText { get; set; } = "Select a Hotkey...";

        private List<Key> _hotkeys = new();
        public List<Key> SelectedHotkeys
        {
            get => _hotkeys;
            set
            {
                _hotkeys = value;
                ButtonText = string.Join("+", value.Select(i => KeyMap.KeyToChar(i)).ToList());
            }
        }

        private bool active;
        public bool MultiKey { get; set; }


        public event EventHandler? HotkeyAssigned;

        public bool isActive
        {
            get { return active; }
            set
            {
                active = value;

                if (value)
                {
                    SelectedHotkeys.Clear();
                    InputHandler._GlobalHook.KeyDown += Listen;
                    if (MultiKey) InputHandler._GlobalHook.KeyUp += WaitForKeyUp;
                    base.BackColor = ActiveColor;
                }
                else
                {
                    HotkeyAssigned?.Invoke(this, EventArgs.Empty);
                    InputHandler._GlobalHook.KeyDown -= Listen;
                    if (MultiKey) InputHandler._GlobalHook.KeyUp -= WaitForKeyUp;
                    base.BackColor = InactiveColor;
                }
            }
        }

        public HotkeySelector()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

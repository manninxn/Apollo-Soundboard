using System.ComponentModel;
using System.Diagnostics;

namespace Apollo_Soundboard
{
    public partial class HotkeySelector : Button
    {
        public List<Keys> SelectedHotkeys = new List<Keys>();

        private bool active;
        public bool MultiKey { get; set; }

        private EventHandler onHotkeyAssigned;

        public event EventHandler HotkeyAssigned
        {
            add
            {
                onHotkeyAssigned += value;
            }
            remove
            {
                onHotkeyAssigned -= value;
            }
        }

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
                    onHotkeyAssigned?.Invoke(this, EventArgs.Empty);
                    InputHandler._GlobalHook.KeyDown -= Listen;
                    if (MultiKey) InputHandler._GlobalHook.KeyUp -= WaitForKeyUp;
                    base.BackColor = InactiveColor;
                }
            }
        }
        [
            PropertyTab("Appearance"),
            Browsable(true),
            Category("Custom"),
            Description("The active color")
        ]
        public Color ActiveColor { get; set; }
        [
            PropertyTab("Appearance"),
            Browsable(true),
            Category("Custom"),
            Description("The inactive color"),
        ]
        public Color InactiveColor { get; set; }





        protected override void OnClick(EventArgs e)
        {
            isActive = !isActive;
            Debug.WriteLine(isActive);
            base.OnClick(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            Debug.WriteLine("left");
            isActive = false;
            // Call the base class
            base.OnLeave(e);
        }
        int numKeys = 0;
        void Listen(object sender, KeyEventArgs e)
        {
            Keys keyCode = KeyMap.ParseModifierKey(e.KeyCode);

            if (SelectedHotkeys.Contains(keyCode)) SelectedHotkeys.Remove(keyCode); else numKeys++;
            SelectedHotkeys.Add(keyCode);

            Text = String.Join("+", SelectedHotkeys.Select(i => KeyMap.KeyToChar(i)).ToList());
            if (!MultiKey) isActive = false;
        }

        void WaitForKeyUp(object sender, KeyEventArgs e)
        {
            numKeys--;
            Debug.WriteLine(numKeys);
            if (numKeys == 0) isActive = false;

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HotkeySelector
            // 
            this.Name = "HotkeySelector";
            this.Size = new System.Drawing.Size(267, 150);
            this.ResumeLayout(false);

        }
    }
}

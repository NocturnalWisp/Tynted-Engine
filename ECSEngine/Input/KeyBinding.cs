using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ECSEngine.SFML.Window.Mouse;
using static ECSEngine.SFML.Window.Keyboard;
using static ECSEngine.SFML.Window.Joystick;
using ECSEngine.SFML.Window;

namespace ECSEngine.Input
{
	/// <summary>
	/// Input bindings.
	/// </summary>
    public struct KeyBinding
    {
        public string Name { get; set; }
        public List<Key> Keys { get; set; }

        public List<Button> MouseButtons { get; set; }
        public List<uint> GamePadButtons { get; set; }

        public bool JustPressed { get; set; }
        public bool IsDown { get; set; }

        public KeyBinding(string name)
        {
            Name = name;

            Keys = new List<Key>();
			MouseButtons = new List<Button>();
			GamePadButtons = new List<uint>();

			JustPressed = false;
			IsDown = false;
        }

        public KeyBinding(string name, List<Key> keys = default, List<Button> mouseButtons = default, List<uint> gamePadButtons = default)
        {
            Name = name;

            Keys = keys;
            MouseButtons = mouseButtons;
            GamePadButtons = gamePadButtons;

			JustPressed = false;
			IsDown = false;
		}
    }
}

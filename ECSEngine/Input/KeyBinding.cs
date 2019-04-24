using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ECSEngine.SFML.Window.Mouse;
using static ECSEngine.SFML.Window.Keyboard;
using ECSEngine.SFML.Window;

namespace ECSEngine.Input
{
	/// <summary>
	/// Input bindings.
	/// </summary>
    public struct KeyBinding
    {
		/// <summary>
		/// The name of the binding.
		/// </summary>
        public string Name { get; set; }
        public List<Key> Keys { get; set; }

        /// <summary>
        /// The list of buttons this binding has.
        /// </summary>
        public List<Button> Buttons { get; set; }

		/// <summary>
		/// If the binding was just pressed.
		/// </summary>
        public bool JustPressed { get; set; }
		/// <summary>
		/// If the binding is down.
		/// </summary>
        public bool IsDown { get; set; }

        public KeyBinding(string name)
        {
            Name = name;

            Keys = new List<Key>();
            Buttons = new List<Button>();

			JustPressed = false;
			IsDown = false;
        }

        public KeyBinding(string name, List<Key> keys, List<Button> buttons)
        {
            Name = name;

            Keys = keys;
            Buttons = buttons;

			JustPressed = false;
			IsDown = false;
		}
    }
}

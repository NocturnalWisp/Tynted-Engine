using ECSEngine.SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ECSEngine.SFML.Window.Keyboard;
using static ECSEngine.SFML.Window.Mouse;

namespace ECSEngine.Input
{
	public static class InputManager
	{
		private static List<KeyBinding> bindings = new List<KeyBinding>();

		internal static void Initialize(Window window)
		{
			window.KeyPressed += KeyPressed;
			window.KeyReleased += KeyReleased;

			window.MouseButtonPressed += MousePressed;
		}

		public static void JustPressedReset()
		{
			for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
			{
				KeyBinding kb = bindings[bindingIndex];
				kb.JustPressed = false;
				bindings[bindingIndex] = kb;
			}
		}

		public static void AddBinding(KeyBinding binding)
		{
			if (!bindings.Contains(binding))
			{
				bindings.Add(binding);
			}
		}

		public static KeyBinding GetBinding(string name)
		{
			return bindings.Find(o => o.Name == name);
		}

		private static void KeyPressed(object sender, KeyEventArgs e)
		{
			for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
			{
				KeyBinding kb = bindings[bindingIndex];

				for (int keyIndex = 0; keyIndex < bindings[bindingIndex].Keys.Count; keyIndex++)
				{
					Key key = bindings[bindingIndex].Keys[keyIndex];

					if (key == e.Code)
					{
						if (!kb.IsDown)
						{
							kb.IsDown = true;
							kb.JustPressed = false;
						}
					}
				}

				bindings[bindingIndex] = kb;
			}
		}

		private static void KeyReleased(object sender, KeyEventArgs e)
		{
			for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
			{
				KeyBinding kb = bindings[bindingIndex];

				for (int keyIndex = 0; keyIndex < bindings[bindingIndex].Keys.Count; keyIndex++)
				{
					Key key = bindings[bindingIndex].Keys[keyIndex];

					if (key == e.Code)
					{
						if (kb.IsDown)
						{
							kb.IsDown = false;
							kb.JustPressed = true;
						}
					}
				}

				bindings[bindingIndex] = kb;
			}
		}

		private static void MousePressed(object sender, MouseButtonEventArgs e)
		{
			for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
			{
				KeyBinding kb = bindings[bindingIndex];

				for (int buttonIndex = 0; buttonIndex < bindings[bindingIndex].Buttons.Count; buttonIndex++)
				{
					Button button = bindings[bindingIndex].Buttons[buttonIndex];

					if (button == e.Button)
					{
						kb.JustPressed = true;
					}
				}

				bindings[bindingIndex] = kb;
			}
		}
	}
}
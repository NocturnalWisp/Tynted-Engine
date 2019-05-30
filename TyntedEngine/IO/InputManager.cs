using Tynted.SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Tynted.SFML.Window.Keyboard;
using static Tynted.SFML.Window.Mouse;

namespace Tynted.IO
{
	public static class InputManager
	{
		private static List<KeyBinding> bindings = new List<KeyBinding>();

		internal static void Initialize(Window window)
		{
			window.KeyPressed += KeyPressed;
			window.KeyReleased += KeyReleased;

			window.MouseButtonPressed += MousePressed;
			window.MouseButtonReleased += MousePressed;

			window.JoystickButtonPressed += GamePadButtonPressed;
			window.JoystickButtonReleased += GamePadButtonReleased;
		}

		internal static void Update(GameTime gameTime)
		{
			Joystick.Update();
		}

		internal static void JustPressedReset()
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

        public static void RemoveBinding(string name)
        {
            bindings.RemoveAll(o => o.Name == name);
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
							kb.JustPressed = true;
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
							kb.JustPressed = false;
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

				for (int buttonIndex = 0; buttonIndex < bindings[bindingIndex].MouseButtons.Count; buttonIndex++)
				{
					Button button = bindings[bindingIndex].MouseButtons[buttonIndex];

					if (button == e.Button)
					{
						if (!kb.IsDown)
						{
							kb.JustPressed = true;
							kb.IsDown = true;
						}
					}
				}

				bindings[bindingIndex] = kb;
			}
		}

		private static void MouseReleased(object sender, MouseButtonEventArgs e)
		{
			for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
			{
				KeyBinding kb = bindings[bindingIndex];

				for (int buttonIndex = 0; buttonIndex < bindings[bindingIndex].MouseButtons.Count; buttonIndex++)
				{
					Button button = bindings[bindingIndex].MouseButtons[buttonIndex];

					if (button != e.Button)
					{
						if (kb.IsDown)
						{
							kb.JustPressed = false;
							kb.IsDown = false;
						}
					}
				}

				bindings[bindingIndex] = kb;
			}
		}

		private static void GamePadButtonPressed(object sender, JoystickButtonEventArgs e)
		{
            if(bindings.Count(o => o.GamePadButtons.Count > 0) <= 0) return;

			for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
			{
				KeyBinding kb = bindings[bindingIndex];

				for (int buttonIndex = 0; buttonIndex < bindings[bindingIndex].Keys.Count; buttonIndex++)
				{
					uint button = bindings[bindingIndex].GamePadButtons[buttonIndex];

					if (button == e.Button)
					{
						if (!kb.IsDown)
						{
							kb.IsDown = true;
							kb.JustPressed = true;
						}
					}
				}

				bindings[bindingIndex] = kb;
			}
		}

		private static void GamePadButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            if (bindings.Count(o => o.GamePadButtons.Count > 0) <= 0) return;

            for (int bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
			{
				KeyBinding kb = bindings[bindingIndex];

				for (int buttonIndex = 0; buttonIndex < bindings[bindingIndex].Keys.Count; buttonIndex++)
				{
					uint button = bindings[bindingIndex].GamePadButtons[buttonIndex];

					if (button == e.Button)
					{
						if (kb.IsDown)
						{
							kb.IsDown = false;
							kb.JustPressed = false;
						}
					}
				}

				bindings[bindingIndex] = kb;
			}
		}
	}
}
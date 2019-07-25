using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using OpenTK.Platform;

namespace Tynted
{
	public class Window
	{
		private GameWindow window;
		internal IWindowInfo WindowInfo
		{
			get => window.WindowInfo;
		}

		public int Width
		{
			get => window.Width;
			set => window.Width = value;
		}
		public int Height
		{
			get => window.Height;
			set => window.Height = value;
		}
		public string Title
		{
			get => window.Title;
			set => window.Title = value;
		}

		internal List<int> VBOs = new List<int>();

		public event EventHandler<EventArgs> OnLoad;
		public event EventHandler<FrameEventArgs> OnUpdate;
		public event EventHandler<FrameEventArgs> OnDraw;
		public event EventHandler<EventArgs> OnClosed;

		internal void Run(string title, double? updateRate)
		{
			window = new GameWindow(600, 400, GraphicsMode.Default, title,
				GameWindowFlags.Default, DisplayDevice.Default);

			window.Load += OnLoad;
			window.UpdateFrame += (object sender, FrameEventArgs args) => Update();
			window.UpdateFrame += OnUpdate;
			window.RenderFrame += OnDraw;
			window.Closed += OnClosed;
			window.Closed += (object sender, EventArgs args) => RemoveVBOs();

			if (updateRate == null)
				window.Run();
			else
				window.Run((double)updateRate);
		}

		internal void Update()
		{
			window.ProcessEvents();
		}

		internal void Close()
		{
			window.Close();
		}

		private void RemoveVBOs()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			for (int i = VBOs.Count-1; i >= 0; i--)
			{
				GL.DeleteBuffer(VBOs[i]);
			}
			VBOs.Clear();
			VBOs = null;
		}
	}
}

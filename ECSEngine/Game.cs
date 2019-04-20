using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML;
using SFML.Window;
using SFML.Graphics;

namespace ECSEngine
{
	public class Game : IDisposable
	{
		RenderWindow window;

		public void Run()
		{
			window = new RenderWindow(new VideoMode(1024, 768), "New Window");

			window.Closed += WindowClosed;

			RunLoop();
		}

		private void WindowClosed(object sender, EventArgs e)
		{
			OnClosed();

			window.Close();
		}

		private void RunLoop()
		{
			Initialize();

			while (window.IsOpen)
			{
				window.DispatchEvents();

				Update();

				window.Clear(Color.Black);

				Draw(window);

				window.Display();
			}
		}

		protected virtual void Initialize()
		{

		}

		protected virtual void Update()
		{

		}

		protected virtual void Draw(RenderWindow renderWindow)
		{

		}

		protected virtual void OnClosed()
		{

		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Game() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}

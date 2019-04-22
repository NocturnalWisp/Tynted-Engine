using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace ECSEngine
{
	public class Game : IDisposable
	{
		RenderWindow window;

		private Time elapsedTime;
		public Time ElapsedTime { get => elapsedTime; }

		GameOptions gameOptions;
		GameTime gameTime;

		public Game(GameOptions options)
		{
			gameOptions = options;
		}

		public void Run()
		{
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
			{
				//Grabs the components from the Assembly
				if (typeof(IComponent).IsAssignableFrom(type) && type != typeof(IComponent))
				{
					SystemManager.AddComponent((IComponent)Activator.CreateInstance(type));
					Console.WriteLine("Added component: " + type);
				}
				//Grabs the Systems from the Assembly
				else if (type.IsSubclassOf(typeof(System)))
				{
					SystemManager.AddSystem((System)Activator.CreateInstance(type));
					Console.WriteLine("Added system: " + type);
				}
			}

			window = new RenderWindow(new VideoMode(1024, 768), "New Window");

			if (gameOptions.forceLimit)
			{
				window.SetFramerateLimit(60);

				Console.WriteLine("Boo");
			}

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

			Clock deltaClock = new Clock();
			Clock totalTime = new Clock();

			while (window.IsOpen)
			{
				window.DispatchEvents();

				//Time stamps
				gameTime.elapsedTime = deltaClock.Restart();
				gameTime.totalTime = totalTime.ElapsedTime;

				Update(gameTime);

				window.Clear(Color.Black);

				Draw(window);

				window.Display();
			}
		}

		/// <summary>
		/// Place all entity and system initialization here.
		/// </summary>
		protected virtual void Initialize()
		{

		}

		/// <summary>
		/// The update method that runs through all the systems and updates them.
		/// </summary>
		/// <param name="gameTime">The time tool to get various variables</param>
		protected virtual void Update(GameTime gameTime)
		{
			SystemManager.Update(gameTime);
		}

		/// <summary>
		/// The draw method that loops through each system and draws them.
		/// </summary>
		/// <param name="renderWindow">The window to draw them in.</param>
		protected virtual void Draw(RenderWindow renderWindow)
		{
			SystemManager.Draw(renderWindow);
		}

		/// <summary>
		/// Callback when the game window is closed.
		/// </summary>
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

using Tynted.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public class Scene
	{
		private string sceneName;
		public string SceneName { get => sceneName; }

		public Scene(string sceneName)
		{
			this.sceneName = sceneName;
		}

		internal virtual void Initialize()
		{

		}

		internal virtual void Update(GameTime gameTime)
		{

		}

		internal virtual void Draw(RenderWindow window)
		{

		}

		internal virtual void OnClosed()
		{

		}
	}
}

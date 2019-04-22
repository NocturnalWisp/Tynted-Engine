using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.Components;
using ECSEngine.SFML.Graphics;
using ECSEngine.SFML.System;

namespace GameTest
{
	class TestGame : Game
	{
		public TestGame(GameOptions options) : base(options)
		{

		}

		protected override void Initialize()
		{
			SystemManager.AddEntityComponent(0, new SpriteRenderee(new Texture("Art/AirShip.png")));
			SystemManager.AddEntityComponent(0, new ECSEngine.Components.Transform(new Vector2f(0, 0)));

			SystemManager.AddEntityComponent(1, new SpriteRenderee(new Texture("Art/AirShip.png")));
			SystemManager.AddEntityComponent(1, new ECSEngine.Components.Transform(new Vector2f(300, 300)));

			base.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			ECSEngine.Components.Transform t = SystemManager.GetEntityComponent<ECSEngine.Components.Transform>(1);
			t.position += new Vector2f(1, 1);
			SystemManager.SetEntityComponent(1, t);

			base.Update(gameTime);
		}

		protected override void Draw(RenderWindow window)
		{


			base.Draw(window);
		}
	}
}

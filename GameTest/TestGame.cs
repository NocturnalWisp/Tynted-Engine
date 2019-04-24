using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.Components;
using ECSEngine.SFML.Graphics;
using ECSEngine.SFML.System;

using Box2DNet.Common;
using ECSEngine.Systems;

namespace GameTest
{
	class TestGame : Game
	{
		public TestGame(GameOptions options) : base(options) { }

		protected override void Initialize()
		{
			ECSManager.CreateEntity("Airship");
			ECSManager.CreateEntity("Airship2");

			ECSManager.RegisterEntityComponents(new List<KeyValuePair<string, IComponent>>()
			{
				new KeyValuePair<string, IComponent>("Airship", new SpriteRenderee(new Texture("Art/AirShip.png"))),
				new KeyValuePair<string, IComponent>("Airship", new ECSEngine.Components.Transform(new Vec2(1, 1))),
				new KeyValuePair<string, IComponent>("Airship", new RigidBody(new Vec2(10, 10), PhysicsManager.World)),

				new KeyValuePair<string, IComponent>("Airship2", new SpriteRenderee(new Texture("Art/AirShip.png"))),
				new KeyValuePair<string, IComponent>("Airship2", new ECSEngine.Components.Transform(new Vec2(300, 300))),
			});

			base.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{

			base.Update(gameTime);
		}

		protected override void Draw(RenderWindow window)
		{


			base.Draw(window);
		}
	}
}

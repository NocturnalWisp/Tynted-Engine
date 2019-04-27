using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.Components;
using ECSEngine.SFML.Graphics;
using ECSEngine.SFML.System;

using ECSEngine.Systems;
using ECSEngine.Input;
using static ECSEngine.SFML.Window.Keyboard;
using static ECSEngine.SFML.Window.Mouse;

using Box2DNet.Collision;
using Box2DNet.Common;

namespace GameTest
{
	class TestGame : Game
	{
		public TestGame(GameOptions options) : base(options) { }

		protected override void Initialize()
		{
			//TODO: Something weird occurs when one is disabled and the second is enabled.
			//They seem to not be grabbing the right component?
			ECSManager.CreateEntity("Airship");
			ECSManager.CreateEntity("Airship2");

			Texture sprite = new Texture("Resources/Art/AirShip.png");

			PolygonShape shipShape = new PolygonShape();

			shipShape.SetAsBox(sprite.Size.X, sprite.Size.Y);

			ECSManager.RegisterEntityComponents(new List<EntityComponentIdentifier>()
			{
				new EntityComponentIdentifier("Airship", new SpriteRenderee(sprite)),
				new EntityComponentIdentifier("Airship", new ECSEngine.Components.Transform(new Vec2(0, 0))),
				new EntityComponentIdentifier("Airship", new RigidBody(new Vec2(100, 100), PhysicsManager.World, 1, shipShape)),

				new EntityComponentIdentifier("Airship2", new SpriteRenderee(sprite)),
				new EntityComponentIdentifier("Airship2", new ECSEngine.Components.Transform(new Vec2(200, 200))),
				new EntityComponentIdentifier("Airship2", new RigidBody(new Vec2(-100, -100), PhysicsManager.World, 1, shipShape)),
			});
			
			base.Initialize();
		}
	}
}
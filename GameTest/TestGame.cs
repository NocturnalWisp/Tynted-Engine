﻿using System;
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
			ECSManager.CreateEntity("Airship");
			ECSManager.CreateEntity("Airship2");

			Texture sprite = new Texture("Art/AirShip.png");

			PolygonShape shipShape = new PolygonShape();

			shipShape.SetAsBox(sprite.Size.X, sprite.Size.Y);

			ECSManager.RegisterEntityComponents(new List<KeyValuePair<string, IComponent>>()
			{
				new KeyValuePair<string, IComponent>("Airship", new SpriteRenderee(sprite)),
				new KeyValuePair<string, IComponent>("Airship", new ECSEngine.Components.Transform(new Vec2(1, 1))),
				new KeyValuePair<string, IComponent>("Airship", new RigidBody(new Vec2(10, 10), PhysicsManager.World, 1, shipShape)),

				new KeyValuePair<string, IComponent>("Airship2", new SpriteRenderee(sprite)),
				new KeyValuePair<string, IComponent>("Airship2", new ECSEngine.Components.Transform(new Vec2(300, 300))),
			});
			
			base.Initialize();
		}
	}
}

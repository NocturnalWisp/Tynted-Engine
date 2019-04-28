using Box2DNet.Collision;
using Box2DNet.Common;

using ECSEngine;
using ECSEngine.Components;
using ECSEngine.Input;
using ECSEngine.SFML.Graphics;
using ECSEngine.Systems;
using static ECSEngine.SFML.Window.Keyboard;
using static ECSEngine.SFML.Window.Mouse;

using System.Collections.Generic;
using System;

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

			InputManager.AddBinding(new KeyBinding("Up", 
				new List<Key>() { Key.W, Key.Up }, 
				new List<Button>() { Button.Left }, 
				new List<uint>() { 3 })
			);

			InputManager.AddBinding(new KeyBinding("Down",
				new List<Key>() { Key.S, Key.Down },
				new List<Button>() { Button.Right },
				new List<uint>() { 0 })
			);

			base.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			if (InputManager.GetBinding("Up").JustPressed)
			{
				RigidBody rb = (RigidBody)ECSManager.GetEntityComponent("Airship", typeof(RigidBody));
				rb.body.SetLinearVelocity(new System.Numerics.Vector2(-100, -100));
				ECSManager.SetEntityComponent("Airship", rb);
			}else if (InputManager.GetBinding("Down").JustPressed)
			{
				RigidBody rb = (RigidBody)ECSManager.GetEntityComponent("Airship", typeof(RigidBody));
				rb.body.SetLinearVelocity(new System.Numerics.Vector2(100, 100));
				ECSManager.SetEntityComponent("Airship", rb);
			}

			base.Update(gameTime);
		}
	}
}
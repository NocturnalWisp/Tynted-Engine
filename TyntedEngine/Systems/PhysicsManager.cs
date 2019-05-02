using Box2DNet.Collision;
using Box2DNet.Common;
using Box2DNet.Dynamics;

using Tynted.Components;
using Tynted.Events;
using Transform = Tynted.Components.Transform;

using System.Collections.Generic;
using System.Numerics;
using System;

namespace Tynted.Systems
{
	[GetComponents(typeof(RigidBody), typeof(Transform))]
	public class PhysicsManager : System
	{
		private static World world = new World(new AABB(new Vector2(-2000, -2000), new Vector2(2000, 2000)), new Vec2(0, 0), false);
		public static World World { get => world; }

		TyntedEvent<object, object> collisionEvent;

		public override void CreateEvents()
		{
			collisionEvent = ECSManager.CreateEvent2Arg("OnCollisionEnter");

			base.CreateEvents();
		}

		public override void Initialize()
		{
			var allTypes = GetEntities();

			foreach (Entity entity in allTypes)
			{
				entity.GetComponent<RigidBody>().body.SetPosition(entity.GetComponent<Transform>().GetWorldPosition());
			}

			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			var allTypes = GetEntities();

			world.Step(gameTime.elapsedTime.AsSeconds(), 8, 3);

			foreach (Entity entity in allTypes)
			{
				Transform transform = entity.GetComponent<Transform>();
				transform.LocalPosition = entity.GetComponent<RigidBody>().body.GetPosition();
				entity.SetComponent(transform);
			}

			base.Update(gameTime);
		}
	}
}

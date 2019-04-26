using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using ECSEngine.Components;
using ECSEngine.Events;

using Box2DNet.Dynamics;
using Box2DNet.Common;
using Box2DNet.Collision;

namespace ECSEngine.Systems
{
	public class PhysicsManager : System
	{
		private static World world = new World(new AABB(new Vector2(0, 0), new Vector2(1024, 768)), new Vec2(0, -9.81f), false);
		public static World World { get => world; }

		List<RigidBody> rigidbody = new List<RigidBody>();

		EngineEvent<object, object> collisionEvent;

		public override void CreateEvents()
		{
			collisionEvent = ECSManager.CreateEvent2Arg("OnCollisionEnter");

			base.CreateEvents();
		}

		public override void Update(GameTime gameTime)
		{
			world.Step(gameTime.elapsedTime.AsSeconds(), 8, 3);

			var rigidBodies = ECSManager.GetComponentEntityActiveList<RigidBody>();
			var transforms = ECSManager.GetComponentEntityActiveList<Components.Transform>();

			foreach (EntityComponent rigidBody in rigidBodies)
			{
				RigidBody rb = (RigidBody)rigidBody.component;

				if (rb.mass > 0)
				{
					EntityComponent tComponent = transforms.Find(o => o.entityID == rigidBody.entityID);
					Components.Transform t = (Components.Transform)tComponent.component;

					t.position = rb.body.GetPosition();
					tComponent.component = t;

					transforms[transforms.IndexOf(transforms.Find(o => o.entityID == rigidBody.entityID))] = tComponent;
				}
			}

			base.Update(gameTime);
		}
	}
}

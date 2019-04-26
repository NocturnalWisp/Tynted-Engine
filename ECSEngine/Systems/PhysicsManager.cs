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

			Dictionary<int, IComponent> rigidBodies = ECSManager.GetComponentEntityActiveList(new RigidBody());
			Dictionary<int, IComponent> transforms = ECSManager.GetComponentEntityActiveList(new Components.Transform());

			foreach (KeyValuePair<int, IComponent> rigidBody in rigidBodies)
			{
				RigidBody rb = (RigidBody)rigidBody.Value;

				if (rb.mass > 0)
				{
					Components.Transform t = (Components.Transform)transforms[rigidBody.Key];

					t.position = rb.body.GetPosition();
					transforms[rigidBody.Key] = t;
				}
			}

			base.Update(gameTime);
		}
	}
}

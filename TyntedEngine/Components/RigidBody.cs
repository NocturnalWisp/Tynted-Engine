using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Box2DNet.Collision;
using Box2DNet.Common;
using Box2DNet.Dynamics;

namespace Tynted.Components
{
	public struct RigidBody : IComponent
	{
		public bool Enabled { get; set; }

		public Vec2 velocity;
		public float mass;

		private Shape shape;
		public Body body;

		/// <summary>
		/// Creates a new RigidBody Component.
		/// </summary>
		/// <param name="initialVelocity">The velocity at start,</param>
		/// <param name="world">The world in which the object lives.</param>
		/// <param name="mass">The mass of the object in kg.</param>
		/// <param name="shape">The shape object used to define appearance and functionality.</param>
		public RigidBody(Vec2 initialVelocity, World world, float mass = 1, Shape shape = default(Shape))
		{
			velocity = initialVelocity;
			this.mass = mass;

			body = world.CreateBody(new BodyDef() { LinearVelocity = velocity, MassData = new MassData() { Mass = mass } });

			if (shape == default(Shape))
			{
				this.shape = new PolygonShape();
				((PolygonShape)this.shape).SetAsBox(0.5f, 0.5f);
			}
			else
			{
				this.shape = shape;
			}
			
			body.CreateFixture(new PolygonDef() { Type = ShapeType.PolygonShape });

			Enabled = true;
		}
    }
}

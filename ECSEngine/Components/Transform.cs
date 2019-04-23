using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.SFML.System;

using Box2DNet.Common;

namespace ECSEngine.Components
{
	public struct Transform : IComponent
	{
		public bool Enabled { get; set; }

		public Vec2 position;
		public float rotation;
		public Vec2 scale;

		public Transform(Vec2 position, float rotation = 0, Vec2 scale = default(Vec2))
		{
			this.position = position;
			this.rotation = rotation;

			if (scale == default(Vec2))
				this.scale = Vec2.One;
			else
				this.scale = scale;

			Enabled = true;
		}
	}
}

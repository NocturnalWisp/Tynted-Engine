using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

namespace ECSEngine.Components
{
	public struct Transform : IComponent
	{
		public Vector2f position;

		public Transform(Vector2f position)
		{
			this.position = position;
		}
	}
}

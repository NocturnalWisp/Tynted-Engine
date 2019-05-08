using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted.Components
{
	/// <summary>
	/// Identifies an entity name to a component that entity has.
	/// </summary>
	public struct EntityComponentIdentifier
	{
		public string name;
		public IComponent component;

		public EntityComponentIdentifier(string name, IComponent component)
		{
			this.name = name;
			this.component = component;
		}
	}
}

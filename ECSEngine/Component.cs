using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	public struct Component<T> where T : IComponent
	{
		internal List<EntityComponent> entityComponents;
	}

	public struct EntityComponent
	{
		internal int entityID;
		internal IComponent component;

		public EntityComponent(int entityID, IComponent component)
		{
			this.entityID = entityID;
			this.component = component;
		}
	}
}

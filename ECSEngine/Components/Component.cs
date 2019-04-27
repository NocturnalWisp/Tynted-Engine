using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	public class Component
	{
		public Type componentType;
		public  List<EntityComponent> entityComponents = new List<EntityComponent>();

		public Component(Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				this.componentType = componentType;
				entityComponents = new List<EntityComponent>();
			}
			else
			{
				this.componentType = typeof(IComponent);
				entityComponents = new List<EntityComponent>();
				throw new Exception("Component type must be of type IComponent.");
			}
		}
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

        public static bool operator ==(EntityComponent a, EntityComponent b)
        {
            return (a.entityID == b.entityID) && (a.component == b.component);
        }

        public static bool operator !=(EntityComponent a, EntityComponent b)
        {
            return (a.entityID != b.entityID) || (a.component != b.component);
        }
    }
}

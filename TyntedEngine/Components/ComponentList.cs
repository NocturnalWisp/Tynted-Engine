using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public class ComponentList
	{
		public Type componentType;
		public  List<EntityComponent> entityComponents = new List<EntityComponent>();

		public ComponentList(Type componentType)
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
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	/// <summary>
	/// This class is utilized specifically for systems.
	/// The behind the scenes implementation does not have
	/// a list of entities, more a list of components.
	/// </summary>
	public class Entity
	{
		public int entityID;
		internal List<IComponent> components = new List<IComponent>();

		public Entity(int entityID)
		{
			this.entityID = entityID;
		}

		/// <summary>
		/// Gets a component based on the type.
		/// </summary>
		/// <param name="componentType">The type of component to grab.</param>
		/// <returns>The component if found, otherwise null.</returns>
		public IComponent GetComponent(Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				return components.Find(o => o.GetType() == componentType);
			}

			return null;
		}

		/// <summary>
		/// Template version of the GetComponent function.
		/// </summary>
		/// <typeparam name="T">The type of component to find.</typeparam>
		/// <returns>The component if found, otherwise null.</returns>
		public T GetComponent<T>() where T : IComponent
		{
			IComponent component = GetComponent(typeof(T));
			if (component != null)
				return (T)component;
			else
				return default;
		}

		public void SetComponent(IComponent component)
		{
			components[components.FindIndex(o => o.GetType() == component.GetType())] = component;
		}
	}
}

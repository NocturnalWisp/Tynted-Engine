using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.SFML.Graphics;

namespace ECSEngine
{
	public class SystemManager
	{
		static List<System> systems = new List<System>();

		static Dictionary<IComponent, Dictionary<int, IComponent>> components = new Dictionary<IComponent, Dictionary<int, IComponent>>();

		public static void Update(GameTime gameTime)
		{
			foreach (System system in systems)
			{
				system.Update(gameTime);
			}
		}

		public static void Draw(RenderWindow window)
		{
			foreach (System system in systems)
			{
				system.Draw(window);
			}
		}

		#region System and Component Utility Functions
		public static void AddSystem(System system)
		{
			if (!systems.Contains(system))
			{
				systems.Add(system);
			}
		}

		public static void RemoveSystem(System system)
		{
			if (systems.Contains(system))
			{
				systems.Remove(system);
			}
		}

		public static List<IComponent> GetComponents()
		{
			return components.Keys.ToList();
		}

		public static void AddComponent(IComponent component)
		{
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 0)
			{
				components.Add((IComponent)Activator.CreateInstance(component.GetType()), new Dictionary<int, IComponent>());
			}
		}

		public static void RemoveComponent(IComponent component)
		{
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				components.Remove(components.First(o => o.Key.GetType() == component.GetType()).Key);
			}
		}

		public static Dictionary<int, IComponent> GetComponentEntityList(IComponent component)
		{
			return components.First(o => o.Key.GetType() == component.GetType()).Value;
		}

		public static void AddEntityComponent(int entityID, IComponent component)
		{
			//make sure it has component
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				//make sure entity does not already exist
				if (!GetComponentEntityList(component).ContainsKey(entityID))
				{
					components.First(o => o.Key.GetType() == component.GetType()).Value.Add(entityID, component);
				}
			}
		}

		public static void RemoveEntityComponent(int entityID, IComponent component)
		{
			//make sure it has component
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				//make sure entity already exist
				if (GetComponentEntityList(component).ContainsKey(entityID))
				{
					components.First(o => o.Key.GetType() == component.GetType()).Value.Remove(entityID);
				}
			}
		}

		public static void RemoveAllEntityComponents(int entityID)
		{
			List<IComponent> componentList = components.Keys.ToList();
			for (int i = 0; i < componentList.Count; i++)
			{
				var component = components[componentList[i]];
				if (component.ContainsKey(entityID))
				{
					components.First(o => o.Key.GetType() == component.GetType()).Value.Remove(entityID);
				}
			}
		}

		public static List<IComponent> GetEntityComponents(int entityID)
		{
			List<IComponent> componentList = components.Keys.ToList();

			for (int i = 0; i < componentList.Count; i++)
			{
				if (!components[componentList[i]].ContainsKey(entityID))
				{
					componentList.RemoveAt(i);
				}
			}

			return componentList;
		}
		#endregion
	}
}

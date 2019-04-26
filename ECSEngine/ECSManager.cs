using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.Events;
using ECSEngine.SFML.Graphics;

namespace ECSEngine
{
	public class ECSManager
	{
		static List<EntityData> entities = new List<EntityData>();
		static int nextEntity = 0;

		static List<System> systems = new List<System>();

		static List<Component<IComponent>> components = new List<Component<IComponent>>();

		//TODO: Create a list of events here that systems can create and others can subscribe to.
		static Dictionary<string, EngineEvent> events = new Dictionary<string, EngineEvent>();
		static Dictionary<string, EngineEvent<object>> events1 = new Dictionary<string, EngineEvent<object>>();
		static Dictionary<string, EngineEvent<object, object>> events2 = new Dictionary<string, EngineEvent<object, object>>();

		public static void Initialize()
		{
			foreach (System system in systems)
			{
				system.Initialize();

				system.CreateEvents();
			}

			foreach (System system in systems)
			{
				system.SubscribeEvents();
			}
		}

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

		public static List<Component<IComponent>> GetComponents()
		{
			return components.ToList();
		}
		
		public static void AddComponent<T>() where T : IComponent
		{
			//Make sure component doesn't already exist
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 0)
			{
				components.Add(new Component<IComponent>());
			}
		}

		public static void RemoveComponent<T>() where T : IComponent
		{
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				components.Remove(components.First(o => o.GetType() == typeof(T)));
			}
		}

		#endregion
		#region Entities Utility Functions
		public static List<EntityComponent> GetComponentEntityActiveList<T>() where T : IComponent
		{
			return (List<EntityComponent>)components.First(o => o.GetType() == typeof(T)).entityComponents.Where(o => o.component.Enabled);
		}

		public static List<EntityComponent> GetComponentEntityList<T>() where T : IComponent
		{
			return components.First(o => o.GetType() == typeof(T)).entityComponents;
		}

		public static void CreateEntity(string name, string tag = "Default")
		{
			entities.Add(new EntityData(nextEntity, name, tag));
			nextEntity++;
		}

		public static void AddEntityComponent(string entityName, IComponent component)
		{
			//make sure component exists
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//Make sure data exists
				if (!eData.Equals(default(EntityData)))
				{
					//make sure entity does not already exist
					if (!GetComponentEntityList(component).ContainsKey(eData.EntityID))
					{
						components.First(o => o.Key.GetType() == component.GetType()).Value.Add(eData.EntityID, component);
					}
				}
			}
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

		public static void RegisterEntityComponents(List<KeyValuePair<string, IComponent>> entityComponents)
		{
			foreach (KeyValuePair<string, IComponent> entityComponent in entityComponents)
			{
				AddEntityComponent(entityComponent.Key, entityComponent.Value);
			}
		}

		public static void RemoveEntityComponent(string entityName, IComponent component)
		{
			//make sure component exists
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//Make sure data exists
				if (!eData.Equals(default(EntityData)))
				{
					//make sure entity component already exists
					if (GetComponentEntityList(component).ContainsKey(eData.EntityID))
					{
						components.First(o => o.Key.GetType() == component.GetType()).Value.Remove(eData.EntityID);
					}
				}
			}
		}

		public static void RemoveEntityComponent(int entityID, IComponent component)
		{
			//make sure it has component
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				//make sure entity component already exist
				if (GetComponentEntityList(component).ContainsKey(entityID))
				{
					components.First(o => o.Key.GetType() == component.GetType()).Value.Remove(entityID);
				}
			}
		}

		public static T GetEntityComponent<T>(string entityName) where T : IComponent
		{
			//make component exists
			if (components.Where(o => o.Key.GetType() == typeof(T)).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					return (T)components.First(o => o.Key.GetType() == typeof(T)).Value[eData.EntityID];
				}
			}

			return default(T);
		}

		public static T GetEntityComponent<T>(int entityID) where T : IComponent
		{
			//make component exists
			if (components.Where(o => o.Key.GetType() == typeof(T)).Count() == 1)
			{
				return (T)components.First(o => o.Key.GetType() == typeof(T)).Value[entityID];
			}

			return default(T);
		}

		public static void SetEntityComponent(string entityName, IComponent component)
		{
			//make component exists
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					components.First(o => o.Key.GetType() == component.GetType()).Value[eData.EntityID] = component;
				}
			}
		}

		public static void SetEntityComponent(int entityID, IComponent component)
		{
			//make component exists
			if (components.Where(o => o.Key.GetType() == component.GetType()).Count() == 1)
			{
				components.First(o => o.Key.GetType() == component.GetType()).Value[entityID] = component;
			}
		}

		public static void RemoveAllEntityComponents(string entityName)
		{
			List<IComponent> componentList = components.Keys.ToList();
			for (int i = componentList.Count-1; i >= 0; i--)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					RemoveEntityComponent(eData.EntityID, componentList[i]);
				}
			}
		}

		public static void RemoveAllEntityComponents(int entityID)
		{
			List<IComponent> componentList = components.Keys.ToList();
			for (int i = componentList.Count-1; i >= 0; i--)
			{
				RemoveEntityComponent(entityID, componentList[i]);
			}
		}

		public static List<IComponent> GetEntityComponents(string entityName)
		{
			List<IComponent> componentList = components.Keys.ToList();

			for (int i = 0; i < componentList.Count; i++)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					if (!components[componentList[i]].ContainsKey(eData.EntityID))
					{
						componentList.RemoveAt(i);
					}
				}
			}

			return componentList;
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
		#region Event Functions
		#region Create Events
		public static EngineEvent CreateEvent(string name)
		{
			EngineEvent ev = null;

			if (!events.ContainsKey(name))
			{
				ev = new EngineEvent();
				events[name] = ev;
			}

			return ev;
		}

		public static EngineEvent<object> CreateEvent1Arg(string name)
		{
			EngineEvent<object> ev = null;

			if (!events1.ContainsKey(name))
			{
				ev = new EngineEvent<object>();
				events1[name] = ev;
			}

			return ev;
		}

		public static EngineEvent<object, object> CreateEvent2Arg(string name)
		{
			EngineEvent<object, object> ev = null;

			if (!events2.ContainsKey(name))
			{
				ev = new EngineEvent<object, object>();
				events2[name] = ev;
			}

			return ev;
		}
		#endregion

		#region Subscribe Events
		public static void SubscribeEvent(string name, EngineAction action)
		{
			if (events.ContainsKey(name))
			{
				events[name].AddListener(action);
			}
		}

		public static void SubscribeEvent(string name, EngineAction<object> action)
		{
			if (events1.ContainsKey(name))
			{
				events1[name].AddListener(action);
			}
		}

		public static void SubscribeEvent(string name, EngineAction<object, object> action)
		{
			if (events2.ContainsKey(name))
			{
				events2[name].AddListener(action);
			}
		}
		#endregion

		#region Unsubscribe Events
		public static void UnSubscribeEvent(string name, EngineAction ev)
		{
			if (events.ContainsKey(name))
			{
				events[name].RemoveListener(ev);
			}
		}

		public static void UnSubscribeEvent(string name, EngineAction<object> ev)
		{
			if (events1.ContainsKey(name))
			{
				events1[name].RemoveListener(ev);
			}
		}

		public static void UnSubscribeEvent(string name, EngineAction<object, object> ev)
		{
			if (events2.ContainsKey(name))
			{
				events2[name].RemoveListener(ev);
			}
		}
		#endregion
		#endregion
	}
}
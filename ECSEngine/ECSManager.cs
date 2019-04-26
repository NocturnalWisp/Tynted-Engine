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
		public static void AddSystem<T>() where T : System
		{
			if (systems.Find(o => o.GetType() == typeof(T)) == null)
			{
				systems.Add((T)Activator.CreateInstance(typeof(T)));
			}
		}

		public static void RemoveSystem<T>()
		{
			systems.RemoveAll(o => o.GetType() == typeof(T));
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
				components.Remove(components.Find(o => o.GetType() == typeof(T)));
			}
		}

		#endregion
		#region Entities Utility Functions
		public static List<EntityComponent> GetComponentEntityActiveList<T>() where T : IComponent
		{
			return (List<EntityComponent>)components.Find(o => o.GetType() == typeof(T)).entityComponents.Where(o => o.component.Enabled);
		}

		public static List<EntityComponent> GetComponentEntityList<T>() where T : IComponent
		{
			return components.Find(o => o.GetType() == typeof(T)).entityComponents;
		}

		public static void CreateEntity(string name, string tag = "Default")
		{
			entities.Add(new EntityData(nextEntity, name, tag));
			nextEntity++;
		}

		public static void AddEntityComponent<T>(string entityName, IComponent component) where T : IComponent
		{
			//make sure component exists
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//Make sure data exists
				if (!eData.Equals(default(EntityData)))
				{
					//make sure entity does not already exist
					if (!GetComponentEntityList<T>().Exists(o => o.entityID == eData.EntityID))
					{
						components.Find(o => o.GetType() == typeof(T)).entityComponents.Add(new EntityComponent(eData.EntityID, component));
					}
				}
			}
		}

		public static void AddEntityComponent<T>(int entityID, IComponent component) where T : IComponent
		{
			//make sure it has component
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				//make sure entity does not already exist
				if (!GetComponentEntityList<T>().Exists(o => o.entityID == entityID))
				{
					components.Find(o => o.GetType() == typeof(T)).entityComponents.Add(new EntityComponent(entityID, component));
				}
			}
		}

		//TODO: Fix generic property not existant
		public static void RegisterEntityComponents(List<EntityComponent> entityComponents)
		{
			foreach (EntityComponent entityComponent in entityComponents)
			{
				entityComponent.component.GetType().GetMethod("AddEntityComponent").MakeGenericMethod()
					.Invoke(null, new object[] { entityComponent.entityID, entityComponent.component });
			}
		}

		public static void RemoveEntityComponent<T>(string entityName, IComponent component) where T : IComponent
		{
			//make sure component exists
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//Make sure data exists
				if (!eData.Equals(default(EntityData)))
				{
					//make sure entity component already exists
					if (GetComponentEntityList<T>().Exists(o => o.entityID == eData.EntityID))
					{
						components.Find(o => o.GetType() == typeof(T)).entityComponents
							.RemoveAll(o => o.entityID == eData.EntityID && o.component == component);
					}
				}
			}
		}

		public static void RemoveEntityComponent<T>(int entityID, IComponent component) where T : IComponent
		{
			//make sure it has component
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				//make sure entity component already exist
				if (GetComponentEntityList<T>().Exists(o => o.entityID == entityID))
				{
					components.Find(o => o.GetType() == typeof(T)).entityComponents
						.RemoveAll(o => o.entityID == entityID && o.component == component);
				}
			}
		}

		public static T GetEntityComponent<T>(string entityName) where T : IComponent
		{
			//make component exists
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					return (T)components.Find(o => o.GetType() == typeof(T)).entityComponents.Where(o => o.entityID == eData.EntityID);
				}
			}

			return default(T);
		}

		public static T GetEntityComponent<T>(int entityID) where T : IComponent
		{
			//make component exists
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				return (T)components.Find(o => o.GetType() == typeof(T)).entityComponents.Where(o => o.entityID == entityID);
			}

			return default(T);
		}

		public static void SetEntityComponent<T>(string entityName, IComponent component)
		{
			//make component exists
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					SetEntityComponent<T>(eData.EntityID, component);
				}
			}
		}

		public static void SetEntityComponent<T>(int entityID, IComponent component)
		{
			//make component exists
			if (components.Where(o => o.GetType() == typeof(T)).Count() == 1)
			{
				EntityComponent ec = components.Find(o => o.GetType() == typeof(T)).entityComponents.Find(o => o.entityID == entityID);
				ec.component = component;
				components.Find(o => o.GetType() == typeof(T)).entityComponents
					[components.Find(o => o.GetType() == typeof(T)).entityComponents.FindIndex(o => o.entityID == entityID)] = ec;
			}
		}

		public static void RemoveAllEntityComponents(string entityName)
		{
			for (int i = 0; i < components.Count; i++)
			{
				for (int j = components[i].entityComponents.Count - 1; j >= 0; j--)
				{
					EntityData eData = entities.Find(o => o.Name == entityName && o.EntityID == components[i].entityComponents[j].entityID);

					//make sure entity exists
					if (!eData.Equals(default(EntityData)))
					{
						components[i].entityComponents[j].component.GetType().GetMethod("RemoveEntityComponent").MakeGenericMethod()
							.Invoke(null, new object[] { components[i].entityComponents[j].entityID, components[i].entityComponents[j].component });
					}
				}
			}
		}

		public static void RemoveAllEntityComponents(int entityID)
		{
			for (int i = 0; i < components.Count; i++)
			{
				for (int j = components[i].entityComponents.Count - 1; j >= 0; j--)
				{
					if (entityID == components[i].entityComponents[j].entityID)
					{
						components[i].entityComponents[j].component.GetType().GetMethod("RemoveEntityComponent").MakeGenericMethod()
							.Invoke(null, new object[] { components[i].entityComponents[j].entityID, components[i].entityComponents[j].component });
					}
				}
			}
		}

		public static List<IComponent> GetEntityComponents(string entityName)
		{
			List<IComponent> componentList = new List<IComponent>();

			for (int i = 0; i < components.Count; i++)
			{
				for (int j = components[i].entityComponents.Count - 1; j >= 0; j--)
				{
					EntityData eData = entities.Find(o => o.Name == entityName && o.EntityID == components[i].entityComponents[j].entityID);

					//make sure entity exists
					if (!eData.Equals(default(EntityData)))
					{
						componentList.Add(components[i].entityComponents[j].component);
					}
				}
			}

			return componentList;
		}

		public static List<IComponent> GetEntityComponents(int entityID)
		{
			List<IComponent> componentList = new List<IComponent>();

			for (int i = 0; i < components.Count; i++)
			{
				for (int j = components[i].entityComponents.Count - 1; j >= 0; j--)
				{
					if (components[i].entityComponents[j].entityID == entityID)
					{
						componentList.Add(components[i].entityComponents[j].component);
					}
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
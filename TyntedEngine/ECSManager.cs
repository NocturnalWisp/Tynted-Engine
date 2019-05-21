using Tynted.Components;
using Tynted.Events;
using Tynted.SFML.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tynted
{
	public class ECSManager
	{
		internal static List<EntityData> entities = new List<EntityData>();
        public static List<EntityData> GetEntityData { get => entities; }
		static int nextEntity = 0;

		static List<System> systems = new List<System>();

		static List<Component> components = new List<Component>();

		static Dictionary<string, TyntedEvent> events = new Dictionary<string, TyntedEvent>();
		static Dictionary<string, TyntedEvent<object>> events1 = new Dictionary<string, TyntedEvent<object>>();
		static Dictionary<string, TyntedEvent<object, object>> events2 = new Dictionary<string, TyntedEvent<object, object>>();

		/// <summary>
		/// Initializer for each system.
		/// </summary>
		internal static void Initialize()
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

		/// <summary>
		/// Update method applied to each system.
		/// </summary>
		/// <param name="gameTime">Game Time with grabbable fields.</param>
		internal static void Update(GameTime gameTime)
		{
			foreach (System system in systems)
			{
				system.Update(gameTime);
			}
		}

		/// <summary>
		/// Draw method applied to each system.
		/// </summary>
		/// <param name="window">The window to draw within.</param>
		internal static void Draw(RenderWindow window)
		{
			foreach (System system in systems)
			{
				system.Draw(window);
			}
		}

		#region System and Component Utility Functions
		/// <summary>
		/// Add a system to the system types list.
		/// </summary>
		/// <param name="systemType">The type of system to add.</param>
		public static void AddSystem(Type systemType)
		{
			if (typeof(System).IsAssignableFrom(systemType))
			{
				if (systems.Find(o => o.GetType() == systemType) == null)
				{
					bool typeAttributeUsed = false;
					bool tagAttributeUsed = false;
					bool sceneAttributeUsed = false;

					System system = (System)Activator.CreateInstance(systemType);

					//Check if has require components attribute
					foreach (object attribute in systemType.GetCustomAttributes(false))
					{
						if (attribute.GetType() == typeof(GetComponents))
						{
							system.types = ((GetComponents)attribute).Types;
							typeAttributeUsed = true;
						}

						if (attribute.GetType() == typeof(RequireTags))
						{
							system.tagSpecific = true;
							system.tags = ((RequireTags)attribute).tag;
							tagAttributeUsed = true;
						}

						if (attribute.GetType() == typeof(RequireScenes))
						{
							system.sceneSpecific = true;
							system.scenes = ((RequireScenes)attribute).scenes;
							sceneAttributeUsed = true;
						}
					}

					if (!typeAttributeUsed) system.types = new Type[0];
					if (!tagAttributeUsed) system.tags = new string[0];
					if (!sceneAttributeUsed) system.scenes = new string[0];

					systems.Add(system);

					Console.WriteLine("Added system: " + systemType);
				}
			}
		}

		/// <summary>
		/// Remove a system from the system types list.
		/// </summary>
		/// <param name="systemType">The type of system to remove.</param>
		internal static void RemoveSystem(Type systemType)
		{
			if (typeof(System).IsAssignableFrom(systemType))
				systems.RemoveAll(o => o.GetType() == systemType);
		}

		/// <summary>
		/// Gets all of the components in the components list.
		/// </summary>
		/// <returns>List of components.</returns>
		internal static List<Component> GetComponents()
		{
			return components.ToList();
		}

		/// <summary>
		/// Adds a component to the type of components list.
		/// </summary>
		/// <param name="componentType">The component type to add.</param>
		public static void AddComponent(Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				//Make sure component doesn't already exist
				if (components.Find(o => o.componentType == componentType) == null)
				{
					components.Add(new Component(componentType));
					Console.WriteLine("Added component: " + componentType);
				}
			}
		}

		/// <summary>
		/// Removes a component from the component types list.
		/// </summary>
		/// <param name="componentType">The component type to remove.</param>
		internal static void RemoveComponent(Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				if (components.Find(o => o.componentType == componentType) != null)
				{
					components.Remove(components.Find(o => o.componentType == componentType));
				}
			}
		}

		#endregion
		#region Entities Utility Functions
		/// <summary>
		/// Gets all of the active entity components with a certain component type.
		/// </summary>
		/// <param name="componentType">The component type to search for.</param>
		/// <returns>List of objects found.</returns>
		public static List<EntityComponent> GetComponentEntityActiveList(Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				if (components.Find(o => o.componentType == componentType) != null)
				{
					return components.Find(o => o.componentType == componentType).entityComponents.Where(o => o.component.Enabled && SceneManager.SceneExists(entities.Find(x => x.EntityID == o.entityID).SceneName)).ToList();
				}
			}

			return new List<EntityComponent>();
		}

		/// <summary>
		/// Gets all of the entity components with a component type.
		/// </summary>
		/// <param name="componentType">The component type to search for.</param>
		/// <returns>List of objects found.</returns>
		public static List<EntityComponent> GetComponentEntityList(Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				if (components.Find(o => o.componentType == componentType) != null)
				{
					return components.Find(o => o.componentType == componentType).entityComponents;
				}
			}

			return new List<EntityComponent>();
		}

		/// <summary>
		/// Creates an entity with entityData.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <param name="tag">Optional tag of the entity.</param>
		public static void CreateEntity(string name, string tag = "Default", string scene = "")
		{
			EntityData data = new EntityData(nextEntity, name, scene, tag);
			entities.Add(data);
			nextEntity++;
		}

		/// <summary>
		/// Removes an entity completely from existance.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		public static void DeleteEntity(string name)
		{
			foreach (System system in systems)
			{
				system.RemoveEntity(entities.Find(o => o.Name == name).EntityID);
			}

			//Remove all components from the entity
			RemoveAllEntityComponents(name);

			//Remove the entity;
			entities.Remove(entities.Find(o => o.Name == name));
		}

		/// <summary>
		/// Adds a component to the list on the specified entity.
		/// </summary>
		/// <param name="entityName">The name of the entity to add the component to.</param>
		/// <param name="component">The component to add to the entity.</param>
		public static void AddEntityComponent(string entityName, IComponent component)
		{
			//make sure component exists
			if (components.Find(o => o.componentType == component.GetType()) != null)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//Make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					AddEntityComponent(eData.EntityID, component);
				}
			}
		}

		/// <summary>
		/// Adds a component to the list on the specified entity.
		/// </summary>
		/// <param name="entityID">The ID of the entity to add the component to.</param>
		/// <param name="component">The component to add to the entity.</param>
		internal static void AddEntityComponent(int entityID, IComponent component)
        {
            //make sure it has component
            if (components.Find(o => o.componentType == component.GetType()) != null)
			{
				//Make sure entity exists
				if (!entities.Find(o => o.EntityID == entityID).Equals(default(EntityData)))
				{
					//make sure entity does not already have this component
					if (!GetComponentEntityList(component.GetType()).Exists(o => o.entityID == entityID))
					{
						components.Find(o => o.componentType == component.GetType()).entityComponents.Add(new EntityComponent(entityID, component));
                        
						foreach (System system in systems)
						{
							system.AddEntityComponents(entityID, GetEntityComponents(entityID));
						}
					}
				}
			}
		}

		/// <summary>
		/// Adds a list of entity components in batch.
		/// </summary>
		/// <param name="entityComponents">The entity component list to add.</param>
		public static void RegisterEntityComponents(List<EntityComponentIdentifier> entityComponents)
		{
			foreach (EntityComponentIdentifier entityComponent in entityComponents)
			{
				AddEntityComponent(entityComponent.name, entityComponent.component);
			}
		}

		/// <summary>
		/// Adds a list of entity components in batch.
		/// </summary>
		/// <param name="entityComponents">The list of entity components to add.</param>
		internal static void RegisterEntityComponents(List<EntityComponent> entityComponents)
		{
			foreach (EntityComponent entityComponent in entityComponents)
			{
				AddEntityComponent(entityComponent.entityID, entityComponent.component);
			}
		}

		/// <summary>
		/// Removes a component from an entity.
		/// </summary>
		/// <param name="entityName">The name of the entity to remove a component from.</param>
		/// <param name="componentType">The type of component to remove.</param>
		public static void RemoveEntityComponent(string entityName, Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				//make sure component exists
				if (components.Find(o => o.componentType == componentType) != null)
				{
					EntityData eData = entities.Find(o => o.Name == entityName);

					//Make sure data exists
					if (!eData.Equals(default(EntityData)))
					{
						RemoveEntityComponent(eData.EntityID, componentType);
					}
				}
			}
		}

		/// <summary>
		/// Removes a component from a specified entity.
		/// </summary>
		/// <param name="entityID">The ID of the entity.</param>
		/// <param name="componentType">The type of component to remove.</param>
		internal static void RemoveEntityComponent(int entityID, Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				//make sure it has component
				if (components.Find(o => o.componentType == componentType) != null)
				{
					//Make sure entity exists
					if (!entities.Find(o => o.EntityID == entityID).Equals(default(EntityData)))
					{
						//make sure entity component already exist
						if (GetComponentEntityList(componentType).Exists(o => o.entityID == entityID))
						{
							components.Find(o => o.componentType == componentType).entityComponents
								.RemoveAll(o => o.entityID == entityID && o.component.GetType() == componentType);

							foreach (System system in systems)
							{
								system.RemoveEntityComponent(entityID, componentType);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets a component instance from an entity based on a component type.
		/// </summary>
		/// <param name="entityName">The name of the entity.</param>
		/// <param name="componentType">The type of component to grab.</param>
		/// <returns>The component if found, otherwise null.</returns>
		public static IComponent GetEntityComponent(string entityName, Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				//make sure component exists
				if (components.Find(o => o.componentType == componentType) != null)
				{
					EntityData eData = entities.Find(o => o.Name == entityName);

					//make sure entity exists
					if (!eData.Equals(default(EntityData)))
					{
						return components.Find(o => o.componentType == componentType).entityComponents.Find(o => o.entityID == eData.EntityID).component;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets a component instance from an entity based on a component type.
		/// </summary>
		/// <param name="entityID">ID of the entity.</param>
		/// <param name="componentType">The type of component to get.</param>
		/// <returns>The component if found, otherwise null.</returns>
		internal static IComponent GetEntityComponent(int entityID, Type componentType)
		{
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				//make sure component exists
				if (components.Find(o => o.componentType == componentType) != null)
				{
					//Make sure entity exists
					if (!entities.Find(o => o.EntityID == entityID).Equals(default(EntityData)))
					{
						return components.Find(o => o.componentType == componentType).entityComponents.Find(o => o.entityID == entityID).component;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Sets the component to a new component value.
		/// </summary>
		/// <param name="entityName">The name of the entity.</param>
		/// <param name="component">The component to become.</param>
		public static void SetEntityComponent(string entityName, IComponent component)
		{
			//make sure component exists
			if (components.Find(o => o.componentType == component.GetType()) != null)
			{
				EntityData eData = entities.Find(o => o.Name == entityName);

				//make sure entity exists
				if (!eData.Equals(default(EntityData)))
				{
					SetEntityComponent(eData.EntityID, component);
				}
			}
		}

		/// <summary>
		/// Sets the component to a new component value.
		/// </summary>
		/// <param name="entityID">The ID of the entity.</param>
		/// <param name="component">The component to become.</param>
		internal static void SetEntityComponent(int entityID, IComponent component)
		{
			//make sure component exists
			if (components.Find(o => o.componentType == component.GetType()) != null)
			{
				//Make sure entity exists
				if (!entities.Find(o => o.EntityID == entityID).Equals(default(EntityData)))
				{
					EntityComponent ec = components.Find(o => o.componentType == component.GetType()).entityComponents.Find(o => o.entityID == entityID);

					if (ec != default)
					{
						ec.component = component;
						components.Find(o => o.componentType == component.GetType()).entityComponents
							[components.Find(o => o.componentType == component.GetType()).entityComponents.FindIndex(o => o.entityID == entityID)] = ec;

						foreach (System system in systems)
						{
							system.SetEntityComponent(entityID, component);
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes all components from a specified entity.
		/// </summary>
		/// <param name="entityName">The name of the entity.</param>
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
						RemoveEntityComponent(components[i].entityComponents[j].entityID, components[i].entityComponents[j].component.GetType());
					}
				}
			}
		}

		/// <summary>
		/// Removes all components from a specified entity.
		/// </summary>
		/// <param name="entityID">The ID of the entity.</param>
		internal static void RemoveAllEntityComponents(int entityID)
		{
			for (int i = 0; i < components.Count; i++)
			{
				for (int j = components[i].entityComponents.Count - 1; j >= 0; j--)
				{
					if (entityID == components[i].entityComponents[j].entityID)
					{
						RemoveEntityComponent(components[i].entityComponents[j].entityID, components[i].entityComponents[j].component.GetType());
					}
				}
			}
		}

		/// <summary>
		/// Gets a list of entity components from a specific entity.
		/// </summary>
		/// <param name="entityName">The name of the entity that has the components.</param>
		/// <returns>The components if found, otherwise an empty list.</returns>
		public static List<IComponent> GetEntityComponents(string entityName)
		{
			List<IComponent> componentList = new List<IComponent>();

			for (int i = 0; i < components.Count; i++)
			{
				for (int j = components[i].entityComponents.Count - 1; j >= 0; j--)
				{
					EntityData eData = entities.Find(o => o.Name == entityName && o.EntityID == components[i].entityComponents[j].entityID && SceneManager.SceneExists(o.SceneName));

					//make sure entity exists
					if (!eData.Equals(default(EntityData)))
					{
						componentList.Add(components[i].entityComponents[j].component);
					}
				}
			}

			return componentList;
		}

		/// <summary>
		/// Gets a list of entity components from a specific entity.
		/// </summary>
		/// <param name="entityID">The ID of the entity that has the components.</param>
		/// <returns>The components if found, otherwise an empty list.</returns>
		internal static List<IComponent> GetEntityComponents(int entityID)
		{
			List<IComponent> componentList = new List<IComponent>();

			for (int i = 0; i < components.Count; i++)
			{
				for (int j = components[i].entityComponents.Count - 1; j >= 0; j--)
				{
					//Make sure entity exists
					if (!entities.Find(o => o.EntityID == entityID).Equals(default(EntityData)))
					{
						if (components[i].entityComponents[j].entityID == entityID)
						{
							componentList.Add(components[i].entityComponents[j].component);
						}
					}
				}
			}

			return componentList;
		}

        #region Scenes
        /// <summary>
        /// Returns a list of all the entities in a scene
        /// </summary>
        internal static List<Entity> GetSceneEntities(string sceneName)
        {
            List<Entity> entityList = new List<Entity>();

            foreach (Component component in components)
            {
                foreach (EntityComponent entityComponent in component.entityComponents)
                {
                    EntityData eData = entities.Find(o => o.EntityID == entityComponent.entityID && SceneManager.SceneExists(o.SceneName) && o.SceneName.Equals(sceneName));

                    //make sure entity exists
                    if (!eData.Equals(default))
                    {
                        //check if it exists in the new list yet
                        if (!entityList.Exists(o => o.entityID == entityComponent.entityID))
                        {
                            Entity entity = new Entity(entityComponent.entityID);
                            entityList.Add(entity);
                            entity.components.Add(entityComponent.component);
                        }
                        else
                        {
                            entityList.Find(o => o.entityID == entityComponent.entityID).components.Add(entityComponent.component);
                        }
                    }
                }
            }

            return entityList;
        }

        /// <summary>
        /// Sets all the components that are within a scene.
        /// </summary>
        /// <param name="sceneEntities">The list of entities the scene has.</param>
        internal static void SetSceneEntities(List<Entity> sceneEntities, string sceneName)
        {
            if (!SceneManager.SceneExists(sceneName))
                return;

            foreach (Entity entity in sceneEntities)
            {
                for (int componentTypeIndex = 0; componentTypeIndex < components.Count; componentTypeIndex++)
                {
                    List<EntityComponent> entityComponents = components[componentTypeIndex].entityComponents.Where(o => o.entityID == entity.entityID).ToList();
                    for (int entityComponentIndex = 0; entityComponentIndex < entityComponents.Count; entityComponentIndex++)
                    {
                        //TODO: May be broken if objects were removed or added to a scene...
                        if (entityComponents[entityComponentIndex].entityID == entity.entityID)
                        {
                            EntityComponent ec = entityComponents[entityComponentIndex];
                            ec.component = entity.components.Find(o => o.GetType().Equals(entityComponents[entityComponentIndex].component));
                            entityComponents[entityComponentIndex] = ec;
                        }
                    }
                    components[componentTypeIndex].entityComponents = entityComponents;
                }
            }
        }
        #endregion

        #endregion
        #region Event Functions
        #region Create Events
        /// <summary>
        /// Creates a new event with 0 parameters.
        /// </summary>
        /// <param name="name">The name identifier of the event.</param>
        /// <returns>The created event, or null if already created.</returns>
        public static TyntedEvent CreateEvent(string name)
		{
			TyntedEvent ev = null;

			if (!events.ContainsKey(name))
			{
				ev = new TyntedEvent();
				events[name] = ev;
			}

			return ev;
		}

		/// <summary>
		/// Creates a new event with 1 parameter.
		/// </summary>
		/// <param name="name">The name identifier of the event.</param>
		/// <returns>The created event, or null if already created.</returns>
		public static TyntedEvent<object> CreateEvent1Arg(string name)
		{
			TyntedEvent<object> ev = null;

			if (!events1.ContainsKey(name))
			{
				ev = new TyntedEvent<object>();
				events1[name] = ev;
			}

			return ev;
		}

		/// <summary>
		/// Creates a new event with 2 parameters.
		/// </summary>
		/// <param name="name">The name identifier of the event.</param>
		/// <returns>The created event, or null if already created.</returns>
		public static TyntedEvent<object, object> CreateEvent2Arg(string name)
		{
			TyntedEvent<object, object> ev = null;

			if (!events2.ContainsKey(name))
			{
				ev = new TyntedEvent<object, object>();
				events2[name] = ev;
			}

			return ev;
		}
		#endregion

		#region Subscribe Events
		/// <summary>
		/// Subscribes an engine action to an event.
		/// </summary>
		/// <param name="name">The name identifier of the event.</param>
		/// <param name="action">The action that occurs when the event is invoked.</param>
		public static void SubscribeEvent(string name, EngineAction action)
		{
			if (events.ContainsKey(name))
			{
				events[name].AddListener(action);
			}
		}

		/// <summary>
		/// Subscribes an engine action to an event.
		/// </summary>
		/// <param name="name">The name identifier of the event.</param>
		/// <param name="action">The action that occurs when the event is invoked.</param>
		public static void SubscribeEvent(string name, EngineAction<object> action)
		{
			if (events1.ContainsKey(name))
			{
				events1[name].AddListener(action);
			}
		}

		/// <summary>
		/// Subscribes an engine action to an event.
		/// </summary>
		/// <param name="name">The name identifier of the event.</param>
		/// <param name="action">The action that occurs when the event is invoked.</param>
		public static void SubscribeEvent(string name, EngineAction<object, object> action)
		{
			if (events2.ContainsKey(name))
			{
				events2[name].AddListener(action);
			}
		}
		#endregion

		#region Unsubscribe Events
		/// <summary>
		/// Unsubscribes an action from the event.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="ev">The action.</param>
		public static void UnSubscribeEvent(string name, EngineAction ev)
		{
			if (events.ContainsKey(name))
			{
				events[name].RemoveListener(ev);
			}
		}

		/// <summary>
		/// Unsubscribes an action from the event.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="ev">The action.</param>
		public static void UnSubscribeEvent(string name, EngineAction<object> ev)
		{
			if (events1.ContainsKey(name))
			{
				events1[name].RemoveListener(ev);
			}
		}

		/// <summary>
		/// Unsubscribes an action from the event.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		/// <param name="ev">The action.</param>
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
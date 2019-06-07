using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tynted;
using Tynted.SFML.Graphics;

namespace Tynted
{
	public abstract class System
	{
		//These both allow for the system to control a list of entities 
		//that can be dynamically altered based on the ECSManager list, 
		//and when things are changed within the system.
		internal Type[] types;
		private List<Entity> entities = new List<Entity>();

		//If the attribute RequireTag is used, then use these.
		internal bool tagSpecific = false;
		internal string[] tags;
		internal bool sceneSpecific = false;
		internal string[] scenes;

		public virtual void Initialize() { }

		/// <summary>
		/// Use this to create all the events that a system will utilize.
		/// </summary>
		public virtual void CreateEvents() { }

		/// <summary>
		/// Use this to subscribe to events that have been created.
		/// </summary>
		public virtual void SubscribeEvents() { }

		public virtual void Update(GameTime gameTime)
		{
			foreach (Entity entity in entities)
			{
				for (int i = 0; i < entity.components.Count; i++)
				{
					ECSManager.SetEntityComponent(entity.entityID, entity.components[i]);
				}
			}
		}

		public virtual void Draw(RenderWindow window) { }

		//TODO: Make sure tag is checked if required.
		/// <summary>
		/// For the engine to add new entity components.
		/// </summary>
		internal void AddEntityComponents(int entityID, List<IComponent> components)
        {
            //Make sure all types are matched
            if (types.All(o => components.Exists(x => o.Equals(x.GetType()))))
			{
				EntityData data = ECSManager.entities.Find(o => o.EntityID == entityID);

                //check for tag if that attribute has been added.
                if (((tagSpecific && tags.Contains(data.Tag)) || !tagSpecific) &&
					((sceneSpecific && scenes.Contains(data.SceneName)) || !sceneSpecific))
				{
					//Check if entity exists and components are empty
					if (entities.Exists(o => o.entityID == entityID && o.components.Count <= 0))
					{
						entities[entities.IndexOf(entities.Find(o => o.entityID == entityID))].components = components.FindAll(o => types.Contains(o.GetType()));
					}
					else
					{
						AddEntity(entityID);
						entities[entities.IndexOf(entities.Find(o => o.entityID == entityID))].components = components.FindAll(o => types.Contains(o.GetType()));
					}
				}
			}
		}

		/// <summary>
		/// For the engine to remove entity components.
		/// </summary>
		internal void RemoveEntityComponent(int entityID, Type componentType)
		{
			//Make sure component is of type IComponent
			if (typeof(IComponent).IsAssignableFrom(componentType))
			{
				//Make sure entity and component both exists
				if (entities.Exists(o => o.entityID == entityID && o.components.Exists(x => x.GetType() == componentType)))
				{
					entities[entities.IndexOf(entities.Find(o => o.entityID == entityID))].components.RemoveAll(o => o.GetType() == componentType);
				}
			}
		}
		
		/// <summary>
		/// For the engine to remove entity components.
		/// </summary>
		internal void SetEntityComponent(int entityID, IComponent component)
		{
			//Make sure entity and component exist
			if (entities.Exists(o => o.entityID == entityID && o.components.Exists(x => x.GetType() == component.GetType())))
			{
				int entityIndex = entities.IndexOf(entities.Find(o => o.entityID == entityID));
				entities[entityIndex].components[entities[entityIndex].components.FindIndex(o => o.GetType() == component.GetType())] = component;
			}
		}

		/// <summary>
		/// For the engine to add a new entity.
		/// </summary>
		private void AddEntity(int entityID)
		{
			//Make sure entity doesn't exist
			if (!entities.Exists(o => o.entityID == entityID))
			{
				entities.Add(new Entity(entityID));
			}
		}

		/// <summary>
		/// For the engine to remove entities.
		/// </summary>
		internal void RemoveEntity(int entityID)
		{
			entities.Find(o => o.entityID == entityID).components = null;

			entities.RemoveAll(o => o.entityID == entityID);
		}

		/// <summary>
		/// Gets the entity list that are currently in a scene.
		/// </summary>
		/// <returns>The entity list.</returns>
		protected List<Entity> GetEntities()
		{
			return entities.Where(o =>
                SceneManager.SceneExists(ECSManager.entities.Find(x => x.EntityID == o.entityID).SceneName) && 
                !SceneManager.GetSceneByName(ECSManager.entities.Find(x => x.EntityID == o.entityID).SceneName).Paused
            ).ToList();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.SFML.Graphics;

namespace ECSEngine
{
	public class System
	{
		internal Type[] types;

		private List<Entity> entities = new List<Entity>();

		public System() { }

		public virtual void Initialize()
		{
			foreach(Entity entity in entities)
			{
				for (int i = 0; i < entity.components.Count; i++)
				{
					ECSManager.SetEntityComponent(entity.entityID, entity.components[i]);
				}
			}
		}

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

		public virtual void Draw(RenderWindow window)
		{
			foreach (Entity entity in entities)
			{
				for (int i = 0; i < entity.components.Count; i++)
				{
					ECSManager.SetEntityComponent(entity.entityID, entity.components[i]);
				}
			}
		}

		/// <summary>
		/// For the engine to add new entity components.
		/// </summary>
		internal void AddEntityComponent(int entityID, IComponent component)
		{
			//Make sure type is allowed
			if (types.Contains(component.GetType()))
			{
				//Make sure entity exists and component does not
				if (entities.Exists(o => o.entityID == entityID && !o.components.Exists(x => x.GetType() == component.GetType())))
				{
					entities[entities.IndexOf(entities.Find(o => o.entityID == entityID))].components.Add(component);
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
		
		//TODO: Make sure entity contains all types.
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
		internal void AddEntity(int entityID)
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
		/// Gets the entity list.
		/// </summary>
		/// <returns>The entity list.</returns>
		protected List<Entity> GetEntities()
		{
			return entities;
		}
	}
}

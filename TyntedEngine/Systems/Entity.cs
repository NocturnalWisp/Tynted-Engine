using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tynted.Components;
using Tynted.SFML.Graphics;
using Transform = Tynted.Components.Transform;

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
		internal IComponent GetComponent(Type componentType)
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

        #region Entity Templates
        //IMPORTANT!!!!! Fix any discrepancies within Scene to make sure
        //               the template entities in scene json files are 
        //               updated properly.
        public static void CreateEmpty(string name, string tag = "Default", string scene = "")
        {
            ECSManager.CreateEntity(name, tag, scene);
        }

        public static void CreateTransform(string name, string tag = "Default", string scene = "")
        {
            ECSManager.CreateEntity(name, tag, scene);
            ECSManager.AddEntityComponent(name, new Transform(new Box2DNet.Common.Vec2(0, 0)));
        }

        public static void CreateSprite(string name, Texture texture, string tag = "Default", string scene = "")
        {
            ECSManager.CreateEntity(name, tag, scene);
            ECSManager.AddEntityComponent(name, new Transform(new Box2DNet.Common.Vec2(0, 0)));
            ECSManager.AddEntityComponent(name, new SpriteRenderee(texture));
        }
        #endregion

        #region Cloning
        public static void CloneEntity(string clonable, string entityName, string clonableTag, string clonableSceneName)
        {
            EntityData data = ECSManager.entities.Find(o => o.Name == clonable && o.Tag == clonableTag && o.SceneName == clonableSceneName);
            
            int? i = ECSManager.CreateEntity(entityName, clonableTag, clonableSceneName);

            if (i != null)
            {
                foreach (IComponent component in ECSManager.GetEntityComponents(clonable))
                {
                    ECSManager.AddEntityComponent((int)i, component.Clone);
                }
            }
        }

        internal static void CloneEntity(int entityID)
        {
            EntityData data = ECSManager.entities.Find(o => o.EntityID == entityID);

            int? i = ECSManager.CreateEntity(data.Name + " (Clone)", data.Tag, data.SceneName);

            if (i != null)
            {
                foreach (IComponent component in ECSManager.GetEntityComponents(data.Name))
                {
                    ECSManager.AddEntityComponent(entityID, component.Clone);
                }
            }
        }
        #endregion
    }
}

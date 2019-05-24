using Tynted.SFML.Graphics;
using Tynted.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Diagnostics;

namespace Tynted
{
    public class Scene
    {
        private string sceneName;
        public string SceneName { get => sceneName; }

        internal bool paused;
        public bool Paused { get => paused; }

        internal List<Entity> initalEntities;
        internal List<Entity> entities;

		public Scene(string scene, bool isPath, Dictionary<string, object> variables)
		{
            if (!isPath)
            {
                sceneName = scene;
            }
            else
            {
                #region Setup Scene From File
                //Do all the json work
                try
                {
                    JObject sceneJson = JsonManager.LoadFile(scene);

                    double version = sceneJson.Value<double>("version");

                    //Work with various versions for backwards compatability.
                    if (version >= 0.1)
                    {
                        sceneName = sceneJson.Value<string>("name");

                        JArray entities = sceneJson.Value<JArray>("entities");

                        foreach (JObject entity in entities)
                        {
                            string entityName = entity.Value<string>("name");
                            string tag = entity.Value<string>("tag");

                            //Clone
                            if (entity.GetValue("clone") != null)
                            {
                                Entity.CloneEntity(entity.Value<string>("clone"), entityName, tag, sceneName);
                            }
                            else if (entity.GetValue("template") != null)
                            {
                                //TODO: Make it generalized so any templates will work off the bat.
                                JObject templateObject = entity.Value<JObject>("template");

                                string templateName = templateObject.Value<string>("name");
                                object[] args = templateObject.Value<JArray>("args").ToObject<object[]>();

                                //IMPORTANT!!!! Update here if any templates change in the entity class.
                                switch (templateName.ToLower())
                                {
                                    case "empty":
                                        {
                                            Entity.CreateEmpty(entityName, tag, sceneName);
                                            break;
                                        }
                                    case "transform":
                                        {
                                            Entity.CreateTransform(entityName, tag, sceneName);
                                            break;
                                        }
                                    case "sprite":
                                        {
                                            if (args.Length > 0)
                                            {
                                                string textureArgument = ((string)args[0]).Substring(1, ((string)args[0]).Length - 1);

                                                if (variables.ContainsKey(textureArgument))
                                                {
                                                    if (variables[textureArgument].GetType().Equals(typeof(Texture)))
                                                    {
                                                        Entity.CreateSprite(entityName, tag, sceneName, (Texture)variables[textureArgument]);
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("The variable " + variables[textureArgument] + " is not of type Texture.");
                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception("The variable " + textureArgument + " does not exist.");
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("The arguments list is too short for creating a new sprite with template \"Sprite.\"");
                                            }
                                            break;
                                        }
                                }
                            }
                            else if (entity.GetValue("components") != null)
                            {
                                int? entityID = ECSManager.CreateEntity(entityName, tag, sceneName);

                                if (entityID != null)
                                {
                                    foreach (var jsonComponent in entity.Value<JObject>("components"))
                                    {
                                        try
                                        {
                                            Type componentType = Type.GetType("Tynted.Components." + jsonComponent.Key);

                                            object[] args = jsonComponent.Value.Value<JArray>().ToObject<object[]>();

                                            for (int i = 0; i < args.Length; i++)
                                            {
                                                if (args[i].GetType().Equals(typeof(string)))
                                                {
                                                    if (((string)args[i]).StartsWith("@"))
                                                    {
                                                        string variableName = ((string)args[i]).Substring(1, ((string)args[i]).Length - 1);
                                                        args[i] = variables[variableName];
                                                    }
                                                }
                                            }

                                            List<Type> types = new List<Type>();

                                            foreach (object obj in args)
                                            {
                                                types.Add(obj.GetType());
                                            }

                                            ConstructorInfo ctor = componentType.GetConstructor(types.ToArray());
                                            
                                            if (ctor != null)
                                            {
                                                IComponent newComponent = (IComponent)ctor.Invoke(args);

                                                ECSManager.AddEntityComponent((int)entityID, newComponent);
                                            }
                                            else
                                            {
                                                throw new Exception("Could not create a " + componentType + " component with the arguments " + args.ToString() + ".");
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("Could not create entity with component " + jsonComponent.Key + ". Error: " + e.ToString());
                                            ECSManager.DeleteEntity(entityName, tag, sceneName);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("An entity by that name already exists, could not create a new one.");
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not extract the json for scene " + scene + " because " + e);
                }
                #endregion
            }
        }

		internal virtual void Initialize()
		{
            initalEntities = ECSManager.GetSceneEntities(SceneName);
            entities = initalEntities;
		}

		internal virtual void Update(GameTime gameTime)
		{
            entities = ECSManager.GetSceneEntities(SceneName);
		}

        internal virtual void Draw(RenderWindow window) { }

		internal virtual void OnClosed()
		{
            initalEntities.Clear();
            entities.Clear();
		}

        #region EntityHandling
        public int EntityCount { get => initalEntities.Count; }
        public List<Entity> Entities { get => entities; }
        #endregion
    }
}

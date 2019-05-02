using ECSEngine.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	public static class SceneManager
	{
		private static List<Scene> currentScenes = new List<Scene>();

		public static void LoadScene(Scene newScene)
		{
			if (SceneExists(newScene.SceneName))
				return;

			currentScenes.RemoveAll(o => o.SceneName != "");
			currentScenes.Add(newScene);
			newScene.Initialize();
		}

		public static void LoadSceneOnTop(Scene newScene)
		{
			if (SceneExists(newScene.SceneName))
				return;

			currentScenes.Add(newScene);
		}

		public static void UnloadScene(string sceneName)
		{
			if (sceneName == "")
				return;

			currentScenes.RemoveAll(o => o.SceneName == sceneName);
		}

		public static void UnloadScene(Scene scene)
		{
			currentScenes.Remove(scene);
		}

		public static void UnloadAllScenes(bool closing = false)
		{
			OnClosed();

			if (closing)
			{
				currentScenes.Clear();
			}
			else
			{
				currentScenes.RemoveAll(o => o.SceneName != "");
			}
		}

		//TODO: Having the function take in an entity name could 
		//cause problems if there are multiple entities of the same name.
		/// <summary>
		/// Prevents the entity from being destroyed when a new scene is loaded.
		/// </summary>
		/// <param name="entityName">The name of the entity.</param>
		public static void CarryableEntity(string entityName)
		{
			EntityData data = ECSManager.entities.Find(o => o.Name == entityName);

			data.SceneName = "";
		}

		internal static bool SceneExists(string sceneName)
		{
			return currentScenes.Exists(o => o.SceneName == sceneName);
		}

		internal static void Initialize()
		{
			foreach (Scene scene in currentScenes)
			{
				scene.Initialize();
			}
		}

		internal static void Update(GameTime gameTime)
		{
			foreach (Scene scene in currentScenes)
			{
				scene.Update(gameTime);
			}
		}

		internal static void Draw(RenderWindow window)
		{
			foreach (Scene scene in currentScenes)
			{
				scene.Draw(window);
			}
		}

		private static void OnClosed()
		{
			foreach (Scene scene in currentScenes)
			{
				scene.OnClosed();
			}
		}
	}
}

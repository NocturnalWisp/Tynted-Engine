using Tynted.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public static class SceneManager
	{
		private static List<Scene> currentScenes = new List<Scene>();

        /// <summary>
        /// Loads the main static scene for overall usage.
        /// </summary>
        internal static void LoadStaticScene()
        {
            Scene newScene = new Scene("");

            currentScenes.Add(newScene);
            newScene.Initialize();
        }

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

            foreach (Scene scene in currentScenes.Where(o => o.SceneName.Equals(sceneName)))
            {
                scene.OnClosed();
            }

            //dereferences it from the list
			currentScenes.RemoveAll(o => o.SceneName == sceneName);
		}

		public static void UnloadScene(Scene scene)
		{
            scene.OnClosed();

			currentScenes.Remove(scene);
		}

		public static void UnloadAllScenes()
		{
            currentScenes.RemoveAll(o => o.SceneName != "");
        }

        public static void PauseScene(string sceneName)
        {
            if (sceneName != "")
            {
                foreach (Scene scene in currentScenes.Where(o => !o.paused && o.SceneName.Equals(sceneName)))
                {
                    scene.paused = true;
                }
            }
        }

        public static void PauseScene(Scene scene)
        {
            scene.paused = true;
        }

        public static void ResumeScene(string sceneName)
        {
            if (sceneName != "")
            {
                foreach (Scene scene in currentScenes.Where(o => !o.paused && o.SceneName.Equals(sceneName)))
                {
                    scene.paused = false;
                }
            }
        }

        public static void ResumeScene(Scene scene)
        {
            scene.paused = false;
        }

        public static bool SceneExists(string sceneName)
		{
            if (sceneName != "")
                return currentScenes.Exists(o => o.SceneName == sceneName);
            else
                return false;
		}

        public static Scene GetSceneByName(string sceneName)
        {
            if (sceneName != "")
                return currentScenes.Find(o => o.SceneName.Equals(sceneName));
            else
                return null;
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

		internal static void OnClosed()
		{
			foreach (Scene scene in currentScenes)
			{
				scene.OnClosed();
			}

            currentScenes.Clear();
		}
	}
}

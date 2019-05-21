using Tynted.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public Scene(string sceneName)
		{
			this.sceneName = sceneName;
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

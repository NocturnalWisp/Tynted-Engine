using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.SFML.Graphics;

namespace ECSEngine
{
	public class System
	{
		public virtual void Initialize()
		{

		}

		/// <summary>
		/// Use this to create all the events that a system will utilize.
		/// </summary>
		public virtual void CreateEvents()
		{

		}

		/// <summary>
		/// Use this to subscribe to events that have been created.
		/// </summary>
		public virtual void SubscribeEvents()
		{

		}

		public virtual void Update(GameTime gameTime)
		{

		}

		public virtual void Draw(RenderWindow window)
		{

		}
	}
}

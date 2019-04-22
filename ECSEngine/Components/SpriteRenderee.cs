using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.SFML.Graphics;

namespace ECSEngine.Components
{
	public struct SpriteRenderee : IComponent
	{
		public bool Enabled { get; set; }

		Texture texture;
		public Sprite sprite;

		public SpriteRenderee(Texture texture)
		{
			this.texture = texture;
			sprite = new Sprite(texture);
			Enabled = true;
		}
	}
}

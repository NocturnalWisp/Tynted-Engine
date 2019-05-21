using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tynted;
using Tynted.SFML.Graphics;

namespace Tynted.Components
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

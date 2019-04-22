using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;

using ECSEngine;


using ECSEngine.Components;
using ECSEngine.SFML.Graphics;

namespace ECSEngine.Audio.Systems
{
	class SpriteRenderer : System
	{
		public override void Draw(RenderWindow window)
		{
			var spriteRenderees = SystemManager.GetComponentEntityList(new SpriteRenderee());
			var transforms = SystemManager.GetComponentEntityList(new Components.Transform());

			for (int entityID = 0; entityID < spriteRenderees.Count; entityID++)
			{
				if (transforms.ContainsKey(entityID))
				{
					SpriteRenderee sRenderee = (SpriteRenderee)spriteRenderees[entityID];
					window.Draw(sRenderee.sprite);
					sRenderee.sprite.Position = ((Components.Transform)transforms[entityID]).position;
				}
			}

			base.Draw(window);
		}

		//Movement
		//public override void Update(GameTime gameTime)
		//{
		//	var spriteRenderees = SystemManager.GetComponentEntityList(new SpriteRenderee());
		//	var transforms = SystemManager.GetComponentEntityList(new Components.Transform());

		//	for (int entityID = 0; entityID < spriteRenderees.Count; entityID++)
		//	{
		//		if (transforms.ContainsKey(entityID))
		//		{
		//			Components.Transform t = (Components.Transform)transforms[entityID];
		//			t.position += new Vector2f(1, 1);

		//			transforms[entityID] = t;
		//		}
		//	}

		//	base.Update(gameTime);
		//}
	}
}

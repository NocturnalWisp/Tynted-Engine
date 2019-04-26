using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;
using ECSEngine.Components;
using ECSEngine.Events;
using ECSEngine.SFML.Graphics;
using ECSEngine.SFML.System;

using Box2DNet.Common;

namespace ECSEngine.Systems
{
	class SpriteRenderer : System
	{
		public override void Draw(RenderWindow window)
		{
			var spriteRenderees = ECSManager.GetComponentEntityActiveList(new SpriteRenderee());
			var transforms = ECSManager.GetComponentEntityActiveList(new Components.Transform());

			for (int entityID = 0; entityID < spriteRenderees.Count(); entityID++)
			{
				if (transforms.ContainsKey(entityID))
				{
					SpriteRenderee sRenderee = (SpriteRenderee)spriteRenderees[entityID];
					Components.Transform transform = (Components.Transform)transforms[entityID];
					window.Draw(sRenderee.sprite);
					sRenderee.sprite.Position = transform.position;
					sRenderee.sprite.Rotation = transform.rotation;
					sRenderee.sprite.Scale = transform.scale;

					spriteRenderees[entityID] = sRenderee;
				}
			}

			base.Draw(window);
		}

		private IEnumerable<DictionaryEntry> CastDict(IDictionary dictionary)
		{
			foreach (DictionaryEntry entry in dictionary)
			{
				yield return entry;
			}
		}
	}
}

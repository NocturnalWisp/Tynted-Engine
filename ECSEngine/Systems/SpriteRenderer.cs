using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;

using ECSEngine;


using ECSEngine.Components;
using ECSEngine.SFML.Graphics;
using ECSEngine.SFML.System;

namespace ECSEngine.Systems
{
	class SpriteRenderer : System
	{
		public override void Draw(RenderWindow window)
		{
			var spriteRenderees = SystemManager.GetComponentEntityActiveList(new SpriteRenderee());
			var transforms = SystemManager.GetComponentEntityActiveList(new Components.Transform());

			for (int entityID = 0; entityID < spriteRenderees.Count(); entityID++)
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

		private IEnumerable<DictionaryEntry> CastDict(IDictionary dictionary)
		{
			foreach (DictionaryEntry entry in dictionary)
			{
				yield return entry;
			}
		}
	}
}

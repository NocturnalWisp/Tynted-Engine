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
			var spriteRenderees = ECSManager.GetComponentEntityActiveList(typeof(SpriteRenderee));
			var transforms = ECSManager.GetComponentEntityActiveList(typeof(Components.Transform));

			for (int entityID = 0; entityID < spriteRenderees.Count(); entityID++)
			{
				if (transforms.Exists(o => o.entityID == entityID))
				{
					EntityComponent rComponent = spriteRenderees.Find(o => o.entityID == entityID);
					EntityComponent tComponent = transforms.Find(o => o.entityID == entityID);
					SpriteRenderee sRenderee = (SpriteRenderee)rComponent.component;
					Components.Transform transform = (Components.Transform)tComponent.component;
					window.Draw(sRenderee.sprite);
					sRenderee.sprite.Position = transform.position;
					sRenderee.sprite.Rotation = transform.rotation;
					sRenderee.sprite.Scale = transform.scale;

					rComponent.component = sRenderee;
					spriteRenderees[spriteRenderees.IndexOf(spriteRenderees.Find(o => o.entityID == entityID))] = rComponent;
				}
			}

			foreach(EntityComponent sr in spriteRenderees)
			{
				ECSManager.SetEntityComponent(sr.entityID, sr.component);
			}

			foreach (EntityComponent t in transforms)
			{
				ECSManager.SetEntityComponent(t.entityID, t.component);
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

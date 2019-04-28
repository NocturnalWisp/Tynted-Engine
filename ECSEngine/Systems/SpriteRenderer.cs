using ECSEngine.Components;
using ECSEngine.SFML.Graphics;
using Transform = ECSEngine.Components.Transform;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ECSEngine.Systems
{
	[RequireComponents(typeof(SpriteRenderee), typeof(Components.Transform))]
	class SpriteRenderer : System
	{
		public override void Draw(RenderWindow window)
		{
			var allTypes = GetEntities();

			foreach (Entity entity in allTypes)
			{
				SpriteRenderee sr = entity.GetComponent<SpriteRenderee>();
				Transform t = entity.GetComponent<Transform>();

				//Set sprite values based on transform
				sr.sprite.Position = t.position;
				sr.sprite.Rotation = t.rotation;
				sr.sprite.Scale = t.scale;

				window.Draw(sr.sprite);

				entity.SetComponent(sr);
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

using ECSEngine.Components;
using ECSEngine.SFML.Graphics;
using Transform = ECSEngine.Components.Transform;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ECSEngine.Systems
{
	[GetComponents(typeof(SpriteRenderee), typeof(Components.Transform))]
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
				sr.sprite.Position = t.GetWorldPosition();
				sr.sprite.Rotation = t.GetWorldRotation();
				sr.sprite.Scale = t.GetWorldScale();

				window.Draw(sr.sprite);

				entity.SetComponent(sr);
			}

			base.Draw(window);
		}
	}
}

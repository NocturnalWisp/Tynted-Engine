﻿using Tynted.Components;
using Tynted.SFML.Graphics;
using Transform = Tynted.Components.Transform;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Tynted.Systems
{
	[GetComponents(typeof(SpriteRenderee), typeof(Components.Transform))]
	public class SpriteRenderer : System
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
				sr.sprite.Rotation = t.LocalRotation;
				sr.sprite.Scale = t.LocalScale;

				window.Draw(sr.sprite);
            }

			base.Draw(window);
		}
	}
}

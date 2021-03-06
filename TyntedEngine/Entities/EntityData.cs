﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public struct EntityData : IEquatable<EntityData>
	{
		public int EntityID { get; set; }
		public string Name { get; set; }
		public string Tag { get; set; }
		public string SceneName { get; set; }

		internal EntityData(int entityID, string name, string sceneName, string tag)
		{
			EntityID = entityID;
			Name = name;
			Tag = tag;
			SceneName = sceneName;
		}

		public bool Equals(EntityData other)
		{
			return (EntityID == other.EntityID) && 
				(Name == other.Name) && 
				(Tag == other.Tag) && 
				(SceneName == other.SceneName);
		}
	}
}

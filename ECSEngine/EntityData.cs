using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	public struct EntityData : IEquatable<EntityData>
	{
		public int EntityID { get; set; }
		public string Name { get; set; }
		public string Tag { get; set; }

		public EntityData(int entityID, string name, string tag)
		{
			EntityID = entityID;
			Name = name;
			Tag = tag;
		}

		public bool Equals(EntityData other)
		{
			return (EntityID == other.EntityID) && (Name == other.Name) && (Tag == other.Tag);
		}
	}
}

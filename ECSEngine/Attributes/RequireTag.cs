using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	/// <summary>
	/// Applied to system to only place entities with a certain tag in the GetEntities() list.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	class RequireTag : Attribute
	{
		public string tag;

		public RequireTag(string tag)
		{
			this.tag = tag;
		}
	}
}

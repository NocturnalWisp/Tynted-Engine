using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	/// <summary>
	/// Applied to system to only place entities with a certain tag in the GetEntities() list.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RequireTags : Attribute
	{
		public string[] tag;

		public RequireTags(params string[] tags)
		{
			this.tag = tags;
		}
	}
}

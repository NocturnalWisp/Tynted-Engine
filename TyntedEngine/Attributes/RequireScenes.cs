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
	public class RequireScenes : Attribute
	{
		public string[] scenes;

		public RequireScenes(params string[] scenes)
		{
			this.scenes = scenes;
		}
	}
}

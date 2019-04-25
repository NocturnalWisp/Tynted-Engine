using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class RequireComponents : Attribute
	{
		private Type[] components;

		public RequireComponents(params Type[] components)
		{
			foreach (Type t in components)
			{
				if (!(t is IComponent))
				{
					throw new Exception("The component specified is not of type IComponent.");
				}
				else
				{
					this.components = components;
				}
			}
		}

		public virtual Type[] Components
		{
			get { return components; }
		}
	}
}

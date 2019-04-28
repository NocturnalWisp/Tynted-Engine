using System;

namespace ECSEngine
{
	[AttributeUsage(AttributeTargets.Class)]
	class RequireComponents : Attribute
	{
		private Type[] types;
		public Type[] Types { get => types; }

		public RequireComponents(params Type[] types)
		{
			this.types = types;
		}
	}
}

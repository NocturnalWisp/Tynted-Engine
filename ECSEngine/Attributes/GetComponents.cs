using System;

namespace Tynted
{
	/// <summary>
	/// System attribute to have a list of each type of component provided to the System.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	class GetComponents : Attribute
	{
		private Type[] types;
		public Type[] Types { get => types; }

		public GetComponents(params Type[] types)
		{
			this.types = types;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public struct GameOptions
	{
		private static readonly GameOptions def = new GameOptions(false);

		public bool forceLimit;

		//Properties
		public static GameOptions Default
		{
			get
			{
				return def;
			}
		}

		public GameOptions(bool forceLimit = false)
		{
			this.forceLimit = forceLimit;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public struct GameOptions
	{
		private static readonly GameOptions def = new GameOptions(false, "New Game");

		public bool ForceLimit { get; set; }
		public string GameName { get; set; }

		//Properties
		public static GameOptions Default
		{
			get
			{
                return def;
			}
		}

		public GameOptions(bool forceLimit = false, string name)
		{
			this.ForceLimit = forceLimit;
			this.GameName = name;
		}
	}
}

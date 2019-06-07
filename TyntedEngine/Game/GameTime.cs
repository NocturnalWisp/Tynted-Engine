using Tynted;
using Tynted.SFML.System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public struct GameTime
	{
		public Time ElapsedTime { get; set; }
		public Time TotalTime { get; set; }
	}
}
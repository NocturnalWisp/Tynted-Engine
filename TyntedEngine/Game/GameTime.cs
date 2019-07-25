using Tynted;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public struct GameTime
	{
		public TimeSpan ElapsedTime { get; set; }
		public TimeSpan TotalTime { get; set; }
	}
}
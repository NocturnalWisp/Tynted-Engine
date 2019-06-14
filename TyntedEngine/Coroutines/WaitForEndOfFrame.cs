using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public class WaitForEndOfFrame : YieldInstruction
	{
		public override bool MoveNext()
		{
			return true;
		}

		public override void Reset()
		{
			
		}
	}
}

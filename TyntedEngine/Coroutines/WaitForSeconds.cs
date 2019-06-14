using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public class WaitForSeconds : YieldInstruction
	{
		public float duration;

		public WaitForSeconds(float duration)
		{
			this.duration = duration;
        }

		public override bool MoveNext()
		{
            return true;
		}

		public override void Reset()
		{
			
		}
	}
}

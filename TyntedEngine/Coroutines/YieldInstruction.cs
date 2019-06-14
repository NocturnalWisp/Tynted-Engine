using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	public abstract class YieldInstruction : IEnumerator
	{
		public object Current { get; set; }

		public bool IsDone { get => isDone; }

		internal bool isDone = false;

		public abstract bool MoveNext();

		public abstract void Reset();
	}
}

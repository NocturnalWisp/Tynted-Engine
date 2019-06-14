using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted.Events
{
	public delegate void EngineAction();
	public delegate void EngineAction<in T0>(T0 arg0);
	public delegate void EngineAction<in T0, in T1>(T0 arg0, T1 arg1);
	public delegate void EngineAction<in T0, in T1, in T2>(T0 arg0, T1 arg1, T2 arg2);
	public delegate void EngineAction<in T0, in T1, in T2, in T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted.Events
{
	public class TyntedEvent
	{
		List<EngineAction> actions;

		public TyntedEvent()
		{
			actions = new List<EngineAction>();
		}

		internal void AddListener(EngineAction call)
		{
			if (!actions.Contains(call))
			{
				actions.Add(call);
			}
		}

		internal void RemoveListener(EngineAction call)
		{
			if (actions.Contains(call))
			{
				actions.Remove(call);
			}
		}

		internal void RemoveAllListeners()
		{
			for (int i = actions.Count-1; i >= 0; i--)
			{
				actions.RemoveAt(i);
			}
		}

		internal void Invoke()
		{
			foreach (EngineAction action in actions)
			{
				action.Invoke();
			}
		}
	}

	public class TyntedEvent<T0>
	{
		List<EngineAction<T0>> actions;

		public TyntedEvent()
		{
			actions = new List<EngineAction<T0>>();
		}

		internal void AddListener(EngineAction<T0> call)
		{
			if (!actions.Contains(call))
			{
				actions.Add(call);
			}
		}

		internal void RemoveListener(EngineAction<T0> call)
		{
			if (actions.Contains(call))
			{
				actions.Remove(call);
			}
		}

		internal void RemoveAllListeners()
		{
			for (int i = actions.Count - 1; i >= 0; i--)
			{
				actions.RemoveAt(i);
			}
		}

		internal void Invoke(T0 arg0)
		{
			foreach (EngineAction<T0> action in actions)
			{
				action.Invoke(arg0);
			}
		}
	}

	public class TyntedEvent<T0, T1>
	{
		List<EngineAction<T0, T1>> actions;

		public TyntedEvent()
		{
			actions = new List<EngineAction<T0, T1>>();
		}

		internal void AddListener(EngineAction<T0, T1> call)
		{
			if (!actions.Contains(call))
			{
				actions.Add(call);
			}
		}

		internal void RemoveListener(EngineAction<T0, T1> call)
		{
			if (actions.Contains(call))
			{
				actions.Remove(call);
			}
		}

		internal void RemoveAllListeners()
		{
			for (int i = actions.Count - 1; i >= 0; i--)
			{
				actions.RemoveAt(i);
			}
		}

		internal void Invoke(T0 arg0, T1 arg1)
		{
			foreach (EngineAction<T0, T1> action in actions)
			{
				action.Invoke(arg0, arg1);
			}
		}
	}
}

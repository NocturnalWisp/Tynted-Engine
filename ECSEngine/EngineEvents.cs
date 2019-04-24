using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	public delegate void EngineAction();
	public delegate void EngineAction<T0>(T0 arg0);
	public delegate void EngineAction<T0, T1>(T0 arg0, T1 arg1);

	public class EngineEvent
	{
		List<EngineAction> actions;

		public EngineEvent()
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

	public class EngineEvent<T0>
	{
		List<EngineAction<T0>> actions;

		public EngineEvent()
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
	//FIX: Coroutines stop working after certain number of movements
	public static class Coroutine
	{
		static List<IEnumerator> unblockedCoroutines = new List<IEnumerator>();
		static List<IEnumerator> shouldRunNextFrame = new List<IEnumerator>();
		static List<IEnumerator> shouldRunAtEndOfFrame = new List<IEnumerator>();
		static List<Tuple<float, IEnumerator>> shouldRunAfterTimes = new List<Tuple<float, IEnumerator>>();

		/// <summary>
		/// Runs a coroutine.
		/// </summary>
		/// <param name="c">The coroutine method to run.</param>
		/// <returns></returns>
		public static void StartCoroutine(IEnumerator c)
		{
			unblockedCoroutines.Add(c);
		}

		/// <summary>
		/// Stops a running coroutine.
		/// </summary>
		/// <param name="c">The coroutine to stop.</param>
		public static void StopCoroutine(IEnumerator c)
		{
			if (unblockedCoroutines.Contains(c))
			{
				unblockedCoroutines.Remove(c);
			}else if (shouldRunNextFrame.Contains(c))
			{
				shouldRunNextFrame.Remove(c);
			}else if (shouldRunAtEndOfFrame.Contains(c))
			{
				shouldRunAtEndOfFrame.Remove(c);
			}else if (shouldRunAfterTimes.Find(o => o.Item2 == c) != null)
			{
				shouldRunAfterTimes.RemoveAll(o => o.Item2 == c);
			}
		}

		internal static void Update(GameTime gameTime)
		{
			//do the wait for seconds
            foreach (Tuple<float, IEnumerator> timeRun in shouldRunAfterTimes.ToList())
            {
                if ((int)gameTime.TotalTime.AsSeconds() >= timeRun.Item1)
                {
                    unblockedCoroutines.Add(timeRun.Item2);
                    shouldRunAfterTimes.RemoveAt(shouldRunAfterTimes.FindIndex(o => o.Item2 == timeRun.Item2));
                }
            }

			foreach (IEnumerator coroutine in unblockedCoroutines.ToList())
			{
				if (!coroutine.MoveNext())
				{
					// This coroutine has finished
					unblockedCoroutines.Remove(coroutine);

					if (coroutine.Current is YieldInstruction)
					{
						((YieldInstruction)coroutine.Current).isDone = true;
					}
                    continue;
				}

				if (!(coroutine.Current is YieldInstruction))
				{
					// This coroutine yielded null, or some other value we don't understand; run it next frame.
					shouldRunNextFrame.Add(coroutine);
					unblockedCoroutines.Remove(coroutine);
					continue;
				}

				if (coroutine.Current is WaitForSeconds)
				{
					WaitForSeconds wait = (WaitForSeconds)coroutine.Current;
					shouldRunAfterTimes.Add(new Tuple<float, IEnumerator>((int)gameTime.TotalTime.AsSeconds() + wait.duration, coroutine));

					unblockedCoroutines.Remove(coroutine);
				}
				else if (coroutine.Current is WaitForEndOfFrame)
				{
					shouldRunAtEndOfFrame.Add(coroutine);
					unblockedCoroutines.Remove(coroutine);
                }
			}

			unblockedCoroutines.AddRange(shouldRunNextFrame);
			shouldRunNextFrame.Clear();
		}

		internal static void EndFrame()
		{
			foreach (IEnumerator coroutine in shouldRunAtEndOfFrame.ToList())
			{
				unblockedCoroutines.Add(coroutine);
				shouldRunAtEndOfFrame.Remove(coroutine);
			}
		}
	}
}

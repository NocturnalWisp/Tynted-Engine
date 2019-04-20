using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECSEngine;

namespace GameTest
{
	class Program
	{
		static void Main(string[] args)
		{
			using (TestGame game = new TestGame())
			{
				game.Run();
			}
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSEngine
{
	public interface IComponent
	{
		bool Enabled { get; set; }
	}
}

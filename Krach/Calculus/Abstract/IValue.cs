using System;
using System.Collections.Generic;

namespace Krach.Calculus.Abstract
{
	public interface IValue
	{
		int Dimension { get; }
		
		IEnumerable<double> Evaluate();
	}
}


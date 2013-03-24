using System;
using System.Collections.Generic;

namespace Krach.Calculus.Abstract
{
	public interface IValue
	{
		int Dimension { get; }
		
		string GetText();
		IEnumerable<double> Evaluate();
	}
}


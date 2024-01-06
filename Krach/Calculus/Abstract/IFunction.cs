using System;
using System.Collections.Generic;

namespace Krach.Calculus.Abstract
{
	public interface IFunction
	{
		int DomainDimension { get; }
		int CodomainDimension { get; }
		
		IEnumerable<double> Evaluate(IEnumerable<double> parameters);
		IEnumerable<IFunction> GetDerivatives();
	}
}


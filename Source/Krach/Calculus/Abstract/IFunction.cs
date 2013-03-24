using System;
using System.Collections.Generic;

namespace Krach.Calculus.Abstract
{
	public interface IFunction
	{
		int DomainDimension { get; }
		int CodomainDimension { get; }
		
		string GetText();
		bool HasCustomApplicationText(IValue parameter);
		string GetCustomApplicationText(IValue parameter);
		IEnumerable<double> Evaluate(IEnumerable<double> parameters);
		IEnumerable<IFunction> GetDerivatives();
	}
}


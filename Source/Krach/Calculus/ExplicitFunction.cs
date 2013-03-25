using System;
using System.Collections.Generic;

namespace Krach.Calculus
{
	class ExplicitFunction : IFunction
	{
		readonly int domainDimension;
		readonly int codomainDimension;
		readonly Func<IEnumerable<double>, IEnumerable<double>> evaluate;
		readonly IEnumerable<IFunction> derivatives;
		
		public int DomainDimension { get { return domainDimension; } }
		public int CodomainDimension { get { return codomainDimension; } }
		
		public ExplicitFunction(int domainDimension, int codomainDimension, Func<IEnumerable<double>, IEnumerable<double>> evaluate, IEnumerable<IFunction> derivatives)
		{
			if (domainDimension < 0) throw new ArgumentOutOfRangeException("domainDimension");
			if (codomainDimension < 0) throw new ArgumentOutOfRangeException("codomainDimension");
			if (evaluate == null) throw new ArgumentNullException("evaluate");
			if (derivatives == null) throw new ArgumentNullException("derivatives");
			
			this.domainDimension = domainDimension;
			this.codomainDimension = codomainDimension;
			this.evaluate = evaluate;
			this.derivatives = derivatives;
		}
		
		public IEnumerable<double> Evaluate(IEnumerable<double> parameters)
		{
			return evaluate(parameters);
		}
		public IEnumerable<IFunction> GetDerivatives()
		{
			return derivatives;
		}
	}
}


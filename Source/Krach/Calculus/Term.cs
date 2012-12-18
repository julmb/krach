using System;
using System.Collections.Generic;
using Krach.Basics;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus
{
	public abstract class Term
	{
		public abstract double Evaluate();
		public abstract Term Substitute(Variable variable, Term substitute);
		public abstract Term GetDerivative(Variable variable);
		
		public Term Assign(IEnumerable<Variable> variables, Matrix matrix)
		{
			return
				Enumerable.Zip(variables, matrix.Columns.Single(), Tuple.Create)
				.Aggregate(this, (result, item) => result.Substitute(item.Item1, new Constant(item.Item2)));
		}
	}
}


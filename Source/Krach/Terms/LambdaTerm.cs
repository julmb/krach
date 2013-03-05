using System;
using System.Collections.Generic;
using System.Linq;

namespace Krach.Terms
{
	public abstract class LambdaTerm<T> where T : LambdaTerm<T>
	{
		public abstract T Substitute(Variable variable, Term substitute);
		public T Substitute(IEnumerable<Variable> variables, IEnumerable<Term> substitutes)
		{
			return
				Enumerable.Zip(variables, substitutes, Tuple.Create)
				.Aggregate((T)this, (result, item) => result.Substitute(item.Item1, item.Item2));
		}
	}
}


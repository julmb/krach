using System;
using System.Collections.Generic;
using Krach.Extensions;
using System.Linq;

namespace Krach.Terms
{
	public class Product : Function
	{
		public override IEnumerable<Function> Jacobian 
		{
			get 
			{
				yield return new Abstraction(Enumerables.Create(new Variable("x"), new Variable("y")), Term.Variable("y"));
				yield return new Abstraction(Enumerables.Create(new Variable("x"), new Variable("y")), Term.Variable("x"));
			}
		}

		public override string ToString()
		{
			return "*";
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return values.ElementAt(0) * values.ElementAt(1);
		}
	}
}

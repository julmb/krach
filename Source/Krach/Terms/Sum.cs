using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms
{
	public class Sum : Function
	{
		public override IEnumerable<Function> Jacobian 
		{
			get 
			{
				yield return new Abstraction(Enumerables.Create(new Variable("x"), new Variable("y")), Term.Constant(1));
				yield return new Abstraction(Enumerables.Create(new Variable("x"), new Variable("y")), Term.Constant(1));
			}
		}
	
		public override string ToString()
		{
			return "+";
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return values.ElementAt(0) + values.ElementAt(1);
		}
	}
}


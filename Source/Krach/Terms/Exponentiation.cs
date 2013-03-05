using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms
{
	public class Exponentiation : Function
	{
		readonly Constant exponent;
	
		public override IEnumerable<Function> Jacobian 
		{
			get 
			{
				yield return new Abstraction
				(
					Enumerables.Create(new Variable("x")), 
					Term.Product(exponent, Term.Variable("x").Exponentiate(new Constant(exponent.Evaluate() - 1)))
				);
			}
		}
		
		public Exponentiation(Constant exponent) 
		{
			this.exponent = exponent;
		}

		public override string ToString()
		{
			return string.Format("^{0}", exponent.Evaluate());
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return values.ElementAt(0).Exponentiate(exponent.Evaluate());
		}
	}
}


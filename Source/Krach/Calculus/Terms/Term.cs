using System;
using System.Collections.Generic;
using Krach.Basics;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms
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

		public Exponentiation Exponentiate(Constant exponent)
		{
			return new Exponentiation(this, exponent);
		}
		public Exponentiation Square()
		{
			return Exponentiate(new Constant(2));
		}

		public static Constant Constant(double value)
		{
			return new Constant(value);
		}
		public static Variable Variable(string name)
		{
			return new Variable(name);
		}
		public static Sum Sum(Term term1, Term term2)
		{
			return new Sum(term1, term2);
		}
		public static Term Sum(IEnumerable<Term> terms)
		{
			return terms.Aggregate(Sum);
		}
		public static Sum Difference(Term term1, Term term2)
		{
			return new Sum(term1, new AdditiveInverse(term2));
		}
		public static Product Product(Term term1, Term term2)
		{
			return new Product(term1, term2);
		}
		public static Term Product(IEnumerable<Term> terms)
		{
			return terms.Aggregate(Product);
		}
		public static Product Quotient(Term term1, Term term2)
		{
			return new Product(term1, new MultiplicativeInverse(term2));
		}
	}
}


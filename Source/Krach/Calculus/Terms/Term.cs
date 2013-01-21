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

		public Term Exponentiate(Constant exponent)
		{
			return new Exponentiation(this, exponent);
		}
		public Term Square()
		{
			return Exponentiate(new Constant(2));
		}

		public static Term Constant(double value)
		{
			return new Constant(value);
		}
		public static Term Variable(string name)
		{
			return new Variable(name);
		}
		public static Term Sum(Term term1, Term term2)
		{
			return new Sum(term1, term2);
		}
		public static Term Sum(IEnumerable<Term> terms)
		{
			return terms.Aggregate<Term, Term>(Term.Constant(0), Sum);
		}
		public static Term Difference(Term term1, Term term2)
		{
			return new Sum(term1, new AdditiveInverse(term2));
		}
		public static Term Product(Term term1, Term term2)
		{
			return new Product(term1, term2);
		}
		public static Term Product(IEnumerable<Term> terms)
		{
			return terms.Aggregate(Term.Constant(1), Product);
		}
		public static Term Quotient(Term term1, Term term2)
		{
			return new Product(term1, new MultiplicativeInverse(term2));
		}
	}
}


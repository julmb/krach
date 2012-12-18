using System;
using System.Linq;
using System.Collections.Generic;

namespace Krach.Calculus
{
	public static class Terms
	{
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
		public static Term Sum(this IEnumerable<Term> terms)
		{
			return terms.Aggregate(Sum);
		}
		public static Term Difference(Term term1, Term term2)
		{
			return new Sum(term1, new AdditiveInverse(term2));
		}
		public static Term Product(Term term1, Term term2)
		{
			return new Product(term1, term2);
		}
		public static Term Product(this IEnumerable<Term> terms)
		{
			return terms.Aggregate(Product);
		}
		public static Term Quotient(Term term1, Term term2)
		{
			return new Product(term1, new MultiplicativeInverse(term2));
		}
		public static Term Exponentiate(this Term term, Constant exponent)
		{
			return new Exponentiation(term, exponent);
		}
		public static Term Square(this Term term)
		{
			return term.Exponentiate(new Constant(2));
		}
	}
}


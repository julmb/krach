using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms
{
	public abstract class Term : LambdaTerm<Term>
	{
		public abstract double Evaluate();
		public abstract Term GetDerivative(Variable variable);
	
		public Term Negate() 
		{
			return Term.Product(Term.Constant(-1), this);
		}
		public Term Invert()
		{
			return Exponentiate(new Constant(-1));
		}
		public Term Exponentiate(Constant exponent)
		{
			return new Application(new Exponentiation(exponent), Enumerables.Create(this));
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
			return new Application(new Sum(), Enumerables.Create(term1, term2));
		}
		public static Term Sum(IEnumerable<Term> terms)
		{
			return terms.Aggregate(Term.Constant(0), Sum);
		}
		public static Term Difference(Term term1, Term term2)
		{
			return Term.Sum(term1, term2.Negate());
		}
		public static Term Product(Term term1, Term term2)
		{
			return new Application(new Product(), Enumerables.Create(term1, term2));
		}
		public static Term Product(IEnumerable<Term> terms)
		{
			return terms.Aggregate(Term.Constant(1), Product);
		}
		public static Term Quotient(Term term1, Term term2)
		{
			return Term.Product(term1, term2.Invert());
		}
	}
}


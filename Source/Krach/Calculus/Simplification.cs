using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Rewriting;
using Krach.Calculus.Terms.Combination;
using Krach.Extensions;
using Krach.Calculus.Terms.Rewriting.Rules;
using Krach.Calculus.Terms.Rewriting.Rules.FirstOrder;
using Krach.Calculus.Terms;
using System.Linq;

namespace Krach.Calculus
{
	public static class Simplification
	{
		public static ValueTerm Simplify(this ValueTerm valueTerm)
		{
			if (valueTerm == null) throw new ArgumentNullException("valueTerm");
			
			return GetSimplifier().Rewrite(valueTerm);
		}
		public static FunctionTerm Simplify(this FunctionTerm functionTerm)
		{
			if (functionTerm == null) throw new ArgumentNullException("functionTerm");
			
			return GetSimplifier().Rewrite(functionTerm);
		}
		public static IFunction Simplify(this FunctionTerm functionTerm, int depth)
		{
			if (functionTerm == null) throw new ArgumentNullException("functionTerm");
			if (depth < 0) throw new ArgumentOutOfRangeException("depth");
			
			IFunction function = functionTerm.Simplify();
			
			if (depth == 0) return function;
		
			return new ExplicitFunction
			(
				function.DomainDimension,
				function.CodomainDimension,
				function.Evaluate,
				(
					from derivative in functionTerm.GetDerivatives()
					select derivative.Simplify(depth - 1)
				)
				.ToArray()
			);	
		}
		
		static Rewriter GetSimplifier()
		{
			Variable x = new Variable(1, "x");
                        
			return new Rewriter
			(
				Enumerables.Create<Rule>
				(
					new BetaReduction(),
					new EtaContraction(),
					new SelectVector(),
					//new SingletonVector(),
					//new SelectSingle(),
					//new FlattenVector(),
					new SumExpansion(),
					new ProductExpansion(),
					new ScalingExpansion(),
					new FirstOrderRule(Term.Sum(Term.Constant(0), x), x),
					new FirstOrderRule(Term.Sum(x, Term.Constant(0)), x),
					new FirstOrderRule(Term.Product(Term.Constant(1), x), x),
					new FirstOrderRule(Term.Product(x, Term.Constant(1)), x),
					new FirstOrderRule(Term.Product(Term.Constant(0), x), Term.Constant(0)),
					new FirstOrderRule(Term.Product(x, Term.Constant(0)), Term.Constant(0)),
					new FirstOrderRule(Term.Constant(1).Exponentiate(-2), Term.Constant(1)),
					new FirstOrderRule(Term.Constant(1).Exponentiate(-1), Term.Constant(1)),
					new FirstOrderRule(Term.Constant(1).Exponentiate( 0), Term.Constant(1)),
					new FirstOrderRule(Term.Constant(1).Exponentiate(+1), Term.Constant(1)),
					new FirstOrderRule(Term.Constant(1).Exponentiate(+2), Term.Constant(1)),
					new FirstOrderRule(x.Exponentiate(0), Term.Constant(1)),
					new FirstOrderRule(x.Exponentiate(1), x)
				)
			);
		}
	}
}


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
			
			functionTerm = functionTerm.Simplify();
			
			if (depth == 0) return functionTerm;
		
			return new ExplicitFunction
			(
				functionTerm.DomainDimension,
				functionTerm.CodomainDimension,
				functionTerm.Evaluate,
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
			Variable y = new Variable(1, "y");
			Variable z = new Variable(1, "z");
                        
			return new Rewriter
			(
				Enumerables.Create<Rule>
				(
					// lambda term rewriting
					new BetaReduction(),
					new EtaContraction(),
					
					// vector simplification
					new SelectVector(),
					
					// operator expansion
					new SumExpansion(),
					new ProductExpansion(),
					new ScalingExpansion(),
					
					// basic simplification
					new FirstOrderRule(Term.Sum(Term.Constant(0), x), x),
					new FirstOrderRule(Term.Sum(x, Term.Constant(0)), x),
					new FirstOrderRule(Term.Product(Term.Constant(0), x), Term.Constant(0)),
					new FirstOrderRule(Term.Product(x, Term.Constant(0)), Term.Constant(0)),
					new FirstOrderRule(Term.Product(Term.Constant(1), x), x),
					new FirstOrderRule(Term.Product(x, Term.Constant(1)), x),
					new FirstOrderRule(Term.Exponentiate(Term.Constant(1), x), Term.Constant(1)),
					new FirstOrderRule(Term.Exponentiate(x, Term.Constant(0)), Term.Constant(1)),
					new FirstOrderRule(Term.Exponentiate(x, Term.Constant(1)), x),
					
					// advanced simplification
					new Evaluation(),
					new Sorting.ProductSimple(),
					new Sorting.ProductAssociative(),
					
					// distributivity
					new FirstOrderRule(Term.Product(x, Term.Sum(y, z)), Term.Sum(Term.Product(x, y), Term.Product(x, z))),
					new FirstOrderRule(Term.Product(Term.Sum(y, z), x), Term.Sum(Term.Product(y, x), Term.Product(z, x))),
					new FirstOrderRule(Term.Exponentiate(Term.Product(x, y), z), Term.Product(Term.Exponentiate(x, z), Term.Exponentiate(y, z))),
					
					// associativity
					new FirstOrderRule(Term.Sum(x, Term.Sum(y, z)), Term.Sum(Term.Sum(x, y), z)),
					new FirstOrderRule(Term.Product(x, Term.Product(y, z)), Term.Product(Term.Product(x, y), z)),
					
					// merging (sum -> product)
					new FirstOrderRule
					(
						Term.Sum(x, x),
						Term.Product(Term.Constant(2), x)
					),
					new FirstOrderRule
					(
						Term.Sum(Term.Product(y, x), x),
						Term.Product(Term.Sum(y, Term.Constant(1)), x)
					),
					new FirstOrderRule
					(
						Term.Sum(x, Term.Product(z, x)),
						Term.Product(Term.Sum(Term.Constant(1), z), x)
					),
					new FirstOrderRule
					(
						Term.Sum(Term.Product(y, x), Term.Product(z, x)),
						Term.Product(Term.Sum(y, z), x)
					),
					
					// merging (product -> exponentiation)
					new FirstOrderRule
					(
						Term.Product(x, x),
						Term.Exponentiate(x, Term.Constant(2))
					),
					new FirstOrderRule
					(
						Term.Product(Term.Exponentiate(x, y), x),
						Term.Exponentiate(x, Term.Sum(y, Term.Constant(1)))
					),
					new FirstOrderRule
					(
						Term.Product(x, Term.Exponentiate(x, z)),
						Term.Exponentiate(x, Term.Sum(Term.Constant(1), z))
					),
					new FirstOrderRule
					(
						Term.Product(Term.Exponentiate(x, y), Term.Exponentiate(x, z)),
						Term.Exponentiate(x, Term.Sum(y, z))
					),
					
					// merging (exponentiation -> exponentiation)
					new FirstOrderRule
					(
						Term.Exponentiate(Term.Exponentiate(x, y), z),
						Term.Exponentiate(x, Term.Product(y, z))
					),
					
					// binomial theorem, special case
					new FirstOrderRule
					(
						Term.Exponentiate(Term.Sum(x, y), Term.Constant(2)),
						Term.Sum
						(
							Term.Exponentiate(x, Term.Constant(2)),
							Term.Product(Term.Constant(2), x, y),
							Term.Exponentiate(y, Term.Constant(2))
						)
					)
				)
			);
		}
	}
}


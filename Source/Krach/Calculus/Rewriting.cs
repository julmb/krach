using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Rules;
using Krach.Calculus.Rules.Composite;
using Krach.Calculus.Rules.Definitions;
using Krach.Calculus.Rules.LambdaCalculus;
using Krach.Calculus.Rules.Vectors;
using Krach.Calculus.Rules.FirstOrder;
using Krach.Calculus.Rules.Simplification;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms.Basic.Atoms;
using System.Diagnostics;

namespace Krach.Calculus
{
	public static class Rewriting
	{
		static Rule DefinitionExpansion { get { return new Any(new ExpandValueDefinition(), new ExpandFunctionDefinition()); } }
		static Rule LambdaCalculusSimplification { get { return new Any(new BetaReduction(), new EtaContraction()); } }
		static Rule VectorSimplification { get { return new Any(new SelectVector(), new SingletonVector(), new SelectSingle()); } }
		static Rule BasicSimplification
		{
			get
			{
				Variable x = new Variable(1, "x");
				Variable y = new Variable(1, "y");
				Variable z = new Variable(1, "z");

				return new Any
				(
					new FirstOrderRule(new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(new Constant(0), x))), x),
					new FirstOrderRule(new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(0)))), x),
					new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Constant(0), x))), new Constant(0)),
					new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(0)))), new Constant(0)),
					new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Constant(1), x))), x),
					new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(1)))), x),
					new FirstOrderRule(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(new Constant(1), x))), new Constant(1)),
					new FirstOrderRule(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(0)))), new Constant(1)),
					new FirstOrderRule(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(1)))), x),
					
					// associativity
					new FirstOrderRule
					(
						new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(x, new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(y, z)))))),
						new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(x, y))), z)))
					),	
					new FirstOrderRule
					(
						new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(y, z)))))),
						new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, y))), z)))
					),
					
//					// distributivity
//					new FirstOrderRule
//					(
//						new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(y, z)))))),
//						new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, y))), new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, z))))))
//					),
//					new FirstOrderRule
//					(
//						new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(x, y))), z))),
//						new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, z))), new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(y, z))))))
//					)
					
					new FirstOrderRule
					(
						new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, y))), z))),
						new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(y, z))))))
					)
				);
			}
		}
		static Rule AdvancedSimplification { get { return new Any(new Evaluation(), new Sorting.ProductSimple(), new Sorting.ProductAssociative()); } }

		public static Rule Expansion { get { return new Repeat(new Anywhere(DefinitionExpansion)); } }
		public static Rule Simplification
		{
			get
			{
				return new Repeat
				(
					new Anywhere(new Any(LambdaCalculusSimplification, VectorSimplification, BasicSimplification, AdvancedSimplification))
				);
			}
		}
		public static Rule Normalization
		{
			get
			{
				return new Repeat
				(
					new Any
					(
						new Anywhere(new Any(LambdaCalculusSimplification, VectorSimplification, BasicSimplification, AdvancedSimplification)),
						new Anywhere(DefinitionExpansion)
					)
				);
			}
		}

		public static T Rewrite<T>(this T term, Rule rule) where T : VariableTerm<T>
		{
			if (term == null) throw new ArgumentNullException("valueTerm");
			if (rule == null) throw new ArgumentNullException("rule");

			if (rule is Repeat)
			{
				rule = ((Repeat)rule).Rule;
			}

			Stopwatch stopwatch = new Stopwatch();
			double longestTime = 0;
			T longestTimeTerm = null;

			Terminal.Write(term.ToString(), ConsoleColor.Red);
			Terminal.WriteLine();

			int rewriteCount = 0;

			while (true)
			{
				stopwatch.Restart();
				T rewrittenTerm = rule.Rewrite(term);
				stopwatch.Stop();
				
				if (rewrittenTerm == null) break;

				if (stopwatch.Elapsed.TotalSeconds > longestTime)
				{
					longestTime = stopwatch.Elapsed.TotalSeconds;
					longestTimeTerm = term;
				}

				term = rewrittenTerm;
				rewriteCount++;

//				Terminal.Write(term.ToString(), ConsoleColor.Yellow);
//				Terminal.WriteLine();
			}

			if (longestTimeTerm != null)
			{
				Terminal.Write(longestTime.ToString(), ConsoleColor.Cyan);
				Terminal.WriteLine();

				Terminal.Write(longestTimeTerm.ToString(), ConsoleColor.Cyan);
				Terminal.WriteLine();

				Terminal.Write(rule.Rewrite(longestTimeTerm).ToString(), ConsoleColor.Cyan);
				Terminal.WriteLine();
			}

			Terminal.Write(rewriteCount.ToString(), ConsoleColor.Yellow);
			Terminal.WriteLine();

			Terminal.Write(term.ToString(), ConsoleColor.Green);
			Terminal.WriteLine();

			return term;
		}

		public static IFunction Normalize(this FunctionTerm functionTerm, int depth)
		{
			if (functionTerm == null) throw new ArgumentNullException("functionTerm");
			if (depth < 0) throw new ArgumentOutOfRangeException("depth");
			
			functionTerm = functionTerm.Rewrite(Normalization);
			
			if (depth == 0) return functionTerm;
		
			return new ExplicitFunction
			(
				functionTerm.DomainDimension,
				functionTerm.CodomainDimension,
				functionTerm.Evaluate,
				(
					from derivative in functionTerm.GetDerivatives()
					select derivative.Normalize(depth - 1)
				)
				.ToArray()
			);	
		}
	}
}
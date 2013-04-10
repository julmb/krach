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
using Krach.Calculus.Abstract;
using Krach.Calculus.Explicit;
using Krach.Calculus.Terms.Constraints;

namespace Krach.Calculus
{
    public static class Rewriting
    {
        static Rule LambdaCalculusSimplification
        {
            get
            {
                return new Any
                (
                    new EtaContraction(),
                    new All
                    (
                        new BetaReduction(),
                        new Repeat(new Everywhere(VectorSimplification))
                    )
                );
            }
        }
        static Rule VectorSimplification { get { return new Any(new SingletonVector(), new SelectSingle(), new SelectVector(), new VectorSelect()); } }
		static Rule Simplification0
		{
			get
			{
                Variable x = new Variable(1, "x");

				return new Any
				(
					new Evaluation(),
	                new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Constant(0), x))), new Constant(0)),
	                new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(0)))), new Constant(0)),
                    new FirstOrderRule(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(new Constant(1), x))), new Constant(1)),
                    new FirstOrderRule(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(0)))), new Constant(1))
				);
			}
		}
        static Rule Simplification1
        {
            get
            {
                Variable x = new Variable(1, "x");

                return new Any
                (
                    new FirstOrderRule(new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(new Constant(0), x))), x),
                    new FirstOrderRule(new Application(new Sum(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(0)))), x),
                    new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Constant(1), x))), x),
                    new FirstOrderRule(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(1)))), x),
                    new FirstOrderRule(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(1)))), x),

					new FirstOrderRule(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, new Constant(0.5)))), new Constant(2)))), x),

                    new Identity.Application(),
                    new VectorSum.ZeroDimensional(), new VectorSum.OneDimensional(),
                    new DotProduct.ZeroDimensional(), new DotProduct.OneDimensional(),
                    new VectorScaling.ZeroDimensional(), new VectorScaling.OneDimensional(),
                    new BigSum.Nullary(), new BigSum.Unary(), new BigSum.Binary(),
                    new BigProduct.Nullary(), new BigProduct.Unary(), new BigProduct.Binary(),
                    new Norm.ZeroDimensional()
                );
            }
        }
		static Rule Simplification2
        {
            get
            {
                Variable x = new Variable(1, "x");
                Variable y = new Variable(1, "y");
                Variable z = new Variable(1, "z");

                return new Any
                (
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

					new Sorting.ProductSimple(), new Sorting.ProductAssociative()
                );
            }
        }
        static Rule Simplification
		{
			get 
			{
				return new Any
				(
					new Everywhere(Simplification0),
					new Everywhere(new Any(LambdaCalculusSimplification, VectorSimplification, Simplification1))
					//new Everywhere(Simplification2)
				);
			}
		}
		static Rule DefinitionExpansion
		{
			get
			{
				return new Any
                (
                    new ExpandValueDefinition(),
					// TODO: find out why this makes such a huge difference
					new ExpandAppliedFunctionDefinition(),
                    new ExpandFunctionDefinition()
				);
			}
		}
        static Rule BasicDefinitionExpansion
        {
            get
            {
                return new Everywhere(DefinitionExpansion);
            }
        }
		static Rule ShorteningDefinitionExpansion
        {
            get
            {
                return new Everywhere
				(
					new Shortening
	                (
						new All
						(
							DefinitionExpansion,
							new Repeat(new Everywhere(Simplification))
						)
					)
				);
            }
        }

        public static Rule CompleteSimplification { get { return new Repeat(new Any(Simplification, ShorteningDefinitionExpansion)); } }
        public static Rule CompleteNormalization { get { return new Repeat(new Any(Simplification, BasicDefinitionExpansion)); } }

		// TODO: remove debug code
		public static T RewriteAll<T>(this Rule rule, T term) where T : VariableTerm<T>
		{
			if (rule is Repeat) rule = ((Repeat)rule).Rule;

			int rewriteCount = 0;
//			Console.WriteLine(term);
//			Console.WriteLine();

			while (true)
			{
				T rewrittenTerm = rule.Rewrite(term);

				if (rewrittenTerm == null) break;

				term = rewrittenTerm;

				rewriteCount++;
//				Console.WriteLine(term);
//				Console.WriteLine();
			}

			Console.WriteLine(rewriteCount);

			return term;
		}
		public static IConstraint<T> Rewrite<T>(this Rule rule, IConstraint<T> constraint) where T : VariableTerm<T>
		{
			return new ExplicitConstraint<T>(rule.Rewrite(constraint.Item), constraint.Ranges);
		}
        public static IFunction Normalize(this FunctionTerm functionTerm, int depth)
        {
            if (functionTerm == null) throw new ArgumentNullException("functionTerm");
            if (depth < 0) throw new ArgumentOutOfRangeException("depth");

            functionTerm = CompleteNormalization.Rewrite(functionTerm);
			Console.WriteLine(new string(' ', (2 - depth) * 4) + functionTerm);

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
		public static IConstraint<IFunction> Normalize(this IConstraint<FunctionTerm> constraint, int depth)
		{
			return new ExplicitConstraint<IFunction>(constraint.Item.Normalize(depth), constraint.Ranges);
		}
    }
}
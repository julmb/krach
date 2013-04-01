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
        static Rule VectorSimplification { get { return new Any(new SingletonVector(), new SelectSingle(), new SelectVector()); } }
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

                    // TODO: this rule does not always hold: (x^2)^0.5 != x
                    new FirstOrderRule
                    (
                        new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, y))), z))),
                        new Application(new Exponentiation(), new Vector(Enumerables.Create<ValueTerm>(x, new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(y, z))))))
                    )
                );
            }
        }
        static Rule AdvancedSimplification
        {
            get
            {
                return new Any
                (
                    new Identity.Application(),
                    new VectorSum.ZeroDimensional(), new VectorSum.OneDimensional(),
                    new DotProduct.ZeroDimensional(), new DotProduct.OneDimensional(),
                    new VectorScaling.ZeroDimensional(), new VectorScaling.OneDimensional(),
                    new BigSum.Nullary(), new BigSum.Unary(), new BigSum.Binary(),
                    new BigProduct.Nullary(), new BigProduct.Unary(), new BigProduct.Binary(),
                    new Norm.ZeroDimensional(),

                    new Evaluation(),

                    new Sorting.ProductSimple(), new Sorting.ProductAssociative()
                );
            }
        }
        static Rule Simplification { get { return new Any(LambdaCalculusSimplification, VectorSimplification, BasicSimplification, AdvancedSimplification); } }
        static Rule DefinitionExpansion
        {
            get
            {
                return new Any
                (
                    new ExpandValueDefinition(),
                    new All
                    (
                        new ExpandAppliedFunctionDefinition(),
                        new Repeat(new Everywhere(LambdaCalculusSimplification))
                    ),
                    new ExpandFunctionDefinition()
                );
            }
        }

        public static Rule CompleteSimplification { get { return new Repeat(new Everywhere(Simplification)); } }
        public static Rule CompleteNormalization { get { return new Repeat(new Any(new Everywhere(Simplification), new Everywhere(DefinitionExpansion))); } }

        public static IFunction Normalize(this FunctionTerm functionTerm, int depth)
        {
            if (functionTerm == null) throw new ArgumentNullException("functionTerm");
            if (depth < 0) throw new ArgumentOutOfRangeException("depth");

            functionTerm = CompleteNormalization.Rewrite(functionTerm);

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
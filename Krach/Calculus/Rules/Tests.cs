using System;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic.Atoms;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Rules;
using Krach.Calculus.Rules.Composite;
using Krach.Calculus.Rules.Definitions;
using Krach.Calculus.Rules.FirstOrder;
using Krach.Calculus.Rules.LambdaCalculus;
using Krach.Calculus.Rules.Simplification;
using Krach.Calculus.Rules.Vectors;
using Krach.Calculus.Terms.Notation;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic.Definitions;
using Krach.Calculus.Terms.Notation.Custom;

namespace Krach.Calculus.Rules
{
    public static class Tests
    {
        static ValueTerm ExpandValueDefinitionTest { get { return new ValueDefinition("test1", new Constant(42), new BasicSyntax("test1")); } }
        static FunctionTerm ExpandFunctionDefinitionTest { get { return new FunctionDefinition("test2", new Constant(42).Abstract(), new BasicSyntax("test2")); } }
        static ValueTerm ExpandAppliedFunctionDefinitionTest { get { return new Application(new FunctionDefinition("test3", new Constant(42).Abstract(), new BasicSyntax("test3")), new Vector(Enumerables.Create<ValueTerm>())); } }

        static ValueTerm BetaReductionTest
        {
            get
            {
                Variable x = new Variable(1, "x");
                Variable y = new Variable(1, "y");

                return new Application(new Abstraction(Enumerables.Create(x, y), x), new Vector(Enumerables.Create(new Constant(41), new Constant(42))));
            }
        }
        static FunctionTerm EtaContractionTest
        {
            get
            {
                Variable x = new Variable(1, "x");
                Variable y = new Variable(1, "y");

                return new Abstraction(Enumerables.Create(x, y), new Application(new Sum(), new Vector(Enumerables.Create(x, y))));
            }
        }

        static ValueTerm IdentityApplicationTest { get { return new Application(Term.Identity(2), new Vector(Enumerables.Create(new Constant(41), new Constant(42)))); } }

        static FunctionTerm VectorSumZeroDimensionalTest { get { return Term.VectorSum(0); } }
        static FunctionTerm VectorSumOneDimensionalTest { get { return Term.VectorSum(1); } }

        static FunctionTerm DotProductZeroDimensionalTest { get { return Term.DotProduct(0); } }
        static FunctionTerm DotProductOneDimensionalTest { get { return Term.DotProduct(1); } }

        static FunctionTerm VectorScalingZeroDimensionalTest { get { return Term.VectorScaling(0); } }
        static FunctionTerm VectorScalingOneDimensionalTest { get { return Term.VectorScaling(1); } }

        static FunctionTerm BigSumNullaryTest { get { return Term.BigSum(3, 0); } }
        static FunctionTerm BigSumUnaryTest { get { return Term.BigSum(3, 1); } }
        static FunctionTerm BigSumBinaryTest { get { return Term.BigSum(3, 2); } }

        static FunctionTerm BigProductNullaryTest { get { return Term.BigProduct(0); } }
        static FunctionTerm BigProductUnaryTest { get { return Term.BigProduct(1); } }
        static FunctionTerm BigProductBinaryTest { get { return Term.BigProduct(2); } }

        static FunctionTerm NormZeroDimensionalTest { get { return Term.Norm(0); } }

        static ValueTerm EvaluationTest { get { return Term.Scaling(Term.Constant(5), Term.Vector(Term.Sum(Term.Constant(1), Term.Constant(2)), Term.Constant(4))); } }

        static ValueTerm SortingProductSimple { get { return new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Variable(1, "x"), new Constant(5)))); } }
        static ValueTerm SortingProductAssociative { get { return new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Application(new Product(), new Vector(Enumerables.Create<ValueTerm>(new Constant(5), new Variable(1, "x")))), new Constant(5)))); } }

        static ValueTerm SingletonVectorTest { get { return new Vector(Enumerables.Create(new Variable(4, "x"))); } }
        static ValueTerm SelectSingleTest { get { return new Selection(new Variable(1, "x"), 0); } }
        static ValueTerm SelectVectorTest { get { return new Selection(new Vector(Enumerables.Create(new Constant(41), new Constant(42))), 1); } }
		static ValueTerm VectorSelectTest { get { return new Vector(Enumerables.Create(new Selection(new Variable(2, "x"), 0), new Selection(new Variable(2, "x"), 1))); } }
        static ValueTerm FlattenVectorTest { get { return new Vector(Enumerables.Create<ValueTerm>(new Constant(41), new Vector(Enumerables.Create(new Constant(42))))); } }

        // errors found using unit tests: 2
        public static void RunTests()
        {
            RunTest(ExpandValueDefinitionTest, new ExpandValueDefinition());
            RunTest(ExpandFunctionDefinitionTest, new ExpandFunctionDefinition());
            RunTest(ExpandAppliedFunctionDefinitionTest, new ExpandAppliedFunctionDefinition());

            RunTest(BetaReductionTest, new BetaReduction());
            RunTest(EtaContractionTest, new EtaContraction());

            RunTest(IdentityApplicationTest, new Identity.Application());

            RunTest(VectorSumZeroDimensionalTest, new VectorSum.ZeroDimensional());
            RunTest(VectorSumOneDimensionalTest, new VectorSum.OneDimensional());

            RunTest(DotProductZeroDimensionalTest, new DotProduct.ZeroDimensional());
            RunTest(DotProductOneDimensionalTest, new DotProduct.OneDimensional());

            RunTest(VectorScalingZeroDimensionalTest, new VectorScaling.ZeroDimensional());
            RunTest(VectorScalingOneDimensionalTest, new VectorScaling.OneDimensional());

            RunTest(BigSumNullaryTest, new BigSum.Nullary());
            RunTest(BigSumUnaryTest, new BigSum.Unary());
            RunTest(BigSumBinaryTest, new BigSum.Binary());

            RunTest(BigProductNullaryTest, new BigProduct.Nullary());
            RunTest(BigProductUnaryTest, new BigProduct.Unary());
            RunTest(BigProductBinaryTest, new BigProduct.Binary());

            RunTest(NormZeroDimensionalTest, new Norm.ZeroDimensional());

            RunTest(EvaluationTest, new Evaluation());

            RunTest(SortingProductSimple, new Sorting.ProductSimple());
            RunTest(SortingProductAssociative, new Sorting.ProductAssociative());

            RunTest(SingletonVectorTest, new SingletonVector());
            RunTest(SelectSingleTest, new SelectSingle());
            RunTest(SelectVectorTest, new SelectVector());
			RunTest(VectorSelectTest, new VectorSelect());
            RunTest(FlattenVectorTest, new FlattenVector());
        }

        static void RunTest<T>(T term, Rule rule) where T : VariableTerm<T>
        {
			Terminal.Write(string.Format("testing rule {0}...", rule.GetType().Name));

            T result = rule.Rewrite(term);

            if (result == null) throw new InvalidOperationException();

            if (term is ValueTerm)
            {
                if (!(result is ValueTerm)) throw new InvalidOperationException();

                ValueTerm termValueTerm = (ValueTerm)(BaseTerm)term;
                ValueTerm resultValueTerm = (ValueTerm)(BaseTerm)result;

                if (termValueTerm.Dimension != resultValueTerm.Dimension) throw new InvalidOperationException();
            }

            if (term is FunctionTerm)
            {
                if (!(result is FunctionTerm)) throw new InvalidOperationException();

                FunctionTerm termFunctionTerm = (FunctionTerm)(BaseTerm)term;
                FunctionTerm resultFunctionTerm = (FunctionTerm)(BaseTerm)result;

                if (resultFunctionTerm.DomainDimension != termFunctionTerm.DomainDimension) throw new InvalidOperationException();
                if (resultFunctionTerm.CodomainDimension != termFunctionTerm.CodomainDimension) throw new InvalidOperationException();
            }

			Terminal.Write("success");
			Terminal.WriteLine();
        }
    }
}

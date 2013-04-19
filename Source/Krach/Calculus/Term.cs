using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus.Terms.Basic.Atoms;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Rules;
using Krach.Calculus.Rules.Composite;
using Krach.Calculus.Rules.Vectors;
using Krach.Calculus.Rules.LambdaCalculus;
using Krach.Calculus.Rules.Simplification;
using Krach.Calculus.Terms.Notation.Custom;
using Krach.Calculus.Terms.Basic.Definitions;

namespace Krach.Calculus
{
    // term creation
    //   term constructors
    //     description: create terms exactly as specified, no syntactical changes
    //     use case: syntax is important, terms should be created exactly as specified
    //     examples: left-hand side of first order rules, replacing subterms as in Anywhere, Everywhere, Sorting
    //   convencience methods in Term
    //     description: create terms with focus on convenience for the caller, not necessarily syntactically unchanged
    //     use case: syntax does not matter, only semantics is important
    //     examples: deriving basic and composite terms, specifying terms in client code
    //   simplification in Rewriting
    //     description: simplifies terms syntactically
    //     use case: syntax is important, terms should be as simple as possible
    //     examples: terms need to be simplified before being printed or derived
    public static class Term
    {
        // lambda terms
        public static ValueTerm Variable(int dimension, string name)
        {
            return new Variable(dimension, name);
        }
        public static ValueTerm Variable(string name)
        {
            return Variable(1, name);
        }
        public static FunctionTerm Abstract(this ValueTerm term, IEnumerable<Variable> variables)
        {
            return new Abstraction(variables, term);
        }
        public static FunctionTerm Abstract(this ValueTerm term, params Variable[] variables)
        {
            return term.Abstract((IEnumerable<Variable>)variables);
        }
        public static ValueTerm Apply(this FunctionTerm function, IEnumerable<ValueTerm> parameters)
        {
            return new Application(function, Vector(parameters));
        }
        public static ValueTerm Apply(this FunctionTerm function, params ValueTerm[] parameters)
        {
            return function.Apply((IEnumerable<ValueTerm>)parameters);
        }

        // vectors
        public static ValueTerm Vector(IEnumerable<ValueTerm> terms)
        {
            return new Vector(terms);
        }
        public static ValueTerm Vector(params ValueTerm[] terms)
        {
            return Vector((IEnumerable<ValueTerm>)terms);
        }
        public static ValueTerm Select(this ValueTerm term, int index)
        {
            return new Selection(term, index);
        }

        // constants
        public static ValueTerm Constant(IEnumerable<double> values)
        {
            return Vector(values.Select(value => new Constant(value)));
        }
        public static ValueTerm Constant(params double[] values)
        {
            return Constant((IEnumerable<double>)values);
        }

        // identity function
        public static FunctionTerm Identity(int dimension)
        {
            Variable x = new Variable(dimension, "x");

            return new FunctionDefinition(string.Format("identity_{0}", dimension), x.Abstract(x), new BasicSyntax("id"));
        }

        // sum and product functions
        public static FunctionTerm Sum()
        {
            return new Sum();
        }
        public static FunctionTerm VectorSum(int dimension)
        {
            if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");

            Variable x = new Variable(dimension, "x");
            Variable y = new Variable(dimension, "y");

            return new FunctionDefinition
            (
                string.Format("vector_sum_{0}", dimension),
                Vector
                (
                    from index in Enumerable.Range(0, dimension)
                    select ApplySum(x.Select(index), y.Select(index))
                )
                .Abstract(x, y),
                new BasicBinaryOperatorSyntax("⊕")
            );
        }
        public static FunctionTerm Product()
        {
            return new Product();
        }
        public static FunctionTerm DotProduct(int dimension)
        {
            if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");

            Variable x = new Variable(dimension, "x");
            Variable y = new Variable(dimension, "y");

            return new FunctionDefinition
            (
                string.Format("dot_product_{0}", dimension),
                BigSum(1, dimension)
                .Apply
                (
                    from index in Enumerable.Range(0, dimension)
                    select ApplyProduct(x.Select(index), y.Select(index))
                )
                .Abstract(x, y),
                new BasicBinaryOperatorSyntax("⊙")
            );
        }
        public static FunctionTerm VectorScaling(int dimension)
        {
            if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");

            Variable c = new Variable(1, "c");
            Variable x = new Variable(dimension, "x");

            return new FunctionDefinition
            (
                string.Format("vector_scaling_{0}", dimension),
                Vector
                (
                    from index in Enumerable.Range(0, dimension)
                    select ApplyProduct(c, x.Select(index))
                )
                .Abstract(c, x),
                new BasicBinaryOperatorSyntax("⊛")
            );
        }
        public static FunctionTerm BigSum(int dimension, int length)
        {
            if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
            if (length < 0) throw new ArgumentOutOfRangeException("length");

            ValueTerm seed = Constant(Enumerable.Repeat<double>(0, dimension));
            IEnumerable<Variable> variables =
            (
                from index in Enumerable.Range(0, length)
                select new Variable(dimension, string.Format("x_{0}", index))
            )
            .ToArray();

            return new FunctionDefinition
            (
                string.Format("big_sum_{0}_{1}", dimension, length),
                variables.Aggregate(seed, ApplyVectorSum).Abstract(variables),
                new BasicSyntax("Σ")
            );
        }
        public static FunctionTerm BigProduct(int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length");

            ValueTerm seed = Constant(1);
            IEnumerable<Variable> variables =
            (
                from index in Enumerable.Range(0, length)
                select new Variable(1, string.Format("x_{0}", index))
            )
            .ToArray();

            return new FunctionDefinition
            (
                string.Format("big_product_{0}", length),
                variables.Aggregate(seed, ApplyProduct).Abstract(variables),
                new BasicSyntax("π")
            );
        }

        // sum and product application helpers
        static ValueTerm ApplySum(ValueTerm value1, ValueTerm value2)
        {
            return Sum().Apply(value1, value2);
        }
        static ValueTerm ApplyVectorSum(ValueTerm value1, ValueTerm value2)
        {
            int dimension = Items.Equal(value1.Dimension, value2.Dimension);

            return VectorSum(dimension).Apply(value1, value2);
        }
        static ValueTerm ApplyProduct(ValueTerm value1, ValueTerm value2)
        {
            return Product().Apply(value1, value2);
        }
        static ValueTerm ApplyDotProduct(ValueTerm value1, ValueTerm value2)
        {
            int dimension = Items.Equal(value1.Dimension, value2.Dimension);

            return DotProduct(dimension).Apply(value1, value2);
        }
        static ValueTerm ApplyVectorScaling(ValueTerm factor, ValueTerm value)
        {
            int dimension = value.Dimension;

            return VectorScaling(dimension).Apply(factor, value);
        }
        static ValueTerm ApplyBigSum(IEnumerable<ValueTerm> values)
        {
            if (!values.Any()) throw new ArgumentException("Parameter 'values' did not contain any items.");

            int dimension = values.Select(value => value.Dimension).Distinct().Single();
            int length = values.Count();

            return BigSum(dimension, length).Apply(values);
        }
        static ValueTerm ApplyBigProduct(IEnumerable<ValueTerm> values)
        {
            if (!values.Any()) throw new ArgumentException("Parameter 'values' did not contain any items.");

            int length = values.Count();

            return BigProduct(length).Apply(values);
        }

        // sum and product application public interface
        public static ValueTerm Sum(IEnumerable<ValueTerm> values)
        {
            return ApplyBigSum(values);
        }
        public static ValueTerm Sum(params ValueTerm[] values)
        {
            return Sum((IEnumerable<ValueTerm>)values);
        }
        public static ValueTerm Negate(ValueTerm value)
        {
            return Scaling(Constant(-1), value);
        }
        public static ValueTerm Difference(ValueTerm value1, ValueTerm value2)
        {
            return Sum(value1, Negate(value2));
        }
        public static ValueTerm DotProduct(ValueTerm value1, ValueTerm value2)
        {
            return ApplyDotProduct(value1, value2);
        }
        public static ValueTerm Scaling(ValueTerm factor, ValueTerm value)
        {
            return ApplyVectorScaling(factor, value);
        }
        public static ValueTerm Product(IEnumerable<ValueTerm> values)
        {
            return ApplyBigProduct(values);
        }
        public static ValueTerm Product(params ValueTerm[] values)
        {
            return Product((IEnumerable<ValueTerm>)values);
        }
        public static ValueTerm Invert(ValueTerm value)
        {
            return Exponentiation(value, Constant(-1));
        }
        public static ValueTerm Quotient(ValueTerm value1, ValueTerm value2)
        {
            return Product(value1, Invert(value2));
        }

        // exponentiation
        public static FunctionTerm Exponentiation()
        {
            return new Exponentiation();
        }
        public static ValueTerm Exponentiation(ValueTerm @base, ValueTerm exponent)
        {
            return Exponentiation().Apply(@base, exponent);
        }
        public static ValueTerm Square(ValueTerm value)
        {
            return Exponentiation(value, Constant(2));
        }

        // logarithm
        public static FunctionTerm Logarithm()
        {
            return new Logarithm();
        }
        public static ValueTerm Logarithm(ValueTerm value)
        {
            return Logarithm().Apply(value);
        }

        // norm
        public static FunctionTerm Norm(int dimension)
        {
            if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");

            Variable x = new Variable(dimension, "x");

            return new FunctionDefinition
            (
                string.Format("norm_{0}", dimension),
                Exponentiation(DotProduct(x, x), Constant(0.5)).Abstract(x),
                new NormSyntax()
            );
        }
        public static ValueTerm Norm(ValueTerm value)
        {
            int dimension = value.Dimension;

            return Norm(dimension).Apply(value);
        }

        // polynomial
        public static FunctionTerm Polynomial(int dimension, int degree)
        {
            if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
            if (degree < 0) throw new ArgumentOutOfRangeException("degree");

            Variable variable = new Variable(1, "x");
            IEnumerable<Variable> coefficients =
            (
                from index in Enumerable.Range(0, degree)
                select new Variable(dimension, string.Format("c{0}", index))
            )
            .ToArray();

            return Sum
            (
                from index in Enumerable.Range(0, degree)
                let power = Exponentiation(variable, Constant(index))
                let parameter = coefficients.ElementAt(index)
                select Scaling(power, parameter)
            )
			.Abstract(Enumerables.Concatenate(Enumerables.Create(variable), coefficients));
        }
    }
}
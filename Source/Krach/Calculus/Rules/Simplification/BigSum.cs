using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using Krach.Calculus.Terms.Basic.Definitions;
using System.Text.RegularExpressions;

namespace Krach.Calculus.Rules.Simplification
{
    public class BigSum
    {
        public class Nullary : Rule
        {
            public override string ToString()
            {
                return "big_sum_*_0";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^big_sum_(\d+)_0$");

                if (!match.Success) return null;

                int dimension = int.Parse(match.Groups[1].Value);

                return (T)(BaseTerm)Term.Constant(Enumerable.Repeat<double>(0, dimension)).Abstract();
            }
        }
        public class Unary : Rule
        {
            public override string ToString()
            {
                return "big_sum_*_1";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^big_sum_(\d+)_1$");

                if (!match.Success) return null;

                int dimension = int.Parse(match.Groups[1].Value);

                return (T)(BaseTerm)Term.Identity(dimension);
            }
        }
        public class Binary : Rule
        {
            public override string ToString()
            {
                return "big_sum_*_2";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^big_sum_(\d+)_2$");

                if (!match.Success) return null;

                int dimension = int.Parse(match.Groups[1].Value);

                return (T)(BaseTerm)Term.VectorSum(dimension);
            }
        }
    }
}


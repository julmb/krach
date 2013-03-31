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
    public class BigProduct
    {
        public class Nullary : Rule
        {
            public override string ToString()
            {
                return "big_product_0";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^big_product_0$");

                if (!match.Success) return null;

                return (T)(BaseTerm)Term.Constant(1).Abstract();
            }
        }
        public class Unary : Rule
        {
            public override string ToString()
            {
                return "big_product_1";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^big_product_1$");

                if (!match.Success) return null;

                return (T)(BaseTerm)Term.Identity(1);
            }
        }
        public class Binary : Rule
        {
            public override string ToString()
            {
                return "big_product_2";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^big_product_2$");

                if (!match.Success) return null;

                return (T)(BaseTerm)Term.Product();
            }
        }
    }
}


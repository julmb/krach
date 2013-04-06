using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using System.Text.RegularExpressions;
using Krach.Calculus.Terms.Basic.Definitions;

namespace Krach.Calculus.Rules.Simplification
{
    public class DotProduct
    {
        public class ZeroDimensional : Rule
        {
            public override string ToString()
            {
                return "dot_product_0";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^dot_product_0$");

                if (!match.Success) return null;

                return (T)(BaseTerm)Term.Constant(1).Abstract();
            }
        }
        public class OneDimensional : Rule
        {
            public override string ToString()
            {
                return "dot_product_1";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)(BaseTerm)term;

                Match match = Regex.Match(functionDefinition.Name, @"^dot_product_1$");

                if (!match.Success) return null;

                return (T)(BaseTerm)Term.Product();
            }
        }
    }
}


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
    public class Identity
    {
        public class Application : Rule
        {
            public override string ToString()
            {
                return "identity_*!";
            }
            public override T Rewrite<T>(T term)
            {
                if (!(term is Terms.Composite.Application)) return null;

                Terms.Composite.Application application = (Terms.Composite.Application)(BaseTerm)term;

                if (!(application.Function is FunctionDefinition)) return null;

                FunctionDefinition functionDefinition = (FunctionDefinition)application.Function;

                Match match = Regex.Match(functionDefinition.Name, @"^identity_(\d+)$");

                if (!match.Success) return null;

                return (T)(BaseTerm)application.Parameter;
            }
        }
    }
}

